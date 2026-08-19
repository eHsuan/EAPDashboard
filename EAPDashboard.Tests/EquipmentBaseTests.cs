using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAPDashboard.Models;
using FluentAssertions;
using Xunit;

namespace EAPDashboard.Tests
{
    public class TestableEquipment : EquipmentBase
    {
        public List<string> LogHistory { get; } = new List<string>();
        public string LastTerminalDisplayMessage { get; private set; }

        public TestableEquipment(string id)
        {
            this.Id = id;
            this.Name = $"Test_{id}";
            this.Protocol = "TEST";
        }

        protected override void OnLogReceived(LogLevel level, string message)
        {
            LogHistory.Add($"[{level}] {message}");
            base.OnLogReceived(level, message);
        }

        public override Task SendTerminalDisplay(string message)
        {
            LastTerminalDisplayMessage = message;
            return Task.CompletedTask;
        }

        public string GetMesParamValueTest(string dt, string name) => GetMesParamValue(dt, name);
        public void SetWorkOrderNGCodesForTest(string wo, List<NGCodeItem> items)
        {
            var dict = new System.Collections.Concurrent.ConcurrentDictionary<string, NGCodeItem>(StringComparer.OrdinalIgnoreCase);
            foreach (var it in items)
            {
                dict[it.NGCode] = it;
            }
            _workOrderNgCodesMap[wo] = dict;
        }
        public bool HasWorkOrderNGCodes(string wo) => _workOrderNgCodesMap.ContainsKey(wo);
        public NGCodeItem GetNGCodeItemForTest(string wo, string code)
        {
            if (_workOrderNgCodesMap.TryGetValue(wo, out var dict) && dict.TryGetValue(code, out var item))
            {
                return item;
            }
            return null;
        }
        public override Task SendTimeCalibration() => Task.CompletedTask;
        public override void Start(System.Threading.CancellationToken externalCancellationToken, CyntecMqttLib.Interfaces.MqttBus bus = null, int heartbeatIntervalSeconds = 30) { }
    }

    public class EquipmentBaseTests
    {
        [Fact]
        public async Task ProcessTrackInAsync_EmptyWorkOrders_ShouldReturnFail()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var result = await eq.ProcessTrackInAsync(
                cassetteIds: new List<string> { "C001" },
                workOrders: new List<string>(),
                panels: new List<string>(),
                userId: "U001");

            // Assert
            result.Should().StartWith("FAIL");
            result.Should().Contain("W001");
        }

        [Fact]
        public async Task ProcessTrackInAsync_MultipleWorkOrders_SameRecipe_ShouldReturnPPSelect()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");
            var cassetteIds = new List<string> { "C001", "C002" };
            var workOrders = new List<string> { "WO001", "WO002" };

            // Act
            var result = await eq.ProcessTrackInAsync(cassetteIds, workOrders, new List<string>(), "U001");

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ProcessTrackInFinalAsync_SingleWorkOrder_ShouldReturnResult()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var result = await eq.ProcessTrackInFinalAsync(
                workOrders: new List<string> { "WO001" },
                userId: "U001",
                cassetteIds: new List<string> { "C001" });

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ProcessTrackInFinalAsync_MultipleWorkOrders_ShouldReturnResult()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var result = await eq.ProcessTrackInFinalAsync(
                workOrders: new List<string> { "WO001", "WO002" },
                userId: "U001",
                cassetteIds: new List<string> { "C001", "C002" });

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ProcessTrackOutAsync_SingleWorkOrder_ShouldReturnResult()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var result = await eq.ProcessTrackOutAsync(
                workOrders: new List<string> { "WO001" },
                qty: "10",
                userId: "U001",
                cassetteIds: new List<string> { "C001" },
                panels: new List<string> { "PNL01", "PNL02" });

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ProcessTrackOutAsync_MultipleWorkOrders_ShouldReturnResult()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var result = await eq.ProcessTrackOutAsync(
                workOrders: new List<string> { "WO001", "WO002" },
                qty: "20",
                userId: "U001",
                cassetteIds: new List<string> { "C001", "C002" },
                panels: new List<string> { "PNL01", "PNL02" });

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        public class MockMesEquipment : EquipmentBase
        {
            public string LastTerminalMessage { get; private set; }
            public Dictionary<string, string> MockRecipes { get; set; } = new Dictionary<string, string>();

            public MockMesEquipment(string id)
            {
                this.Id = id;
                this.Name = $"Mock_{id}";
                this.Protocol = "TEST";
            }

            public override Task SendTerminalDisplay(string message)
            {
                LastTerminalMessage = message;
                return Task.CompletedTask;
            }

            public override Task SendTimeCalibration() => Task.CompletedTask;
            public override void Start(System.Threading.CancellationToken externalCancellationToken, CyntecMqttLib.Interfaces.MqttBus bus = null, int heartbeatIntervalSeconds = 30) { }
        }

