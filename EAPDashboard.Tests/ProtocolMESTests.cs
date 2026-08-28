using FluentAssertions;
using Protocol.MES.Enums;
using Protocol.MES.Messages;
using System.Linq;
using Xunit;

namespace EAPDashboard.Tests
{
    public class ProtocolMESTests
    {
        [Fact]
        public void WOQRYRequest_Initialization_ShouldSetCorrectTransactionName()
        {
            // Arrange & Act
            var req = new WOQRYRequest
            {
                WONO = "WO20260701",
                EqpNo = "EQ01",
                UserID = "U001"
            };

            // Assert
            req.TransactionName.Should().Be(MESTransactionName.WOQRY);
            req.WONO.Should().Be("WO20260701");
            req.EqpNo.Should().Be("EQ01");
        }

        [Fact]
        public void WOCHECKINRequest_Initialization_ShouldSetCorrectTransactionName()
        {
            // Arrange & Act
            var req = new WOCHECKINRequest
            {
                WONO = "WO20260701",
                EqpNo = "EQ01",
                UserID = "U001"
            };

            // Assert
            req.TransactionName.Should().Be(MESTransactionName.WOCHECKIN);
            req.WONO.Should().Be("WO20260701");
        }

        [Fact]
        public void WOCHECKOUTRequest_Initialization_ShouldSetCorrectTransactionName()
        {
            // Arrange & Act
            var req = new WOCHECKOUTRequest
            {
                WONO = "WO20260701",
                EqpNo = "EQ01",
                UserID = "U001",
                OutPut = "10"
            };

            // Assert
            req.TransactionName.Should().Be(MESTransactionName.WOCHECKOUT);
            req.WONO.Should().Be("WO20260701");
            req.OutPut.Should().Be("10");
        }

        [Fact]
        public void AddProductLeftoverRequest_Initialization_ShouldSetCorrectTransactionName()
        {
            var req = new Protocol.MES.Messages.AddProductLeftoverRequest
            {
                WONO = "WO20260701",
                Qty = "10",
                ReelID = "REEL001",
                EqpNo = "EQ01",
                UserID = "EAP"
            };

            req.TransactionName.Should().Be(MESTransactionName.AddProductLeftover);
            req.WONO.Should().Be("WO20260701");
            req.Qty.Should().Be("10");
            req.ReelID.Should().Be("REEL001");
        }

        [Fact]
        public void UseProductLeftoverRequest_Initialization_ShouldSetCorrectTransactionName()
        {
            var req = new Protocol.MES.Messages.UseProductLeftoverRequest
            {
                WONO = "WO20260701",
                ReelID = "REEL001",
                LeftOverWONo = "WO_PREV",
                LeftOverBatchNo = "BATCH01",
                AddQty = "10",
                GapQty = "0",
                EqpNo = "EQ01",
                UserID = "EAP"
            };

            req.TransactionName.Should().Be(MESTransactionName.UseProductLeftover);
            req.WONO.Should().Be("WO20260701");
            req.ReelID.Should().Be("REEL001");
        }

        [Fact]
        public void EqpWONGAddRequest_Initialization_ShouldSetCorrectTransactionName()
        {
            var req = new Protocol.MES.Messages.EqpWONGAddRequest
            {
                WONo = "WO20260817001",
                FlowID = "",
                EqpNo = "EQ01",
                UserID = "U001"
            };
            var list = new System.Collections.Generic.List<Protocol.MES.Messages.NGListItem>
            {
                new Protocol.MES.Messages.NGListItem { NGCode = "UPNG501", Value = "5", NGUnit = "PCS" }
            };
            req.NGListDt = Newtonsoft.Json.JsonConvert.SerializeObject(list);

            req.TransactionName.Should().Be(MESTransactionName.EqpWONGAdd);
            req.WONo.Should().Be("WO20260817001");
            req.FlowID.Should().BeEmpty();
            req.EqpNo.Should().Be("EQ01");
            req.UserID.Should().Be("U001");
            req.NGListDt.Should().Contain("UPNG501");
        }

        [Fact]
        public void UserVerifyRequest_Initialization_ShouldSetCorrectTransactionName()
        {
            var req = new UserVerifyRequest
            {
                User2DBarCode = "{{{{123456{Dept",
                UserID = "123456",
                EqpNo = "EQ01"
            };

            req.TransactionName.Should().Be(MESTransactionName.UserVerify);
            req.UserID.Should().Be("123456");
            req.User2DBarCode.Should().Be("{{{{123456{Dept");
            req.EqpNo.Should().Be("EQ01");
        }

        [Fact]
        public void WOCHECKIN_MULTIRequest_WithWhitelist_ShouldOmitNullAndEmptyFields()
        {
            // Arrange: 確保白名單包含 WOCHECKIN_MULTI
            Models.MesJsonContractResolver.ConfigureDefaultSettings();
            Models.MesJsonContractResolver.SetWhitelist(new[] { "WOCHECKIN_MULTI", "WOCHECKOUT_MULTI" });

            var req = new WOCHECKIN_MULTIRequest
            {
                WoNoDt = "[{\"WoNo\":\"WO20260701\"}]",
                EqpNo = "EQ01",
                UserID = "U001",
                WOCarrierDt = "[]",
                InputListDt = "   "
            };

            // Act
            string json = Models.MesJsonContractResolver.Serialize(req, Newtonsoft.Json.Formatting.None);

            // Assert
            json.Should().Contain("\"WoNoDt\":\"[{\\\"WoNo\\\":\\\"WO20260701\\\"}]\"");
            json.Should().Contain("\"EqpNo\":\"EQ01\"");
            json.Should().Contain("\"UserID\":\"U001\"");
            json.Should().Contain("\"TransactionName\":\"WOCHECKIN_MULTI\"");

            // 確保所有 null、空字串與 "[]" 欄位均被完全剔除
            json.Should().NotContain("InputListDt");
            json.Should().NotContain("WOCarrierDt");
            json.Should().NotContain("CheckItemDt");
            json.Should().NotContain("ParmListDt");
            json.Should().NotContain("EqpItemDt");
            json.Should().NotContain("UserPassword");
            json.Should().NotContain("User2DBarCode");
        }

