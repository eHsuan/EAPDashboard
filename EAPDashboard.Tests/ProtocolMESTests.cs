using FluentAssertions;
using Protocol.MES.Enums;
using Protocol.MES.Messages;
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
    }
}
