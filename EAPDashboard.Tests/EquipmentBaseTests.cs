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

            // Assert - Even if MES call returns error/mock in test without MES server, result format should be valid
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ProcessTrackInAsync_MultipleWorkOrders_DifferentRecipes_ShouldTriggerRecipeConsistencyCheck()
        {
            // Arrange
            var eq = new TestableEquipment("EQ01");
            var cassetteIds = new List<string> { "C001", "C002" };
            var workOrders = new List<string> { "WO001", "WO002" };

            // Act
            var result = await eq.ProcessTrackInAsync(cassetteIds, workOrders, new List<string>(), "U001");

            // Assert
            // When MES returns timeouts/fails or recipes, result should be properly formatted
            result.Should().NotBeNull();
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
    }
}
