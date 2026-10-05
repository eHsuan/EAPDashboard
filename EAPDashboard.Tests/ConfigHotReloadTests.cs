using System.Collections.Generic;
using System.Linq;
using EAPDashboard.Models;
using EAPDashboard.Services;
using FluentAssertions;
using Xunit;

namespace EAPDashboard.Tests
{
    public class ConfigHotReloadTests
    {
        [Fact]
        public void Compare_WhenBothConfigsIdentical_ShouldReturnNoChanges()
        {
            var oldCfg = new AppConfig
            {
                EapId = "EAP01",
                HeartbeatIntervalSeconds = 30,
                Equipments = new List<EquipmentConfig>
                {
                    new EquipmentConfig { Id = "TP0013", Name = "TP0013", Protocol = "MQTT", Topic = "factory/TP0013", IsActive = true }
                }
            };

            var newCfg = new AppConfig
            {
                EapId = "EAP01",
                HeartbeatIntervalSeconds = 30,
                Equipments = new List<EquipmentConfig>
                {
                    new EquipmentConfig { Id = "TP0013", Name = "TP0013", Protocol = "MQTT", Topic = "factory/TP0013", IsActive = true }
                }
            };

            var result = ConfigDiffService.Compare(oldCfg, newCfg);

            result.HasAnyChange.Should().BeFalse();
            result.HasGlobalChanges.Should().BeFalse();
            result.HasEquipmentChanges.Should().BeFalse();
            result.EquipmentDiffs.First().DiffType.Should().Be(EquipmentDiffType.Unchanged);
        }

        [Fact]
        public void Compare_WhenGlobalSettingChanged_ShouldDetectGlobalChanges()
        {
            var oldCfg = new AppConfig
            {
                EapId = "EAP01",
                HeartbeatIntervalSeconds = 30,
                MesSettings = new MesSettings { ActiveEnvironment = "Test" }
            };

            var newCfg = new AppConfig
            {
                EapId = "EAP01",
                HeartbeatIntervalSeconds = 60, // 變更心跳
                MesSettings = new MesSettings { ActiveEnvironment = "Production" } // 變更環境
            };

            var result = ConfigDiffService.Compare(oldCfg, newCfg);

            result.HasGlobalChanges.Should().BeTrue();
            result.GlobalChanges.Should().Contain(c => c.PropertyName == "心跳間隔 (秒)" && c.OldValue == "30" && c.NewValue == "60");
            result.GlobalChanges.Should().Contain(c => c.PropertyName == "啟用環境" && c.OldValue == "Test" && c.NewValue == "Production");
        }

        [Fact]
        public void Compare_WhenEquipmentUnchanged_ShouldMarkAsUnchangedAndNotBreakConnection()
        {
            var oldCfg = new AppConfig
            {
                Equipments = new List<EquipmentConfig>
                {
                    new EquipmentConfig
                    {
                        Id = "TP0013",
                        Name = "舊名稱",
                        Protocol = "MQTT",
                        Topic = "factory/LOCAL/machine/TP0013",
                        StatusType = "Cyntec",
                        IsActive = true
                    }
                }
            };

            var newCfg = new AppConfig
            {
                Equipments = new List<EquipmentConfig>
                {
                    new EquipmentConfig
                    {
                        Id = "TP0013",
                        Name = "新顯示名稱", // 僅改名稱，核心通訊未變
                        Protocol = "MQTT",
                        Topic = "factory/LOCAL/machine/TP0013",
                        StatusType = "Cyntec",
                        IsActive = true
                    }
                }
            };

            var result = ConfigDiffService.Compare(oldCfg, newCfg);

            result.HasGlobalChanges.Should().BeFalse();
            var eqDiff = result.EquipmentDiffs.First(e => e.EquipmentId == "TP0013");
            eqDiff.DiffType.Should().Be(EquipmentDiffType.Unchanged);
            result.HasEquipmentChanges.Should().BeFalse(); // 僅非核心屬性更新，不觸發設備重連
        }

