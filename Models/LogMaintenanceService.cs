using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EAPDashboard.Models
{
    public class LogSettings
    {
        public bool Enabled { get; set; } = true;
        public int CompressDays { get; set; } = 3;
        public int RetentionDays { get; set; } = 30;
        public int MaxFileSizeMB { get; set; } = 20;
    }

    public static class LogMaintenanceService
    {
        public static LogSettings CurrentSettings { get; set; } = new LogSettings();
        private static Timer _timer;
        private static readonly object _syncLock = new object();
        private static bool _isRunning = false;

        public static void Start(LogSettings settings = null)
        {
            if (settings != null)
            {
                CurrentSettings = settings;
            }

            if (!CurrentSettings.Enabled) return;

            // 啟動時先以背景 Task 執行一次
            Task.Run(() => RunMaintenanceSafe());

            // 每 6 小時定時執行一次維運清理
            _timer?.Dispose();
            _timer = new Timer(_ => Task.Run(() => RunMaintenanceSafe()), null, TimeSpan.FromHours(6), TimeSpan.FromHours(6));
        }

        public static void Stop()
        {
            _timer?.Dispose();
            _timer = null;
        }

        public static void RunMaintenanceSafe()
        {
            lock (_syncLock)
            {
                if (_isRunning) return;
                _isRunning = true;
            }

            try
            {
                string logsRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                RunMaintenance(logsRoot, CurrentSettings.CompressDays, CurrentSettings.RetentionDays);
            }
            catch
            {
                // 避免背景清理例外影響主程序
            }
            finally
            {
                lock (_syncLock)
                {
                    _isRunning = false;
                }
            }
        }

        public static void RunMaintenance(string logsRootPath, int compressDays = 3, int retentionDays = 30)
        {
            if (string.IsNullOrEmpty(logsRootPath) || !Directory.Exists(logsRootPath)) return;

            var today = DateTime.Today;

            // 1. 執行舊 LOG 資料夾 ZIP 壓縮封存
            if (compressDays > 0)
            {
                var dirEntries = Directory.GetDirectories(logsRootPath);
                foreach (var dir in dirEntries)
                {
                    string dirName = Path.GetFileName(dir);
                    if (DateTime.TryParseExact(dirName, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dirDate))
                    {
                        // 若資料夾日期距今超過 compressDays，且不是今天或昨天的資料夾
                        if (dirDate.Date <= today.AddDays(-compressDays).Date)
                        {
                            string zipPath = Path.Combine(logsRootPath, $"{dirName}.zip");
                            try
                            {
                                if (File.Exists(zipPath))
                                {
                                    // 若已有 zip，先刪除舊 zip 重新完整封存
                                    File.Delete(zipPath);
                                }

                                ZipFile.CreateFromDirectory(dir, zipPath, CompressionLevel.Optimal, false);

                                // 驗證 ZIP 檔案產生成功且大小大於 0
                                if (File.Exists(zipPath) && new FileInfo(zipPath).Length > 0)
                                {
                                    Directory.Delete(dir, true);
                                }
                            }
                            catch
                            {
                                // 若檔案被鎖定，略過由下次排程處理
                            }
                        }
                    }
                }
            }

            // 2. 執行過期日誌 (ZIP 檔案與殘留資料夾) 自動刪除
            if (retentionDays > 0)
            {
                var cutoffDate = today.AddDays(-retentionDays).Date;

                // 檢查 .zip 檔案
                var zipFiles = Directory.GetFiles(logsRootPath, "*.zip");
                foreach (var zip in zipFiles)
                {
                    string zipName = Path.GetFileNameWithoutExtension(zip);
                    if (DateTime.TryParseExact(zipName, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime zipDate))
                    {
                        if (zipDate.Date < cutoffDate)
                        {
                            try { File.Delete(zip); } catch { }
                        }
                    }
                }

                // 檢查殘留未壓縮之過期資料夾
                var remainingDirs = Directory.GetDirectories(logsRootPath);
                foreach (var dir in remainingDirs)
                {
                    string dirName = Path.GetFileName(dir);
                    if (DateTime.TryParseExact(dirName, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dirDate))
                    {
                        if (dirDate.Date < cutoffDate)
                        {
                            try { Directory.Delete(dir, true); } catch { }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根據單檔大小上限（MaxFileSizeMB），計算或切分當前小時的日誌檔名
        /// </summary>
        public static string GetLogFilePath(string logDir, DateTime timestamp, int maxFileSizeMB = 20)
        {
            long maxBytes = (long)maxFileSizeMB * 1024 * 1024;
            string baseFileName = $"{timestamp:HH}";
            string primaryPath = Path.Combine(logDir, $"{baseFileName}.txt");

            if (maxFileSizeMB <= 0) return primaryPath;

            if (!File.Exists(primaryPath))
            {
                return primaryPath;
            }

            try
            {
                var fi = new FileInfo(primaryPath);
                if (fi.Length < maxBytes)
                {
                    return primaryPath;
                }

                // 當前主檔超過上限，搜尋或建立 08_1.txt, 08_2.txt ...
                int index = 1;
                while (true)
                {
                    string rollPath = Path.Combine(logDir, $"{baseFileName}_{index}.txt");
                    if (!File.Exists(rollPath))
                    {
                        return rollPath;
                    }

                    var rollFi = new FileInfo(rollPath);
                    if (rollFi.Length < maxBytes)
                    {
                        return rollPath;
                    }
                    index++;
                }
            }
            catch
            {
                return primaryPath;
            }
        }
    }
}