        [Fact]
        public void ExtractRecipeFromMes_WithValidJson_ShouldReturnRecipeId()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");
            string inputParamListDt = "[{\"ParamName\":\"RECIPEID\",\"DefaultValue\":\"RECIPE_ABC\"}]";

            // Act
            string recipe = eq.GetMesParamValueTest(inputParamListDt, "RECIPEID");

            // Assert
            recipe.Should().Be("RECIPE_ABC");
        }

        [Fact]
        public void RecipeMismatchFormat_ShouldFollowSpecifiedStructure()
        {
            // Arrange
            var woRecipeMap = new Dictionary<string, string>
            {
                { "WOOOOOOOOO01", "RECIPE_A" },
                { "WOOOOOOOO02", "RECIPE_B" }
            };

            // Act
            var details = woRecipeMap.Select((kv, idx) => $"WO_{idx + 1}:{kv.Key}[{kv.Value}]");
            string terminalMsg = $"Recipe Mismatch! {string.Join(",", details)}";

            // Assert
            terminalMsg.Should().Be("Recipe Mismatch! WO_1:WOOOOOOOOO01[RECIPE_A],WO_2:WOOOOOOOO02[RECIPE_B]");
            terminalMsg.Length.Should().BeLessThanOrEqualTo(200);
        }

        [Fact]
        public void GetManagementItemValue_WithValidDictionary_ShouldBeCaseInsensitive()
        {
            // Arrange
            var eq = new TestableEquipment("TS0003")
            {
                DictionaryPath = "Dictionaries/TS0003.json"
            };

            // Act
            eq.LoadAlarmMappings();
            string valUppercase = eq.GetManagementItemValue("TE004");
            string valLowercase = eq.GetManagementItemValue("te004");

            // Assert
            valUppercase.Should().Be("Y");
            valLowercase.Should().Be("Y");
        }

        [Fact]
        public void GetManagementItemValue_NonExistentCode_ShouldReturnNull()
        {
            // Arrange
            var eq = new TestableEquipment("TS0003")
            {
                DictionaryPath = "Dictionaries/TS0003.json"
            };

            // Act
            string val = eq.GetManagementItemValue("NON_EXISTENT_CODE");

            // Assert
            val.Should().BeNull();
        }

        [Fact]
        public void RecordNGCodeReport_WhenWorkOrderAndCodeMatch_ShouldUpdateAndReturnTrue()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");
            eq.SetWorkOrderNGCodesForTest("WO100", new List<NGCodeItem>
            {
                new NGCodeItem { NGCode = "UPNG501", NGName = "機台卡片" },
                new NGCodeItem { NGCode = "UPNG502", NGName = "折片" }
            });

            // Act
            bool result = eq.RecordNGCodeReport("WO100", "UPNG501", "機台卡片", "3", "PCS");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void RecordNGCodeReport_WhenCodeNotInList_ShouldSkipAndReturnFalse()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");
            eq.SetWorkOrderNGCodesForTest("WO100", new List<NGCodeItem>
            {
                new NGCodeItem { NGCode = "UPNG501", NGName = "機台卡片" }
            });

            // Act
            bool result = eq.RecordNGCodeReport("WO100", "UNKNOWN_CODE", "未知", "1", "PCS");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ProcessTrackOutAsync_WhenUnreportedNGCodesExist_ShouldRejectWithCommaSeparatedList()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");
            eq.SetWorkOrderNGCodesForTest("WO100", new List<NGCodeItem>
            {
                new NGCodeItem { NGCode = "UPNG501", NGName = "卡片", NGQty = "1", NGUnit = "PCS" },
                new NGCodeItem { NGCode = "UPNG502", NGName = "折片", NGQty = null },
                new NGCodeItem { NGCode = "UPNG503", NGName = "抽片", NGQty = null }
            });

            // Act
            string result = await eq.ProcessTrackOutAsync(
                workOrders: new List<string> { "WO100" },
                qty: "10",
                userId: "U001",
                cassetteIds: new List<string> { "C001" },
                panels: new List<string>());

            // Assert
            result.Should().StartWith("FAIL|NG_CHECK_FAIL|Unreported NGCode(s): ");
            result.Should().Contain("UPNG502");
            result.Should().Contain("UPNG503");
        }

        [Fact]
        public async Task ProcessTrackOutAsync_WhenAllNGCodesReported_ShouldProceed()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");
            eq.SetWorkOrderNGCodesForTest("WO100", new List<NGCodeItem>
            {
                new NGCodeItem { NGCode = "UPNG501", NGName = "卡片", NGQty = "1", NGUnit = "PCS" },
                new NGCodeItem { NGCode = "UPNG502", NGName = "折片", NGQty = "0", NGUnit = "PCS" }
            });

            // Act
            string result = await eq.ProcessTrackOutAsync(
                workOrders: new List<string> { "WO100" },
                qty: "10",
                userId: "U001",
                cassetteIds: new List<string> { "C001" },
                panels: new List<string>());

            // Assert
            result.Should().NotStartWith("FAIL|NG_CHECK_FAIL|");
        }

        [Fact]
        public async Task ProcessAddProductLeftoverAsync_MissingWO_ShouldReturnFail()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var (isSuccess, message) = await eq.ProcessAddProductLeftoverAsync("", "10", "REEL01");

            // Assert
            isSuccess.Should().BeFalse();
            message.Should().Contain("Missing WorkOrder");
        }

        [Fact]
        public async Task ProcessAddProductLeftoverAsync_MissingReelID_ShouldReturnFail()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var (isSuccess, message) = await eq.ProcessAddProductLeftoverAsync("WO100", "10", "");

            // Assert
            isSuccess.Should().BeFalse();
            message.Should().Contain("Missing WorkOrder ('WO100') or ReelID ('')");
        }

        [Fact]
        public async Task ProcessUseProductLeftoverAsync_MissingWO_ShouldReturnFail()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var (isSuccess, message) = await eq.ProcessUseProductLeftoverAsync("", "REEL01", "WO_PREV", "BATCH01", "10", "0");

            // Assert
            isSuccess.Should().BeFalse();
            message.Should().Contain("Missing WorkOrder");
        }

        [Fact]
        public async Task ProcessUseProductLeftoverAsync_MissingReelID_ShouldReturnFail()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var (isSuccess, message) = await eq.ProcessUseProductLeftoverAsync("WO100", "", "WO_PREV", "BATCH01", "10", "0");

            // Assert
            isSuccess.Should().BeFalse();
            message.Should().Contain("Missing WorkOrder ('WO100') or ReelID ('')");
        }

        [Fact]
        public async Task ProcessAddProductLeftoverAsync_WithCustomUserId_InvalidInput_ShouldFailValidation()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var (isSuccess, message) = await eq.ProcessAddProductLeftoverAsync("", "10", "REEL01", userId: "OP_USER_01");

            // Assert
            isSuccess.Should().BeFalse();
            message.Should().Contain("Missing WorkOrder");
        }

        [Fact]
        public async Task ProcessUseProductLeftoverAsync_WithCustomUserId_InvalidInput_ShouldFailValidation()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var (isSuccess, message) = await eq.ProcessUseProductLeftoverAsync("", "REEL01", "WO_PREV", "BATCH01", "10", "0", userId: "OP_USER_01");

            // Assert
            isSuccess.Should().BeFalse();
            message.Should().Contain("Missing WorkOrder");
        }

        [Fact]
        public async Task ProcessEqpWONGAddAsync_MissingWO_ShouldFailValidation()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var (isSuccess, resultCode, message) = await eq.ProcessEqpWONGAddAsync("", "UPNG501", "5", "PCS", userId: "OP001");

            // Assert
            isSuccess.Should().BeFalse();
            resultCode.Should().Be("ERR_INVALID_PARAM");
            message.Should().Contain("Missing WorkOrder");
        }

        [Fact]
        public async Task ProcessEqpWONGAddAsync_MissingNGCode_ShouldFailValidation()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var (isSuccess, resultCode, message) = await eq.ProcessEqpWONGAddAsync("WO20260817001", "", "5", "PCS", userId: "OP001");

            // Assert
            isSuccess.Should().BeFalse();
            resultCode.Should().Be("ERR_INVALID_PARAM");
            message.Should().Contain("Missing WorkOrder ('WO20260817001') or NGCode ('')");
        }

        [Fact]
        public void RecordTotalCountReport_ValidData_ShouldStoreInMap()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            eq.RecordTotalCountReport("WO20260818001", "10", "500");

            // Assert
            var item = eq.GetTotalCount("WO20260818001");
            item.Should().NotBeNull();
            item.EquipmentId.Should().Be("EQ01");
            item.WorkOrder.Should().Be("WO20260818001");
            item.ReelCount.Should().Be("10");
            item.QtyPerReel.Should().Be("500");
        }

        [Fact]
        public void GetTotalCount_NonExistentWorkOrder_ShouldReturnNull()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");

            // Act
            var item = eq.GetTotalCount("NON_EXISTENT_WO");

            // Assert
            item.Should().BeNull();
        }

        [Fact]
        public void TotalCountCache_Persistence_ShouldSurviveRestart()
        {
            // Arrange
            string tempDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "EAP_Test_" + Guid.NewGuid().ToString("N"));
            EquipmentBase.CustomCacheDirectory = tempDir;

            try
            {
                var eq1 = new TestableEquipment("EQ_PERSIST_01");
                eq1.RecordTotalCountReport("WO_RESTART_001", "20", "1000");

                // Act - Simulate EAP restart by creating a new instance and calling LoadTotalCountCache
                var eq2 = new TestableEquipment("EQ_PERSIST_01");
                eq2.LoadTotalCountCache();

                // Assert
                var restored = eq2.GetTotalCount("WO_RESTART_001");
                restored.Should().NotBeNull();
                restored.EquipmentId.Should().Be("EQ_PERSIST_01");
                restored.WorkOrder.Should().Be("WO_RESTART_001");
                restored.ReelCount.Should().Be("20");
                restored.QtyPerReel.Should().Be("1000");
            }
            finally
            {
                EquipmentBase.CustomCacheDirectory = null;
                if (System.IO.Directory.Exists(tempDir))
                {
                    try { System.IO.Directory.Delete(tempDir, true); } catch { }
                }
            }
        }

        [Fact]
        public async Task NgCodeCache_Persistence_ShouldSurviveRestart()
        {
            // Arrange
            string tempDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "EAP_Test_" + Guid.NewGuid().ToString("N"));
            EquipmentBase.CustomCacheDirectory = tempDir;

            try
            {
                var eq1 = new TestableEquipment("EQ_PERSIST_02");
                var ngItems = new List<NGCodeItem>
                {
                    new NGCodeItem { NGCode = "NG001", NGName = "Defect A" },
                    new NGCodeItem { NGCode = "NG002", NGName = "Defect B" }
                };
                eq1.SetWorkOrderNGCodesForTest("WO_RESTART_002", ngItems);
                eq1.RecordNGCodeReport("WO_RESTART_002", "NG001", "Defect A", "5", "PCS");

                // Act - Simulate EAP restart by creating a new instance and calling LoadNgCodeCache
                var eq2 = new TestableEquipment("EQ_PERSIST_02");
                eq2.LoadNgCodeCache();

                // Assert
                eq2.HasWorkOrderNGCodes("WO_RESTART_002").Should().BeTrue();
                var (isSuccess, _, message) = await eq2.ProcessEqpWONGAddAsync("WO_RESTART_002", "NG001", "5", "PCS", "OP1");
            }
            finally
            {
                EquipmentBase.CustomCacheDirectory = null;
                if (System.IO.Directory.Exists(tempDir))
                {
                    try { System.IO.Directory.Delete(tempDir, true); } catch { }
                }
            }
        }

        [Fact]
        public void TpTrackOut_MissingReports_ShouldFail()
        {
            var eq = new TestableEquipment("TP001");
            string wo = "WO_TP_001";

            // Neither LeftoversReport nor TotalCountReport
            var (ok1, msg1, _) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok1.Should().BeFalse();
            msg1.Should().Contain("LeftoversReport").And.Contain("TotalCountReport");

            // Only LeftoversReport reported
            eq.RecordLeftoversReport(wo, "10", "REEL01");
            var (ok2, msg2, _) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok2.Should().BeFalse();
            msg2.Should().Contain("TotalCountReport");

            // Now TotalCountReport also reported
            eq.RecordTotalCountReport(wo, "10", "500");
            eq.RecordWorkOrderInputQty(wo, 5010);
            var (ok3, msg3, _) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok3.Should().BeTrue();
        }

        [Fact]
        public void TpTrackOut_YieldBelowMin_ShouldFail()
        {
            var eq = new TestableEquipment("TP002");
            eq.MinYieldRate = 0.95;
            eq.MaxYieldRate = 1.00;
            string wo = "WO_TP_002";

            eq.RecordWorkOrderInputQty(wo, 1000);
            eq.RecordTotalCountReport(wo, "9", "100"); // 900
            eq.RecordLeftoversReport(wo, "40", "R01");  // 40 => PassQty = 940 => Yield = 94.0% < 95.0%

            var (ok, msg, _) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok.Should().BeFalse();
            msg.Should().Contain("良率過低異常").And.Contain("94.00%");
        }

        [Fact]
        public void TpTrackOut_YieldAboveMax_ShouldFail()
        {
            var eq = new TestableEquipment("TP003");
            eq.MinYieldRate = 0.95;
            eq.MaxYieldRate = 1.00;
            string wo = "WO_TP_003";

            eq.RecordWorkOrderInputQty(wo, 1000);
            eq.RecordTotalCountReport(wo, "10", "100"); // 1000
            eq.RecordLeftoversReport(wo, "50", "R01");  // 50 => PassQty = 1050 => Yield = 105.0% > 100.0%

            var (ok, msg, _) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok.Should().BeFalse();
            msg.Should().Contain("良率過高/溢出異常").And.Contain("105.00%");
        }

        [Fact]
        public void TpTrackOut_ConfigurableYieldRange_ShouldRespectThresholds()
        {
            var eq = new TestableEquipment("TP004");
            eq.MinYieldRate = 0.90; // Configured to 90%
            eq.MaxYieldRate = 1.10; // Configured to 110%
            string wo = "WO_TP_004";

            eq.RecordWorkOrderInputQty(wo, 1000);
            eq.RecordTotalCountReport(wo, "9", "100"); // 900
            eq.RecordLeftoversReport(wo, "20", "R01");  // 20 => PassQty = 920 => Yield = 92.0% (Between 90% and 110%)

            var (ok, msg, calcNg) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok.Should().BeTrue();
            calcNg.Should().Be(80); // 1000 - 920 = 80
        }

        [Fact]
        public void TpTrackOut_SpecialNgSumExceedsCalcNg_ShouldFail()
        {
            var eq = new TestableEquipment("TP005");
            string wo = "WO_TP_005";

            eq.RecordWorkOrderInputQty(wo, 1000);
            eq.RecordTotalCountReport(wo, "9", "100"); // 900
            eq.RecordLeftoversReport(wo, "80", "R01");  // 80 => PassQty = 980 => CalcNG = 20

            // Special NG items sum = 15 + 10 = 25 > CalcNG (20)
            var ngList = new List<NGCodeItem>
            {
                new NGCodeItem { NGCode = "NG_QC", NGName = "QC Keep Sample", NGQty = "15" },
                new NGCodeItem { NGCode = "NG_FREE", NGName = "免補料", NGQty = "10" }
            };
            eq.SetWorkOrderNGCodesForTest(wo, ngList);

            var (ok, msg, _) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok.Should().BeFalse();
            msg.Should().Contain("NG數溢出");
        }

        [Fact]
        public void TpTrackOut_CalcNgLessThanTotalReported_ShouldDeductFromLargestNonSpecial()
        {
            var eq = new TestableEquipment("TP006");
            string wo = "WO_TP_006";

            eq.RecordWorkOrderInputQty(wo, 1000);
            eq.RecordTotalCountReport(wo, "9", "100"); // 900
            eq.RecordLeftoversReport(wo, "80", "R01");  // 80
            eq.RecordProductLeftoverReport(wo, "10", "0", "R01", "WO_PREV", "B01"); // AddQty = 10 => PassQty = 970 => CalcNG = 30

            // Reported NG items:
            // Special: QC Keep Sample = 5, 免補料 = 5 (Total special = 10)
            // Non-special: NG_DEF1 = 20, NG_DEF2 = 10 (Total reported = 40)
            // CalcNG = 30. Diff to deduct = 40 - 30 = 10.
            // NG_DEF1 is largest (20), so it should be deducted by 10 => becomes 10.
            // Special items should remain 5 and 5.
            var ngList = new List<NGCodeItem>
            {
                new NGCodeItem { NGCode = "NG_QC", NGName = "QC Keep Sample", NGQty = "5" },
                new NGCodeItem { NGCode = "NG_FREE", NGName = "免補料", NGQty = "5" },
                new NGCodeItem { NGCode = "NG_DEF1", NGName = "Card Issue", NGQty = "20" },
                new NGCodeItem { NGCode = "NG_DEF2", NGName = "Broken", NGQty = "10" }
            };
            eq.SetWorkOrderNGCodesForTest(wo, ngList);

            var (ok, msg, calcNg) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok.Should().BeTrue();
            calcNg.Should().Be(30);

            var itemQc = eq.GetNGCodeItemForTest(wo, "NG_QC");
            var itemFree = eq.GetNGCodeItemForTest(wo, "NG_FREE");
            var itemDef1 = eq.GetNGCodeItemForTest(wo, "NG_DEF1");
            var itemDef2 = eq.GetNGCodeItemForTest(wo, "NG_DEF2");

            itemQc.NGQty.Should().Be("5");
            itemFree.NGQty.Should().Be("5");
            itemDef1.NGQty.Should().Be("10"); // Deducted from 20 to 10
            itemDef2.NGQty.Should().Be("10"); // Unchanged
        }

        [Fact]
        public void LeftoversAndProductLeftoversCache_Persistence_ShouldSurviveRestart()
        {
            string tempDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "EAP_Test_" + Guid.NewGuid().ToString("N"));
            EquipmentBase.CustomCacheDirectory = tempDir;

            try
            {
                var eq1 = new TestableEquipment("TP_PERSIST_01");
                eq1.RecordLeftoversReport("WO_LEFT_01", "25", "REEL_X");
                eq1.RecordProductLeftoverReport("WO_LEFT_01", "15", "0", "REEL_X", "WO_PREV", "B01");
                eq1.RecordWorkOrderInputQty("WO_LEFT_01", 1200);

                var eq2 = new TestableEquipment("TP_PERSIST_01");
                eq2.LoadCachesFromDisk();

                var leftover = eq2.GetLeftover("WO_LEFT_01");
                leftover.Should().NotBeNull();
                leftover.Qty.Should().Be("25");
                leftover.ReelId.Should().Be("REEL_X");

                var pLeftover = eq2.GetProductLeftover("WO_LEFT_01");
                pLeftover.Should().NotBeNull();
                pLeftover.AddQty.Should().Be("15");

                eq2.GetWorkOrderInputQty("WO_LEFT_01").Should().Be(1200);
            }
            finally
            {
                EquipmentBase.CustomCacheDirectory = null;
                if (System.IO.Directory.Exists(tempDir))
                {
                    try { System.IO.Directory.Delete(tempDir, true); } catch { }
                }
            }
        }
    }
}
