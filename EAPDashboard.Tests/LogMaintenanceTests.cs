using System;
using System.IO;
using EAPDashboard.Models;
using FluentAssertions;
using Xunit;

namespace EAPDashboard.Tests
{
    public class LogMaintenanceTests : IDisposable
    {
        private readonly string _testTempDir;

        public LogMaintenanceTests()
        {
            _testTempDir = Path.Combine(Path.GetTempPath(), "EAPDashboard_LogTest_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_testTempDir);
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(_testTempDir))
                {
                    Directory.Delete(_testTempDir, true);
                }
            }
            catch { }
        }

        [Fact]
        public void RunMaintenance_OldLogDirectory_ShouldCompressToZipAndDeleteDirectory()
        {
            // Arrange: 建立 5 天前之歷史 LOG 資料夾 (超過預設 3 天)
            string oldDateStr = DateTime.Today.AddDays(-5).ToString("yyyyMMdd");
            string oldDir = Path.Combine(_testTempDir, oldDateStr, "EQ01");
            Directory.CreateDirectory(oldDir);
            string oldFile = Path.Combine(oldDir, "08.txt");
            File.WriteAllText(oldFile, "This is an old log line.");

            // Act
            LogMaintenanceService.RunMaintenance(_testTempDir, compressDays: 3, retentionDays: 30);

            // Assert
            string expectedZip = Path.Combine(_testTempDir, $"{oldDateStr}.zip");
            File.Exists(expectedZip).Should().BeTrue();
            Directory.Exists(Path.Combine(_testTempDir, oldDateStr)).Should().BeFalse();
        }

        [Fact]
        public void RunMaintenance_RecentLogDirectory_ShouldNotCompress()
        {
            // Arrange: 建立 1 天前之 LOG 資料夾 (未滿 3 天)
            string recentDateStr = DateTime.Today.AddDays(-1).ToString("yyyyMMdd");
            string recentDir = Path.Combine(_testTempDir, recentDateStr, "EQ01");
            Directory.CreateDirectory(recentDir);
            string recentFile = Path.Combine(recentDir, "08.txt");
            File.WriteAllText(recentFile, "This is a recent log line.");

            // Act
            LogMaintenanceService.RunMaintenance(_testTempDir, compressDays: 3, retentionDays: 30);

            // Assert
            string zipPath = Path.Combine(_testTempDir, $"{recentDateStr}.zip");
            File.Exists(zipPath).Should().BeFalse();
            Directory.Exists(Path.Combine(_testTempDir, recentDateStr)).Should().BeTrue();
        }

        [Fact]
        public void RunMaintenance_ExpiredZipFile_ShouldBeDeleted()
        {
            // Arrange: 建立 40 天前之 .zip 檔案 (超過預設 30 天)
            string expiredDateStr = DateTime.Today.AddDays(-40).ToString("yyyyMMdd");
            string expiredZip = Path.Combine(_testTempDir, $"{expiredDateStr}.zip");
            File.WriteAllText(expiredZip, "Dummy zip content");

            // Act
            LogMaintenanceService.RunMaintenance(_testTempDir, compressDays: 3, retentionDays: 30);

            // Assert
            File.Exists(expiredZip).Should().BeFalse();
        }

        [Fact]
        public void GetLogFilePath_SmallFile_ShouldReturnPrimaryPath()
        {
            // Arrange
            string logDir = Path.Combine(_testTempDir, "logs_test");
            Directory.CreateDirectory(logDir);
            DateTime now = new DateTime(2026, 8, 14, 8, 30, 0);

            // Act
            string path = LogMaintenanceService.GetLogFilePath(logDir, now, maxFileSizeMB: 1);

            // Assert
            Path.GetFileName(path).Should().Be("08.txt");
        }

        [Fact]
        public void GetLogFilePath_LargeFile_ShouldRollToNextIndex()
        {
            // Arrange: 建立一個大於 1MB 的 08.txt
            string logDir = Path.Combine(_testTempDir, "logs_test_large");
            Directory.CreateDirectory(logDir);
            DateTime now = new DateTime(2026, 8, 14, 8, 30, 0);
            string primaryPath = Path.Combine(logDir, "08.txt");

            // 寫入 1MB + 10 bytes 的內容
            byte[] dummyData = new byte[1024 * 1024 + 10];
            File.WriteAllBytes(primaryPath, dummyData);

            // Act: 設定上限 1MB
            string path = LogMaintenanceService.GetLogFilePath(logDir, now, maxFileSizeMB: 1);

            // Assert
            Path.GetFileName(path).Should().Be("08_1.txt");
        }
    }
}