        [Fact]
        public void WOCHECKOUT_MULTIRequest_WithWhitelist_ShouldOmitNullAndEmptyFields()
        {
            // Arrange: 確保白名單包含 WOCHECKOUT_MULTI
            Models.MesJsonContractResolver.ConfigureDefaultSettings();
            Models.MesJsonContractResolver.SetWhitelist(new[] { "WOCHECKIN_MULTI", "WOCHECKOUT_MULTI" });

            var req = new WOCHECKOUT_MULTIRequest
            {
                WoNoDt = "[{\"WoNo\":\"WO20260701\"}]",
                EqpNo = "EQ01",
                UserID = "U001",
                Operator = "U001",
                OutPut = "100"
            };

            // Act
            string json = Models.MesJsonContractResolver.Serialize(req, Newtonsoft.Json.Formatting.None);

            // Assert
            json.Should().Contain("\"WoNoDt\":\"[{\\\"WoNo\\\":\\\"WO20260701\\\"}]\"");
            json.Should().Contain("\"OutPut\":\"100\"");
            json.Should().Contain("\"Operator\":\"U001\"");
            json.Should().NotContain("NGListDt");
            json.Should().NotContain("InputListDt");
            json.Should().NotContain("CheckItemDt");
            json.Should().NotContain("ParmListDt");
            json.Should().NotContain("EqpItemDt");
            json.Should().NotContain("UserPassword");
            json.Should().NotContain("User2DBarCode");
        }

        [Fact]
        public void NonWhitelistedApi_SingleWOCHECKIN_ShouldPreserveDefaultFields()
        {
            // Arrange: 白名單只有 MULTI，不包含單工單 WOCHECKIN
            Models.MesJsonContractResolver.ConfigureDefaultSettings();
            Models.MesJsonContractResolver.SetWhitelist(new[] { "WOCHECKIN_MULTI", "WOCHECKOUT_MULTI" });

            var req = new WOCHECKINRequest
            {
                WONO = "WO20260701",
                EqpNo = "EQ01",
                UserID = "U001"
            };

            // Act
            string json = Models.MesJsonContractResolver.Serialize(req, Newtonsoft.Json.Formatting.None);

            // Assert: 非白名單 API 應 100% 維持原有序列化 (包含原本的 null 欄位)
            json.Should().Contain("\"WONO\":\"WO20260701\"");
            json.Should().Contain("\"ComponentBatchIn\":null");
            json.Should().Contain("\"EqpListDt\":null");
        }

        [Fact]
        public void Whitelist_CaseAndUnderscore_Tolerance_ShouldMatchCorrectly()
        {
            // Arrange: 測試大小寫與底線容錯
            Models.MesJsonContractResolver.SetWhitelist(new[] { "wocheckin_multi", "WOCHECKOUTMULTI" });

            // Act & Assert
            Models.MesJsonContractResolver.IsFilterEnabled(MESTransactionName.WOCHECKIN_MULTI).Should().BeTrue();
            Models.MesJsonContractResolver.IsFilterEnabled(MESTransactionName.WOCHECKOUT_MULTI).Should().BeTrue();
            Models.MesJsonContractResolver.IsFilterEnabled(MESTransactionName.WOCHECKIN).Should().BeFalse();
            Models.MesJsonContractResolver.IsFilterEnabled(MESTransactionName.EQPSTATUS).Should().BeFalse();
        }

        [Fact]
        public void Whitelist_DynamicConfiguration_ShouldImmediatelyTakeEffect()
        {
            // Arrange 1: 僅加入 EQPSTATUS
            Models.MesJsonContractResolver.SetWhitelist(new[] { "EQPSTATUS" });
            var eqpReq = new EQPSTATUSRequest
            {
                EqpNo = "ET0023",
                StatusCode = "3",
                ErrorCode = null
            };

            // Act 1
            string json1 = Models.MesJsonContractResolver.Serialize(eqpReq, Newtonsoft.Json.Formatting.None);

            // Assert 1: EQPSTATUS 在白名單內，ErrorCode (null) 被刪除
            json1.Should().NotContain("ErrorCode");

            // Arrange 2: 白名單重設為空
            Models.MesJsonContractResolver.SetWhitelist(new string[0]);

            // Act 2
            string json2 = Models.MesJsonContractResolver.Serialize(eqpReq, Newtonsoft.Json.Formatting.None);

            // Assert 2: 不在白名單內，ErrorCode 保留 null
            json2.Should().Contain("\"ErrorCode\":null");

            // 還原預設白名單
            Models.MesJsonContractResolver.SetWhitelist(new[] { "WOCHECKIN_MULTI", "WOCHECKOUT_MULTI" });
        }
    }
}
