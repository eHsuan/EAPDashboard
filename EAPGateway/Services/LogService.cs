using System;
using System.IO;

namespace EAPGateway.Services
{
    public enum LogLevel { Debug, Info, Warn, Error, Fatal }

    public class LogService
    {
        private readonly object _lock = new object();
        public event Action<DateTime, LogLevel, string, string>? OnLog;

        public void Log(LogLevel level, string message, string eqpId = "Gateway")
        {
            var now = DateTime.Now;
            OnLog?.Invoke(now, level, message, eqpId);

            try
            {
                lock (_lock)
                {
                    string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", now.ToString("yyyyMMdd"), eqpId);
                    if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
                    
                    string filePath = Path.Combine(logDir, $"{now:HH}.txt");
                    string logLine = $"[{now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] : {message}{Environment.NewLine}";
                    File.AppendAllText(filePath, logLine);
                }
            }
            catch { /* 避免日誌失敗導致系統崩潰 */ }
        }

        public void Debug(string message, string eqpId = "Gateway") => Log(LogLevel.Debug, message, eqpId);
        public void Info(string message, string eqpId = "Gateway") => Log(LogLevel.Info, message, eqpId);
        public void Warn(string message, string eqpId = "Gateway") => Log(LogLevel.Warn, message, eqpId);
        public void Error(string message, string eqpId = "Gateway") => Log(LogLevel.Error, message, eqpId);
    }
}
