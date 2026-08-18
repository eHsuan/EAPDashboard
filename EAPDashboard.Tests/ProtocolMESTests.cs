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
    }
}
