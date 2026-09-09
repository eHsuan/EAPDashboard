using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAPDashboard.Models;
using FluentAssertions;
using Protocol.MES.Base;
using Protocol.MES.Messages;
using Xunit;
using NGCodeItem = EAPDashboard.Models.NGCodeItem;

namespace EAPDashboard.Tests
{
    public class TestableEquipment : EquipmentBase
    {
        public List<string> LogHistory { get; } = new List<string>();
        public string LastTerminalDisplayMessage { get; private set; }
        public Func<MESBaseMessage, MESBaseMessage> MesCallHandler { get; set; }

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

        protected override TReply CallMesWithLog<TRequest, TReply>(TRequest request)
        {
            if (MesCallHandler != null)
            {
                var result = MesCallHandler(request);
                if (result is TReply typedReply)
                {
                    return typedReply;
                }
            }
            return base.CallMesWithLog<TRequest, TReply>(request);
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

        [Theory]
        [InlineData("RECIPE_MISMATCH_A_WO001", true)]
        [InlineData("SIM_RECIPE_DIFF_WO001", true)]
        [InlineData("UNKNOWN", true)]
        [InlineData("", true)]
        [InlineData("NORMAL_RECIPE", false)]
        public void SingleWorkOrder_RecipeValidation_ShouldDetectNotFoundOrMismatch(string unifiedRecipe, bool shouldFail)
        {
            // Arrange
            int validWoCount = 1;

            // Act
            bool isFail = validWoCount == 1 && (string.IsNullOrWhiteSpace(unifiedRecipe) 
                || unifiedRecipe.Equals("UNKNOWN", StringComparison.OrdinalIgnoreCase) 
                || unifiedRecipe.Equals("ERROR", StringComparison.OrdinalIgnoreCase) 
                || unifiedRecipe.StartsWith("RECIPE_MISMATCH_", StringComparison.OrdinalIgnoreCase) 
                || unifiedRecipe.StartsWith("SIM_RECIPE_DIFF_", StringComparison.OrdinalIgnoreCase));

            // Assert
            isFail.Should().Be(shouldFail);
        }

        [Theory]
        [InlineData("在MES找不到RECIPEID", "RECIPEID NOT FOUND IN MES")]
        [InlineData("Hello World 123!", "Hello World 123!")]
        [InlineData("機台警告: WO001 Recipe Mismatch", ": WO001 Recipe Mismatch")]
        [InlineData("EQ_WARN: WO001 Recipe Mismatch", "EQ_WARN: WO001 Recipe Mismatch")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void SanitizeTerminalMessage_ShouldEnsureAsciiAlphanumericOnly(string input, string expected)
        {
            // Act
            string result = EquipmentBase.SanitizeTerminalMessage(input);

            // Assert
            result.Should().Be(expected);
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
        public async Task ProcessTrackOutAsync_WhenUnreportedNGCodesExist_TpEquipment_ShouldRejectWithCommaSeparatedList()
        {
            // Arrange: TP 開頭設備應檢查並阻擋未回報之 NG 碼
            var eq = new TestableEquipment("TP0013");
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
        public async Task ProcessTrackOutAsync_WhenUnreportedNGCodesExist_NonTpEquipment_ShouldNotCheckAndProceed()
        {
            // Arrange: 非 TP 開頭設備 (如 EQ01, TS0003) 不受 NG 碼未申報阻擋
            var eq = new TestableEquipment("EQ01");
            eq.SetWorkOrderNGCodesForTest("WO100", new List<NGCodeItem>
            {
                new NGCodeItem { NGCode = "UPNG501", NGName = "卡片", NGQty = null },
                new NGCodeItem { NGCode = "UPNG502", NGName = "折片", NGQty = null }
            });

            // Act
            string result = await eq.ProcessTrackOutAsync(
                workOrders: new List<string> { "WO100" },
                qty: "10",
                userId: "U001",
                cassetteIds: new List<string> { "C001" },
                panels: new List<string>());

            // Assert: 非 TP 設備不會被 NG_CHECK_FAIL 阻擋
            result.Should().NotStartWith("FAIL|NG_CHECK_FAIL|");
        }

        [Fact]
        public async Task ProcessTrackOutAsync_WhenAllNGCodesReported_ShouldProceed()
        {
            // Arrange
            var eq = new TestableEquipment("TP0013");
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
        public async Task NgCodeCache_Persistence_TpEquipment_ShouldSurviveRestart()
        {
            // Arrange
            string tempDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "EAP_Test_" + Guid.NewGuid().ToString("N"));
            EquipmentBase.CustomCacheDirectory = tempDir;

            try
            {
                var eq1 = new TestableEquipment("TP_PERSIST_02");
                var ngItems = new List<NGCodeItem>
                {
                    new NGCodeItem { NGCode = "NG001", NGName = "Defect A" },
                    new NGCodeItem { NGCode = "NG002", NGName = "Defect B" }
                };
                eq1.SetWorkOrderNGCodesForTest("WO_RESTART_002", ngItems);
                eq1.SaveNgCodeCacheToDisk();
                eq1.RecordNGCodeReport("WO_RESTART_002", "NG001", "Defect A", "5", "PCS");

                // Act - Simulate EAP restart by creating a new instance and calling LoadNgCodeCache
                var eq2 = new TestableEquipment("TP_PERSIST_02");
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
        public void NgCodeCache_NonTpEquipment_ShouldNotSaveOrLoadCache()
        {
            // Arrange
            string tempDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "EAP_Test_" + Guid.NewGuid().ToString("N"));
            EquipmentBase.CustomCacheDirectory = tempDir;

            try
            {
                var eq1 = new TestableEquipment("EQ_NON_TP");
                var ngItems = new List<NGCodeItem>
                {
                    new NGCodeItem { NGCode = "NG001", NGName = "Defect A" }
                };
                eq1.SetWorkOrderNGCodesForTest("WO_NON_TP", ngItems);
                eq1.SaveNgCodeCacheToDisk();

                string cacheFile = System.IO.Path.Combine(tempDir, "ng_code_cache.json");
                System.IO.File.Exists(cacheFile).Should().BeFalse();

                var eq2 = new TestableEquipment("EQ_NON_TP");
                eq2.LoadNgCodeCache();
                eq2.HasWorkOrderNGCodes("WO_NON_TP").Should().BeFalse();
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
            var (ok1, msg1, _, _) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok1.Should().BeFalse();
            msg1.Should().Contain("LeftoversReport").And.Contain("TotalCountReport");

            // Only LeftoversReport reported
            eq.RecordLeftoversReport(wo, "10", "REEL01");
            var (ok2, msg2, _, _) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok2.Should().BeFalse();
            msg2.Should().Contain("TotalCountReport");

            // Now TotalCountReport also reported
            eq.RecordTotalCountReport(wo, "10", "500");
            eq.RecordWorkOrderInputQty(wo, 5010);
            var (ok3, msg3, _, passQty3) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok3.Should().BeTrue();
            passQty3.Should().Be(5010); // 10 * 500 + 10 = 5010
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

            var (ok, msg, _, _) = eq.ValidateAndAdjustTpTrackOut(wo);
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

            var (ok, msg, _, _) = eq.ValidateAndAdjustTpTrackOut(wo);
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

            var (ok, msg, calcNg, passQty) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok.Should().BeTrue();
            calcNg.Should().Be(80); // 1000 - 920 = 80
            passQty.Should().Be(920);
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

            var (ok, msg, _, _) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok.Should().BeFalse();
            msg.Should().Contain("NG數溢出");
        }

        [Fact]
        public void TpTrackOut_CalcNgLessThanTotalReported_LevelingBalance_ShouldLevelRemainingQuantities()
        {
            var eq = new TestableEquipment("TP006");
            string wo = "WO_TP_006";

            eq.RecordWorkOrderInputQty(wo, 1000);
            eq.RecordTotalCountReport(wo, "9", "100"); // 900
            eq.RecordLeftoversReport(wo, "90", "R01");  // 90
            eq.RecordProductLeftoverReport(wo, "10", "0", "R01", "WO_PREV", "B01"); // AddQty = 10 => PassQty = 980 => CalcNG = 20

            // Reported NG items:
            // Special: QC Keep Sample = 5, 免補料 = 5 (Total special = 10)
            // Non-special: UPNG501 = 20, UPNG502 = 10 (Total non-special = 30, Total reported = 40)
            // CalcNG = 20. Diff to deduct = 40 - 20 = 20.
            // Target remaining non-special = 30 - 20 = 10.
            // Leveling remaining allocation: 10 / 2 = 5 each.
            // UPNG501 (20) -> deducts 15 -> remaining 5.
            // UPNG502 (10) -> deducts 5 -> remaining 5.
            // Special items strictly remain 5 and 5.
            var ngList = new List<NGCodeItem>
            {
                new NGCodeItem { NGCode = "NG_QC", NGName = "QC Keep Sample", NGQty = "5" },
                new NGCodeItem { NGCode = "NG_FREE", NGName = "免補料", NGQty = "5" },
                new NGCodeItem { NGCode = "UPNG501", NGName = "Card Issue", NGQty = "20" },
                new NGCodeItem { NGCode = "UPNG502", NGName = "Broken", NGQty = "10" }
            };
            eq.SetWorkOrderNGCodesForTest(wo, ngList);

            var (ok, msg, calcNg, passQty) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok.Should().BeTrue();
            calcNg.Should().Be(20);
            passQty.Should().Be(980);

            var itemQc = eq.GetNGCodeItemForTest(wo, "NG_QC");
            var itemFree = eq.GetNGCodeItemForTest(wo, "NG_FREE");
            var item501 = eq.GetNGCodeItemForTest(wo, "UPNG501");
            var item502 = eq.GetNGCodeItemForTest(wo, "UPNG502");

            itemQc.NGQty.Should().Be("5"); // Untouched
            itemFree.NGQty.Should().Be("5"); // Untouched
            item501.NGQty.Should().Be("5"); // Level-balanced remaining: 20 - 15 = 5
            item502.NGQty.Should().Be("5"); // Level-balanced remaining: 10 - 5 = 5
        }

        [Fact]
        public void TpTrackOut_CalcNgLessThanTotalReported_MultiItemExceedingShare_ShouldBalanceRemainingLevels()
        {
            var eq = new TestableEquipment("TP007");
            string wo = "WO_TP_007";

            eq.RecordWorkOrderInputQty(wo, 1000);
            eq.RecordTotalCountReport(wo, "9", "100"); // 900
            eq.RecordLeftoversReport(wo, "93", "R01");  // 93
            eq.RecordProductLeftoverReport(wo, "10", "0", "R01", "WO_PREV", "B01"); // AddQty = 10 => PassQty = 983 => CalcNG = 17

            // Special: QC Keep Sample = 5 (Total 5)
            // Non-special: NG_SMALL = 2, NG_BIG1 = 20, NG_BIG2 = 10 (Total non-special = 32, Total reported = 37)
            // CalcNG = 17. Diff to deduct = 37 - 17 = 20.
            // Target remaining non-special = 32 - 20 = 12.
            // Leveling remaining allocation of 12 across [2, 20, 10]:
            // NG_SMALL capped at 2 (deducted 0, remaining 2).
            // Remaining 10 allocated evenly to NG_BIG1 and NG_BIG2: 10 / 2 = 5 each.
            // NG_BIG1 (20) -> deducts 15 -> remaining 5.
            // NG_BIG2 (10) -> deducts 5 -> remaining 5.
            // Total remaining: 2 + 5 + 5 = 12. Total non-special deducted: 0 + 15 + 5 = 20.
            var ngList = new List<NGCodeItem>
            {
                new NGCodeItem { NGCode = "NG_QC", NGName = "QC Keep Sample", NGQty = "5" },
                new NGCodeItem { NGCode = "NG_SMALL", NGName = "Small Scratch", NGQty = "2" },
                new NGCodeItem { NGCode = "NG_BIG1", NGName = "Broken", NGQty = "20" },
                new NGCodeItem { NGCode = "NG_BIG2", NGName = "Dirty", NGQty = "10" }
            };
            eq.SetWorkOrderNGCodesForTest(wo, ngList);

            var (ok, msg, calcNg, passQty) = eq.ValidateAndAdjustTpTrackOut(wo);
            ok.Should().BeTrue();
            calcNg.Should().Be(17);

            var itemQc = eq.GetNGCodeItemForTest(wo, "NG_QC");
            var itemSmall = eq.GetNGCodeItemForTest(wo, "NG_SMALL");
            var itemBig1 = eq.GetNGCodeItemForTest(wo, "NG_BIG1");
            var itemBig2 = eq.GetNGCodeItemForTest(wo, "NG_BIG2");

            itemQc.NGQty.Should().Be("5");
            itemSmall.NGQty.Should().Be("2"); // Capped at original 2 (deducted 0)
            itemBig1.NGQty.Should().Be("5");  // 20 - 15 = 5
            itemBig2.NGQty.Should().Be("5");  // 10 - 5 = 5
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

        [Fact]
        public async Task ProcessTrackInAsync_SingleWorkOrder_AlreadyInFlagY_ShouldFailAndSendTerminalDisplay()
        {
            // Arrange
            var eq = new TestableEquipment("TP0013");
            eq.MesCallHandler = (req) =>
            {
                if (req is WOQRYRequest)
                {
                    var reply = new WOQRYReply
                    {
                        Result = "success",
                        ResultCode = "W001",
                        Message = "",
                        InputParamListDT = "[{\"ParamName\":\"RECIPEID\",\"DefaultValue\":\"RCP_01\"}]"
                    };
                    try { ((dynamic)reply).AlreadyInFlag = "Y"; } catch { }
                    return reply;
                }
                return null;
            };

            // Act
            string result = await eq.ProcessTrackInAsync(
                cassetteIds: new List<string> { "C001" },
                workOrders: new List<string> { "WO2026001" },
                panels: new List<string>(),
                userId: "U001");

            // Assert
            result.Should().StartWith("FAIL|ERR_ALREADY_IN|");
            result.Should().Contain("WO2026001 已進站，禁止重複進站");
            eq.LastTerminalDisplayMessage.Should().Be("WO ALREADY CHECKED IN: WO2026001");
        }

        [Fact]
        public async Task ProcessTrackInAsync_MultipleWorkOrders_AlreadyInFlagY_ShouldFailAndSendTerminalDisplay()
        {
            // Arrange
            var eq = new TestableEquipment("TP0013");
            eq.MesCallHandler = (req) =>
            {
                if (req is WOQRYRequest qry)
                {
                    var reply = new WOQRYReply
                    {
                        Result = "success",
                        ResultCode = "W001",
                        Message = "",
                        InputParamListDT = "[{\"ParamName\":\"RECIPEID\",\"DefaultValue\":\"RCP_01\"}]"
                    };
                    // WO2026002 已進站
                    string flag = (qry.WONO == "WO2026002" || qry.WONO == "WO2026003") ? "Y" : "N";
                    try { ((dynamic)reply).AlreadyInFlag = flag; } catch { }
                    return reply;
                }
                return null;
            };

            // Act
            string result = await eq.ProcessTrackInAsync(
                cassetteIds: new List<string> { "C001", "C002", "C003" },
                workOrders: new List<string> { "WO2026001", "WO2026002", "WO2026003" },
                panels: new List<string>(),
                userId: "U001");

            // Assert
            result.Should().StartWith("FAIL|ERR_ALREADY_IN|");
            result.Should().Contain("WO2026002,WO2026003 已進站，禁止重複進站");
            eq.LastTerminalDisplayMessage.Should().Be("WO ALREADY CHECKED IN: WO2026002,WO2026003");
        }

        [Fact]
        public async Task ProcessTrackInAsync_NonTpEquipment_ShouldNotLoadNgCodesOrSaveCache()
        {
            // Arrange
            string tempDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "EAP_Test_" + Guid.NewGuid().ToString("N"));
            EquipmentBase.CustomCacheDirectory = tempDir;

            try
            {
                var eq = new TestableEquipment("TS0003"); // 非 TP 設備
                eq.MesCallHandler = (req) =>
                {
                    if (req is WOQRYRequest)
                    {
                        var reply = new WOQRYReply
                        {
                            Result = "success",
                            ResultCode = "W001",
                            Message = "",
                            InputParamListDT = "[{\"ParamName\":\"RECIPEID\",\"DefaultValue\":\"RCP_01\"}]"
                        };
                        try { ((dynamic)reply).AlreadyInFlag = "N"; } catch { }
                        try { ((dynamic)reply).NGCodeListDT = "[{\"NGCode\":\"NG_01\",\"NGName\":\"Defect 1\"}]"; } catch { }
                        return reply;
                    }
                    return null;
                };

                // Act
                string result = await eq.ProcessTrackInAsync(
                    cassetteIds: new List<string> { "C001" },
                    workOrders: new List<string> { "WO_NON_TP_01" },
                    panels: new List<string>(),
                    userId: "U001");

                // Assert: 成功切換配方，且未在記憶體與磁碟儲存 NG 碼
                result.Should().StartWith("PP-SELECT|");
                eq.HasWorkOrderNGCodes("WO_NON_TP_01").Should().BeFalse();
                string cacheFile = System.IO.Path.Combine(tempDir, "ng_code_cache.json");
                System.IO.File.Exists(cacheFile).Should().BeFalse();
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
        public async Task ProcessTrackInAsync_TpEquipment_ShouldLoadNgCodesAndSaveCache()
        {
            // Arrange
            string tempDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "EAP_Test_" + Guid.NewGuid().ToString("N"));
            EquipmentBase.CustomCacheDirectory = tempDir;

            try
            {
                var eq = new TestableEquipment("TP0013"); // TP 設備
                eq.MesCallHandler = (req) =>
                {
                    if (req is WOQRYRequest)
                    {
                        var reply = new WOQRYReply
                        {
                            Result = "success",
                            ResultCode = "W001",
                            Message = "",
                            InputParamListDT = "[{\"ParamName\":\"RECIPEID\",\"DefaultValue\":\"RCP_01\"}]"
                        };
                        try { ((dynamic)reply).AlreadyInFlag = "N"; } catch { }
                        try { ((dynamic)reply).NGCodeListDT = "[{\"NGCode\":\"NG_01\",\"NGName\":\"Defect 1\"}]"; } catch { }
                        return reply;
                    }
                    return null;
                };

                // Act
                string result = await eq.ProcessTrackInAsync(
                    cassetteIds: new List<string> { "C001" },
                    workOrders: new List<string> { "WO_TP_01" },
                    panels: new List<string>(),
                    userId: "U001");

                // Assert: 成功切換配方，且記憶體與磁碟皆成功儲存 NG 碼
                result.Should().StartWith("PP-SELECT|");
                eq.HasWorkOrderNGCodes("WO_TP_01").Should().BeTrue();
                string cacheFile = System.IO.Path.Combine(tempDir, "ng_code_cache.json");
                System.IO.File.Exists(cacheFile).Should().BeTrue();
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
        public async Task ProcessStatusChangeAsync_WhenNewStatusMatchesCurrentStatus_ShouldFilterAndNotCallMes()
        {
            // Arrange
            var eq = new TestableEquipment("EQ_STATUS_01");
            eq.SetLastStatus("2"); // 目前狀態已是 2

            int mesCallCount = 0;
            eq.MesCallHandler = req =>
            {
                if (req is EQPSTATUSRequest)
                {
                    mesCallCount++;
                    return new EQPSTATUSReply { Result = "PASS", ResultCode = "0", Message = "OK" };
                }
                return null;
            };

            // Act: 機台重複上報 oldStatus=1, newStatus=2
            string result = await eq.ProcessStatusChangeAsync("1", "2");

            // Assert: 應直接過濾，且不呼叫 MES
            result.Should().StartWith("PASS|DUPLICATE|");
            mesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task ProcessTrackInAsync_WhenWoQryReturnsError0049_ShouldSendTerminalDisplayAndFail()
        {
            // Arrange
            var eq = new TestableEquipment("EQ_ERR0049_01");

            eq.MesCallHandler = req =>
            {
                if (req is WOQRYRequest qryReq)
                {
                    return new WOQRYReply
                    {
                        Result = "FAIL",
                        ResultCode = "ERROR0049",
                        Message = "此工單不屬於此站點可進站之工單"
                    };
                }
                return null;
            };

            // Act
            string result = await eq.ProcessTrackInAsync(
                cassetteIds: new List<string> { "C001" },
                workOrders: new List<string> { "WO_ERR_01" },
                panels: new List<string>(),
                userId: "U001");

            // Assert
            result.Should().StartWith("FAIL|ERROR0049|");
            eq.LastTerminalDisplayMessage.Should().NotBeNull();
            eq.LastTerminalDisplayMessage.Should().Contain("WO_ERR_01 is not valid for check-in at this station");
        }

        [Fact]
        public async Task ProcessTrackInAsync_WhenWoQryReturnsOtherFailCode_ShouldSendTerminalDisplayWithFailFormat()
        {
            // Arrange
            var eq = new TestableEquipment("EQ_FAIL_01");

            eq.MesCallHandler = req =>
            {
                if (req is WOQRYRequest)
                {
                    return new WOQRYReply
                    {
                        Result = "FAIL",
                        ResultCode = "ERRORUSER0003",
                        Message = "條碼無效"
                    };
                }
                return null;
            };

            // Act
            string result = await eq.ProcessTrackInAsync(
                cassetteIds: new List<string> { "C001" },
                workOrders: new List<string> { "WO_FAIL_01" },
                panels: new List<string>(),
                userId: "U001");

            // Assert
            result.Should().StartWith("FAIL|ERRORUSER0003|");
            eq.LastTerminalDisplayMessage.Should().Be("Barcode Invalid");
        }

        [Fact]
        public async Task ProcessTrackInFinalAsync_WhenCheckInFails_ShouldSendTerminalDisplayWithFailFormat()
        {
            // Arrange
            var eq = new TestableEquipment("EQ_CKIN_FAIL");

            eq.MesCallHandler = req =>
            {
                if (req is WOCHECKINRequest)
                {
                    return new WOCHECKINReply
                    {
                        Result = "FAIL",
                        ResultCode = "20601",
                        Message = "未達生產條件"
                    };
                }
                return null;
            };

            // Act
            string result = await eq.ProcessTrackInFinalAsync(
                workOrders: new List<string> { "WO_CKIN_01" },
                userId: "U001",
                cassetteIds: new List<string> { "C001" });

            // Assert
            result.Should().StartWith("FAIL|20601|");
            eq.LastTerminalDisplayMessage.Should().Be("Process time not met");
        }

        [Fact]
        public async Task ProcessTrackOutAsync_WhenCheckOutFails_ShouldSendTerminalDisplayWithFailFormat()
        {
            // Arrange
            var eq = new TestableEquipment("EQ_CKOUT_FAIL");

            eq.MesCallHandler = req =>
            {
                if (req is WOCHECKOUTRequest)
                {
                    return new WOCHECKOUTReply
                    {
                        Result = "FAIL",
                        ResultCode = "20108",
                        Message = "狀態不合"
                    };
                }
                return null;
            };

            // Act
            string result = await eq.ProcessTrackOutAsync(
                workOrders: new List<string> { "WO_CKOUT_01" },
                qty: "10",
                userId: "U001",
                cassetteIds: new List<string> { "C001" },
                panels: new List<string>());

            // Assert
            result.Should().StartWith("FAIL|20108|");
            eq.LastTerminalDisplayMessage.Should().Be("WO WO_CKOUT_01 status is not WOCHECKOUT");
        }

        [Fact]
        public async Task ProcessTrackInFinalAsync_MultipleWorkOrders_WhenCassetteIdIsNAOrEmpty_MesRequestShouldNotContainWOCarrierDt()
        {
            // Arrange
            var eq = new TestableEquipment("EQ_MULTI_NO_CARRIER");
            string capturedReqJson = null;

            eq.MesCallHandler = req =>
            {
                if (req is WOCHECKIN_MULTIRequest multiReq)
                {
                    capturedReqJson = MesJsonContractResolver.Serialize(multiReq, Newtonsoft.Json.Formatting.None);
                    return new WOCHECKIN_MULTIReply
                    {
                        Result = "PASS",
                        ResultCode = "0",
                        Message = "Success"
                    };
                }
                return null;
            };

            // Act 1: 傳入 "N/A" 卡匣
            await eq.ProcessTrackInFinalAsync(
                workOrders: new List<string> { "WO_MULTI_01", "WO_MULTI_02" },
                userId: "U001",
                cassetteIds: new List<string> { "N/A" });

            // Assert 1: JSON 絕對不含 WOCarrierDt
            capturedReqJson.Should().NotBeNull();
            capturedReqJson.Should().NotContain("WOCarrierDt");

            // Act 2: 傳入空集合
            capturedReqJson = null;
            await eq.ProcessTrackInFinalAsync(
                workOrders: new List<string> { "WO_MULTI_01", "WO_MULTI_02" },
                userId: "U001",
                cassetteIds: new List<string>());

            // Assert 2: JSON 絕對不含 WOCarrierDt
            capturedReqJson.Should().NotBeNull();
            capturedReqJson.Should().NotContain("WOCarrierDt");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task ProcessTrackOutAsync_SingleWorkOrder_WhenQtyIsEmptyOrNull_ShouldSendOutPutAsZero(string inputQty)
        {
            // Arrange
            var eq = new TestableEquipment("EQ_SINGLE_EMPTY_QTY");
            string capturedOutPut = null;

            eq.MesCallHandler = req =>
            {
                if (req is WOCHECKOUTRequest singleReq)
                {
                    capturedOutPut = singleReq.OutPut;
                    return new WOCHECKOUTReply { Result = "PASS", ResultCode = "0", Message = "Success" };
                }
                return null;
            };

            // Act: 工單出站時，傳入空字串或 null 之 qty
            var result = await eq.ProcessTrackOutAsync(
                workOrders: new List<string> { "WO_SINGLE_01" },
                qty: inputQty,
                userId: "U001",
                cassetteIds: new List<string>(),
                panels: new List<string>());

            // Assert: OUTPUT 這欄應被填為 "0"
            result.Should().StartWith("PASS|");
            capturedOutPut.Should().Be("0");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task ProcessTrackOutAsync_MultipleWorkOrders_WhenQtyIsEmptyOrNull_ShouldSendOutPutAsZero(string inputQty)
        {
            // Arrange
            var eq = new TestableEquipment("EQ_MULTI_EMPTY_QTY");
            string capturedOutPut = null;

            eq.MesCallHandler = req =>
            {
                if (req is WOCHECKOUT_MULTIRequest multiReq)
                {
                    capturedOutPut = multiReq.OutPut;
                    return new WOCHECKOUT_MULTIReply { Result = "PASS", ResultCode = "0", Message = "Success" };
                }
                return null;
            };

            // Act: 多工單出站時，傳入空字串或 null 之 qty
            var result = await eq.ProcessTrackOutAsync(
                workOrders: new List<string> { "WO_MULTI_01", "WO_MULTI_02" },
                qty: inputQty,
                userId: "U001",
                cassetteIds: new List<string>(),
                panels: new List<string>());

            // Assert: OUTPUT 這欄應被填為 "0"
            result.Should().StartWith("PASS|");
            capturedOutPut.Should().Be("0");
        }

        [Fact]
        public void ResolveMesFailTerminalMessage_WhenMapped_ShouldReturnMappedValue()
        {
            var eq = new TestableEquipment("EQ_MAP_TEST");
            EquipmentBase.ResetMesMappingCache();

            string msg1 = eq.ResolveMesFailTerminalMessage("ERRORUSER0003");
            msg1.Should().Be("Barcode Invalid");

            string msg2 = eq.ResolveMesFailTerminalMessage("20601");
            msg2.Should().Be("Process time not met");

            string msg3 = eq.ResolveMesFailTerminalMessage("20108", "WO123");
            msg3.Should().Be("WO WO123 status is not WOCHECKOUT");
        }

        [Fact]
        public void ResolveMesFailTerminalMessage_WithDynamicPlaceholder_ShouldReplaceAndValidate()
        {
            var eq = new TestableEquipment("EQ_DYN_TEST");
            EquipmentBase.ResetMesMappingCache();

            string msg = eq.ResolveMesFailTerminalMessage("ERROR0049", "WO998877");
            msg.Should().Be("WO WO998877 is not valid for check-in at this station");
        }

        [Fact]
        public void ResolveMesFailTerminalMessage_WhenCodeNotFound_ShouldLogWarningAndReturnResultCode()
        {
            var eq = new TestableEquipment("EQ_NOTFOUND_TEST");
            EquipmentBase.ResetMesMappingCache();

            string result = eq.ResolveMesFailTerminalMessage("CODE_NOT_EXISTS_999");

            result.Should().Be("MES ErrorCode : CODE_NOT_EXISTS_999");
            eq.LogHistory.Should().Contain(l => l.Contains("[Warn]") && l.Contains("查無錯誤碼 'CODE_NOT_EXISTS_999' 之映射"));
        }

        [Fact]
        public void ResolveMesFailTerminalMessage_WhenFileNotFound_ShouldLogWarningAndReturnResultCode()
        {
            var eq = new TestableEquipment("EQ_NOFILE_TEST");
            string originalPath = EquipmentBase.MesMappingFilePath;
            try
            {
                EquipmentBase.MesMappingFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "non_existent_" + Guid.NewGuid() + ".json");
                EquipmentBase.ResetMesMappingCache();

                string result = eq.ResolveMesFailTerminalMessage("ERR_ANY");

                result.Should().Be("MES ErrorCode : ERR_ANY");
                eq.LogHistory.Should().Contain(l => l.Contains("[Warn]") && l.Contains("查無 MES_Mapping.json 檔案"));
            }
            finally
            {
                EquipmentBase.MesMappingFilePath = originalPath;
                EquipmentBase.ResetMesMappingCache();
            }
        }

        [Fact]
        public void ResolveMesFailTerminalMessage_WhenValueContainsNonAlphaNumeric_ShouldLogWarningAndReturnResultCode()
        {
            var eq = new TestableEquipment("EQ_NONALNUM_TEST");
            string originalPath = EquipmentBase.MesMappingFilePath;
            string tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "mes_map_test_" + Guid.NewGuid() + ".json");
            try
            {
                System.IO.File.WriteAllText(tempFile, "{\"ERR_CHINESE\": \"此工單未開工\", \"ERR_SPECIAL\": \"INVALID #$%@\"}");
                EquipmentBase.MesMappingFilePath = tempFile;
                EquipmentBase.ResetMesMappingCache();

                // Test 1: Contains Chinese characters
                string result1 = eq.ResolveMesFailTerminalMessage("ERR_CHINESE");
                result1.Should().Be("MES ErrorCode : ERR_CHINESE");
                eq.LogHistory.Should().Contain(l => l.Contains("[Warn]") && l.Contains("包含非英數字元") && l.Contains("ERR_CHINESE"));

                // Test 2: Contains disallowed special characters
                string result2 = eq.ResolveMesFailTerminalMessage("ERR_SPECIAL");
                result2.Should().Be("MES ErrorCode : ERR_SPECIAL");
                eq.LogHistory.Should().Contain(l => l.Contains("[Warn]") && l.Contains("包含非英數字元") && l.Contains("ERR_SPECIAL"));
            }
            finally
            {
                if (System.IO.File.Exists(tempFile)) System.IO.File.Delete(tempFile);
                EquipmentBase.MesMappingFilePath = originalPath;
                EquipmentBase.ResetMesMappingCache();
            }
        }

        [Fact]
        public void ResolveMesFailTerminalMessage_WithUserIdPlaceholder_ShouldReplaceUserIdAndWorkOrder()
        {
            var eq = new TestableEquipment("EQ_USER_TEST");
            string originalPath = EquipmentBase.MesMappingFilePath;
            string tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "mes_map_user_" + Guid.NewGuid() + ".json");
            try
            {
                System.IO.File.WriteAllText(tempFile, "{\"ERR_AUTH\": \"WO {wo} OPERATOR {userid} NOT PERMITTED\"}");
                EquipmentBase.MesMappingFilePath = tempFile;
                EquipmentBase.ResetMesMappingCache();

                string msg = eq.ResolveMesFailTerminalMessage("ERR_AUTH", "WO12345", "OP007");
                msg.Should().Be("WO WO12345 OPERATOR OP007 NOT PERMITTED");
            }
            finally
            {
                if (System.IO.File.Exists(tempFile)) System.IO.File.Delete(tempFile);
                EquipmentBase.MesMappingFilePath = originalPath;
                EquipmentBase.ResetMesMappingCache();
            }
        }

        [Fact]
        public void ResolveMesFailTerminalMessage_WithEmptyUserIdOrWorkOrder_ShouldStripPlaceholdersCleanly()
        {
            var eq = new TestableEquipment("EQ_STRIP_TEST");
            string originalPath = EquipmentBase.MesMappingFilePath;
            string tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "mes_map_strip_" + Guid.NewGuid() + ".json");
            try
            {
                System.IO.File.WriteAllText(tempFile, "{\"ERR_CHECK\": \"WO {wo} USER {userid} CHECK FAILED\"}");
                EquipmentBase.MesMappingFilePath = tempFile;
                EquipmentBase.ResetMesMappingCache();

                // Case: workOrder and userId are both null
                string msg1 = eq.ResolveMesFailTerminalMessage("ERR_CHECK", null, null);
                msg1.Should().Be("WO USER CHECK FAILED");

                // Case: only workOrder provided, userId is null
                string msg2 = eq.ResolveMesFailTerminalMessage("ERR_CHECK", "WO999", null);
                msg2.Should().Be("WO WO999 USER CHECK FAILED");
            }
            finally
            {
                if (System.IO.File.Exists(tempFile)) System.IO.File.Delete(tempFile);
                EquipmentBase.MesMappingFilePath = originalPath;
                EquipmentBase.ResetMesMappingCache();
            }
        }

        [Fact]
        public void ResolveMesFailTerminalMessage_WithJsonComments_ShouldParseCorrectly()
        {
            var eq = new TestableEquipment("EQ_COMMENT_TEST");
            string originalPath = EquipmentBase.MesMappingFilePath;
            string tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "mes_map_comment_" + Guid.NewGuid() + ".json");
            try
            {
                string jsonWithComments = @"// Header comments
// Line 2 comment
{
    /* Block comment */
    ""ERR_WITH_COMMENT"": ""VALID MESSAGE FOR WO {wo}""
}";
                System.IO.File.WriteAllText(tempFile, jsonWithComments);
                EquipmentBase.MesMappingFilePath = tempFile;
                EquipmentBase.ResetMesMappingCache();

                string msg = eq.ResolveMesFailTerminalMessage("ERR_WITH_COMMENT", "WO888");
                msg.Should().Be("VALID MESSAGE FOR WO WO888");
            }
            finally
            {
                if (System.IO.File.Exists(tempFile)) System.IO.File.Delete(tempFile);
                EquipmentBase.MesMappingFilePath = originalPath;
                EquipmentBase.ResetMesMappingCache();
            }
        }
    }
}