        [Fact]
        public void Compare_WhenEquipmentCoreChanged_ShouldMarkAsModified()
        {
            var oldCfg = new AppConfig
            {
                Equipments = new List<EquipmentConfig>
                {
                    new EquipmentConfig
                    {
                        Id = "EQ02",
                        Protocol = "SECS",
                        Ip = "127.0.0.1",
                        Port = 5000,
                        DeviceId = 1,
                        IsActive = true
                    }
                }
            };

            var newCfg = new AppConfig
            {
                Equipments = new List<EquipmentConfig>
                {
                    new EquipmentConfig
                    {
                        Id = "EQ02",
                        Protocol = "SECS",
                        Ip = "192.168.1.100", // 修改 IP
                        Port = 5001,          // 修改 Port
                        DeviceId = 1,
                        IsActive = true
                    }
                }
            };

            var result = ConfigDiffService.Compare(oldCfg, newCfg);

            result.HasGlobalChanges.Should().BeFalse();
            var eqDiff = result.EquipmentDiffs.First(e => e.EquipmentId == "EQ02");
            eqDiff.DiffType.Should().Be(EquipmentDiffType.Modified);
            eqDiff.ChangeDetails.Should().Contain(d => d.Contains("IP 位址"));
            eqDiff.ChangeDetails.Should().Contain(d => d.Contains("Port 埠號"));
        }

        [Fact]
        public void Compare_WhenEquipmentAddedOrRemoved_ShouldIdentifyCorrectly()
        {
            var oldCfg = new AppConfig
            {
                Equipments = new List<EquipmentConfig>
                {
                    new EquipmentConfig { Id = "OLD01", IsActive = true, Protocol = "MQTT" }
                }
            };

            var newCfg = new AppConfig
            {
                Equipments = new List<EquipmentConfig>
                {
                    new EquipmentConfig { Id = "NEW01", IsActive = true, Protocol = "SECS", Ip = "10.0.0.1", Port = 5000 }
                }
            };

            var result = ConfigDiffService.Compare(oldCfg, newCfg);

            result.HasGlobalChanges.Should().BeFalse();
            result.EquipmentDiffs.Should().Contain(e => e.EquipmentId == "OLD01" && e.DiffType == EquipmentDiffType.Removed);
            result.EquipmentDiffs.Should().Contain(e => e.EquipmentId == "NEW01" && e.DiffType == EquipmentDiffType.Added);
        }

        [Fact]
        public void Compare_WhenEquipmentDeactivated_ShouldMarkAsRemoved()
        {
            var oldCfg = new AppConfig
            {
                Equipments = new List<EquipmentConfig>
                {
                    new EquipmentConfig { Id = "TP0013", IsActive = true, Protocol = "MQTT", Topic = "test" }
                }
            };

            var newCfg = new AppConfig
            {
                Equipments = new List<EquipmentConfig>
                {
                    new EquipmentConfig { Id = "TP0013", IsActive = false, Protocol = "MQTT", Topic = "test" }
                }
            };

            var result = ConfigDiffService.Compare(oldCfg, newCfg);

            var eqDiff = result.EquipmentDiffs.First(e => e.EquipmentId == "TP0013");
            eqDiff.DiffType.Should().Be(EquipmentDiffType.Removed);
            eqDiff.ChangeDetails.Should().Contain(d => d.Contains("由啟用轉為停用"));
        }

        [Fact]
        public void Compare_WhenRootLogAreYouThereIsNullInBoth_ShouldNotReportChanges()
        {
            var oldCfg = new AppConfig
            {
                LogSettings = new LogSettings { LogAreYouThere = false },
                LogAreYouThere = null
            };

            var newCfg = new AppConfig
            {
                LogSettings = new LogSettings { LogAreYouThere = false },
                LogAreYouThere = null
            };

            var result = ConfigDiffService.Compare(oldCfg, newCfg);
            result.HasGlobalChanges.Should().BeFalse();
            result.HasAnyChange.Should().BeFalse();
        }
    }
}
