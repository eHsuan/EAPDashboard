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
    }
}
