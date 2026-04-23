using EAPDashboard.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;

namespace EAPDashboard
{
    public partial class LogForm : Form
    {
        private readonly EquipmentBase _equipment;
        private bool _isLiveView = true;

        public LogForm(EquipmentBase equipment)
        {
            InitializeComponent();
            _equipment = equipment;
            this.Text = $"Log - {_equipment.Id} ({_equipment.Name})";

            // Set default DateTimePicker values
            dateTimePickerStart.Value = DateTime.Today; // Today at 00:00:00
            dateTimePickerEnd.Value = DateTime.Now;     // Current time

            // btnLiveView_Click event handler
            this.btnLiveView.Click += new System.EventHandler(this.btnLiveView_Click);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);

            // 1. 載入過去 30 分鐘的歷史日誌
            LoadHistoricalLogs(TimeSpan.FromMinutes(30));

            // 2. 訂閱即時日誌事件
            _equipment.LogReceived += AppendLog;
            
            // Unsubscribe when form is closed to prevent memory leaks
            this.FormClosed += (s, e) => _equipment.LogReceived -= AppendLog;
        }

        private void LoadHistoricalLogs(TimeSpan duration)
        {
            DateTime end = DateTime.Now;
            DateTime start = end.Subtract(duration);

            var historicalLogs = GetLogsInRange(start, end);

            richTextBoxLogs.SuspendLayout();
            foreach (var log in historicalLogs)
            {
                AppendLogToRichTextBoxInternal(log.Item1, log.Item2, log.Item3);
            }

            richTextBoxLogs.ResumeLayout();
            richTextBoxLogs.ScrollToCaret();
        }

        private void AppendLog(DateTime timestamp, LogLevel level, string message)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            if (!_isLiveView) return;

            if (richTextBoxLogs.InvokeRequired)
            {
                richTextBoxLogs.BeginInvoke(new Action(() => AppendLogToRichTextBoxInternal(timestamp, level, message)));
                return;
            }

            AppendLogToRichTextBoxInternal(timestamp, level, message);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _isLiveView = false;
            try
            {
                richTextBoxLogs.Clear();
                DateTime start = dateTimePickerStart.Value;
                DateTime end = dateTimePickerEnd.Value;
                var filteredLogs = GetLogsInRange(start, end);

                richTextBoxLogs.SuspendLayout();
                foreach (var log in filteredLogs)
                {
                    AppendLogToRichTextBoxInternal(log.Item1, log.Item2, log.Item3);
                }
                richTextBoxLogs.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching logs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLiveView_Click(object sender, EventArgs e)
        {
            if (!_isLiveView)
            {
                _isLiveView = true;
                richTextBoxLogs.Clear();
                AppendLogToRichTextBoxInternal(DateTime.Now, LogLevel.Info, "Live Log view re-enabled.");
            }
        }

        private void AppendLogToRichTextBoxInternal(DateTime timestamp, LogLevel level, string message)
        {
            Color color;
            switch (level)
            {
                case LogLevel.Fatal:
                case LogLevel.Error: color = Color.Crimson; break;
                case LogLevel.Warn: color = Color.Orange; break;
                case LogLevel.Debug: color = Color.DarkGray; break;
                case LogLevel.Info:
                default: color = Color.Gainsboro; break;
            }

            string cleanMsg = string.IsNullOrEmpty(message) ? "" : message.Replace("\0", "").Replace("\r", "").Replace("\t", "    ");
            var lines = cleanMsg.Split('\n');

            foreach (var line in lines)
            {
                richTextBoxLogs.SelectionStart = richTextBoxLogs.TextLength;
                richTextBoxLogs.SelectionLength = 0;
                richTextBoxLogs.SelectionColor = color;
                richTextBoxLogs.AppendText($"[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{level.ToString().ToUpper()}] : {line}{Environment.NewLine}");
                richTextBoxLogs.SelectionColor = richTextBoxLogs.ForeColor;
            }
            richTextBoxLogs.ScrollToCaret();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime start = dateTimePickerStart.Value;
                DateTime end = dateTimePickerEnd.Value;
                saveFileDialogCsv.FileName = $"{_equipment.Id}_Log_{start:yyyyMMddHHmmss}_{end:yyyyMMddHHmmss}.csv";

                if (saveFileDialogCsv.ShowDialog() == DialogResult.OK)
                {
                    var logs = GetLogsInRange(start, end);
                    var csvBuilder = new StringBuilder();
                    csvBuilder.AppendLine("Timestamp,Level,Message");

                    foreach (var log in logs)
                    {
                        string timestamp = log.Item1.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        string level = log.Item2.ToString();
                        string message = $"\"{log.Item3.Replace("\"", "\"\"")}\"";
                        csvBuilder.AppendLine($"{timestamp},{level},{message}");
                    }

                    File.WriteAllText(saveFileDialogCsv.FileName, csvBuilder.ToString());
                    MessageBox.Show($"Log successfully exported to:\n{saveFileDialogCsv.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting logs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<Tuple<DateTime, LogLevel, string>> GetLogsInRange(DateTime start, DateTime end)
        {
            var logs = new List<Tuple<DateTime, LogLevel, string>>();
            var logLineRegex = new Regex(@"^\[(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3})\] \[(.*?)\] : (.*)$", RegexOptions.Compiled);

            // 遍歷範圍內的每一天
            for (DateTime date = start.Date; date <= end.Date; date = date.AddDays(1))
            {
                string dayDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", date.ToString("yyyyMMdd"), _equipment.Id);
                if (!Directory.Exists(dayDir)) continue;

                // 獲取該目錄下所有小時的 .txt 檔案
                var files = Directory.GetFiles(dayDir, "*.txt").OrderBy(f => f).ToList();

                foreach (var file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if (!int.TryParse(fileName, out int hour)) continue;

                    // 檢查檔案代表的小時是否在搜尋範圍內
                    DateTime fileHour = date.AddHours(hour);
                    if (fileHour.AddHours(1) < start || fileHour > end) continue;

                    string[] lines;
                    try { lines = File.ReadAllLines(file, Encoding.UTF8); } catch { continue; }

                    DateTime currentTimestamp = DateTime.MinValue;
                    LogLevel currentLevel = LogLevel.Info;
                    StringBuilder currentMessage = new StringBuilder();

                    foreach (var line in lines)
                    {
                        var match = logLineRegex.Match(line);
                        if (match.Success)
                        {
                            // 儲存前一筆 Log
                            if (currentTimestamp != DateTime.MinValue && currentTimestamp >= start && currentTimestamp <= end)
                            {
                                logs.Add(new Tuple<DateTime, LogLevel, string>(currentTimestamp, currentLevel, currentMessage.ToString().TrimEnd()));
                            }

                            // 初始化新 Log
                            if (DateTime.TryParseExact(match.Groups[1].Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out currentTimestamp))
                            {
                                Enum.TryParse(match.Groups[2].Value.Trim(), true, out currentLevel);
                                currentMessage = new StringBuilder(match.Groups[3].Value);
                            }
                        }
                        else if (currentTimestamp != DateTime.MinValue)
                        {
                            // 多行訊息處理
                            currentMessage.AppendLine().Append(line);
                        }
                    }

                    // 加入最後一筆
                    if (currentTimestamp != DateTime.MinValue && currentTimestamp >= start && currentTimestamp <= end)
                    {
                        logs.Add(new Tuple<DateTime, LogLevel, string>(currentTimestamp, currentLevel, currentMessage.ToString().TrimEnd()));
                    }
                }
            }
            return logs;
        }
    }
}
