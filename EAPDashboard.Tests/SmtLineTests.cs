using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAPDashboard.Models;
using EAPDashboard.Models.Smt;
using EAPDashboard.Services;
using FluentAssertions;
using Protocol.MES.Base;
using Protocol.MES.Messages;
using Xunit;

namespace EAPDashboard.Tests
{
    public class SmtLineTests
    {
        private LineConfig CreateTestLineConfig()
        {
            return new LineConfig
            {
                LineId = "SMT_LINE_TEST",
                LineName = "SMT 測試產線 1",
                LineType = LineTypes.SMT,
                BatchCapacity = 10,
                Station1 = new SmtStationConfig
                {
                    StationCode = "ST1",
                    StationName = "第一站",
                    EquipmentIds = new List<string> { "EQ_SMT_01" }
                },
                Station2 = new SmtStationConfig
                {
                    StationCode = "ST2",
                    StationName = "第二站 (AOI)",
                    EquipmentIds = new List<string> { "EQ_AOI_01" }
                }
            };
        }

        [Fact]
        public void ComponentBatchInRequest_InspectProperties()
        {
            var req = new ComponentBatchInRequest();
            var props = req.GetType().GetProperties().Select(p => p.Name).ToList();
            props.Should().Contain("WONO");
            props.Should().Contain("EqpNo");
            props.Should().Contain("UserID");
        }

        [Fact]
        public async Task Station1_PanelIn_SameWorkOrder_ShouldCallTrackInOnceAndComponentInEveryTime()
        {
            var config = CreateTestLineConfig();
            var sentMessages = new List<MESBaseMessage>();

            var service = new SmtLineService(config, req =>
            {
                sentMessages.Add(req);
                return Task.FromResult<MESBaseMessage>(null);
            });

            // 1. 第一片 Panel 進站 (WO01)
            bool r1 = await service.HandleStation1PanelInAsync("WO01_001");
            r1.Should().BeTrue();

            // 2. 第二片 Panel 進站 (WO01)
            bool r2 = await service.HandleStation1PanelInAsync("WO01_002");
            r2.Should().BeTrue();

            // 驗證 MES 訊息:
            // 第 1 片觸發: WOCHECKINRequest (WO01) + ComponentBatchInRequest
            // 第 2 片觸發: 僅 ComponentBatchInRequest (不重複 Track-In)
            var checkIns = sentMessages.OfType<WOCHECKINRequest>().ToList();
            checkIns.Should().HaveCount(1);
            checkIns[0].WONO.Should().Be("WO01");

            var compIns = sentMessages.OfType<ComponentBatchInRequest>().ToList();
            compIns.Should().HaveCount(2);

            var state = service.GetOrCreateWorkOrderState("WO01");
            state.Station1_In_Count.Should().Be(2);
        }

        [Fact]
        public async Task Station1_PanelIn_WorkOrderChanged_ShouldAutoCheckoutPreviousWoTailBatch()
        {
            var config = CreateTestLineConfig();
            var sentMessages = new List<MESBaseMessage>();

            var service = new SmtLineService(config, req =>
            {
                sentMessages.Add(req);
                return Task.FromResult<MESBaseMessage>(null);
            });

            // 1. 工單 A 進站 3 片，並出站 3 片 (未滿 10 片尾數)
            await service.HandleStation1PanelInAsync("WO_A_001");
            await service.HandleStation1PanelOutAsync("WO_A_001");
            await service.HandleStation1PanelInAsync("WO_A_002");
            await service.HandleStation1PanelOutAsync("WO_A_002");
            await service.HandleStation1PanelInAsync("WO_A_003");
            await service.HandleStation1PanelOutAsync("WO_A_003");

            var woA = service.GetOrCreateWorkOrderState("WO_A");
            woA.Station1_Finished.Should().BeFalse();
            woA.Station1_Out_Count.Should().Be(3);

            // 2. 此時工單 B 首片進站 (觸發換單防呆)
            bool rNew = await service.HandleStation1PanelInAsync("WO_B_001");
            rNew.Should().BeTrue();

            // 驗證:
            // (1) WO_A 應被自動觸發 Track-Out，Actual_QTY = 3，且標記 Finished
            woA.Station1_Finished.Should().BeTrue();
            woA.Station1_Actual_QTY.Should().Be(3);

            var checkOuts = sentMessages.OfType<WOCHECKOUTRequest>().ToList();
            checkOuts.Should().ContainSingle(c => c.WONO == "WO_A" && c.OutPut == "3");

            // (2) WO_B 應被觸發 Track-In
            var checkIns = sentMessages.OfType<WOCHECKINRequest>().ToList();
            checkIns.Should().Contain(c => c.WONO == "WO_B");
        }

