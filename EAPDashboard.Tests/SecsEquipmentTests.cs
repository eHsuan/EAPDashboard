using System.Collections.Generic;
using EAPDashboard.Models;
using FluentAssertions;
using Protocol.Core.Messages;
using Xunit;

namespace EAPDashboard.Tests
{
    public class SecsEquipmentTests
    {
        [Fact]
        public void SecsEquipment_Initialization_ShouldSetProtocolToSECS()
        {
            // Arrange & Act
            var eq = new SecsEquipment();

            // Assert
            eq.Protocol.Should().Be("SECS");
            eq.Connection.Should().Be(ConnectionStatus.Disconnected);
        }

        [Fact]
        public void ParameterResultItem_ValueAssignment_ShouldSupportStringValue()
        {
            // Arrange & Act
            var paramItem = new ParameterResultItem
            {
                Parameter = "PeakTemp",
                Value = "255.4"
            };

            // Assert
            paramItem.Parameter.Should().Be("PeakTemp");
            paramItem.Value.Should().Be("255.4");
        }
    }
}