        [Fact]
        public async Task Station1_PanelOut_FullBatch_ShouldTriggerTrackOutAndRecordActualQty()
        {
            var config = CreateTestLineConfig();
            config.BatchCapacity = 10;
            var sentMessages = new List<MESBaseMessage>();

            var service = new SmtLineService(config, req =>
            {
                sentMessages.Add(req);
                return Task.FromResult<MESBaseMessage>(null);
            });

            // 模擬 10 片依序 Panel Out
            for (int i = 1; i <= 9; i++)
            {
                await service.HandleStation1PanelOutAsync($"WO_FULL_{i:D3}");
                sentMessages.OfType<WOCHECKOUTRequest>().Should().BeEmpty();
            }

            // 第 10 片
            await service.HandleStation1PanelOutAsync("WO_FULL_010");

            var wo = service.GetOrCreateWorkOrderState("WO_FULL");
            wo.Station1_Out_Count.Should().Be(10);
            wo.Station1_Finished.Should().BeTrue();
            wo.Station1_Actual_QTY.Should().Be(10);

            var checkOuts = sentMessages.OfType<WOCHECKOUTRequest>().ToList();
            checkOuts.Should().HaveCount(1);
            checkOuts[0].WONO.Should().Be("WO_FULL");
            checkOuts[0].OutPut.Should().Be("10");
        }

        [Fact]
        public async Task Station2_PanelOut_MatchedStation1FinalQty_ShouldTriggerTrackOut()
        {
            // 核心使用者情境:
            // 第一站在工單結單出站時記憶實際通過數量 6 (例如因抽片)，第二站累積刷到第 6 片時，
            // 即使未達滿批次 10 片，也應因等於第一站最終統計數量而觸發 MES 工單出站。
            var config = CreateTestLineConfig();
            config.BatchCapacity = 10;
            var sentMessages = new List<MESBaseMessage>();

            var service = new SmtLineService(config, req =>
            {
                sentMessages.Add(req);
                return Task.FromResult<MESBaseMessage>(null);
            });

            // 1. 第一站生產 6 片後結單
            for (int i = 1; i <= 6; i++)
            {
                await service.HandleStation1PanelOutAsync($"WO_SAMPLE_{i:D3}");
            }
            // 透過 WebUI 或換單結單，使第一站 Final Qty = 6
            var forceRes = await service.ForceTrackOutAsync("1", "WO_SAMPLE");
            forceRes.Success.Should().BeTrue();
            forceRes.ActualQty.Should().Be(6);

            var woState = service.GetOrCreateWorkOrderState("WO_SAMPLE");
            woState.Station1_Finished.Should().BeTrue();
            woState.Station1_Actual_QTY.Should().Be(6);

            // 2. 第二站開始刷 Panel In 與 Out
            sentMessages.Clear(); // 清空先前紀錄

            await service.HandleStation2PanelInAsync("WO_SAMPLE_001");
            sentMessages.OfType<WOCHECKINRequest>().Should().ContainSingle(c => c.WONO == "WO_SAMPLE");

            // 第二站依序出站 1 ~ 5 片
            for (int i = 1; i <= 5; i++)
            {
                await service.HandleStation2PanelOutAsync($"WO_SAMPLE_{i:D3}");
                sentMessages.OfType<WOCHECKOUTRequest>().Should().BeEmpty();
            }

            // 第 6 片出站 -> 達到第一站結單數量 6
            await service.HandleStation2PanelOutAsync("WO_SAMPLE_006");

            woState.Station2_Out_Count.Should().Be(6);
            woState.Station2_Finished.Should().BeTrue();
            woState.Station2_Actual_QTY.Should().Be(6);

            var st2CheckOuts = sentMessages.OfType<WOCHECKOUTRequest>().ToList();
            st2CheckOuts.Should().HaveCount(1);
            st2CheckOuts[0].WONO.Should().Be("WO_SAMPLE");
            st2CheckOuts[0].OutPut.Should().Be("6");
        }

        [Fact]
        public async Task WebUI_ForceTrackOut_ShouldFinishWorkOrderImmediately()
        {
            var config = CreateTestLineConfig();
            var sentMessages = new List<MESBaseMessage>();

            var service = new SmtLineService(config, req =>
            {
                sentMessages.Add(req);
                return Task.FromResult<MESBaseMessage>(null);
            });

            // 第一站累計出站 4 片
            for (int i = 1; i <= 4; i++)
            {
                await service.HandleStation1PanelOutAsync($"WO_FORCE_{i:D3}");
            }

            // 人員透過 WebUI 強制出站
            var (success, message, actualQty) = await service.ForceTrackOutAsync("1", "WO_FORCE", "OP_ADMIN");

            success.Should().BeTrue();
            actualQty.Should().Be(4);

            var state = service.GetOrCreateWorkOrderState("WO_FORCE");
            state.Station1_Finished.Should().BeTrue();
            state.Station1_Actual_QTY.Should().Be(4);

            var checkOuts = sentMessages.OfType<WOCHECKOUTRequest>().ToList();
            checkOuts.Should().ContainSingle(c => c.WONO == "WO_FORCE" && c.OutPut == "4");
        }
    }
}
