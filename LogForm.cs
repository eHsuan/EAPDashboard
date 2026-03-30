using EAPDashboard.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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
            //if (!string.Equals(id, _equipment.Id, StringComparison.OrdinalIgnoreCase))
            //    return;

            if (this.IsDisposed || !this.IsHandleCreated) return;

            // If not in live view, don't append new logs from the event
            if (!_isLiveView) return;

            if (richTextBoxLogs.InvokeRequired)
            {
                // Use BeginInvoke for a more responsive UI
                richTextBoxLogs.BeginInvoke(new Action(() => AppendLogToRichTextBoxInternal(timestamp, level, message)));
                //if (richTextBoxLogs.InvokeRequired)
                //{
                //    richTextBoxLogs.BeginInvoke(
                //        new Action(() => AppendLog(timestamp, level, message, id))
                //    );
                //    return;
                //}

                return;
            }

            AppendLogToRichTextBoxInternal(timestamp, level, message);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _isLiveView = false; // Disable live view to show search results
            try
            {
                richTextBoxLogs.Clear();
                
                DateTime start = dateTimePickerStart.Value;
                DateTime end = dateTimePickerEnd.Value;
                var filteredLogs = GetLogsInRange(start, end);

                // Display filtered logs
                richTextBoxLogs.SuspendLayout(); // Suspend layout to improve performance
                foreach (var log in filteredLogs)
                {
                    // Directly append to the RichTextBox for search results, bypassing the live view check
                    AppendLogToRichTextBoxInternal(log.Item1, log.Item2, log.Item3);
                }
                richTextBoxLogs.ResumeLayout(); // Resume layout
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
                case LogLevel.Error:
                    color = Color.Crimson;
                    break;
                case LogLevel.Warn:
                    color = Color.Orange;
                    break;
                case LogLevel.Debug:
                    color = Color.DarkGray;
                    break;
                case LogLevel.Info:
                default:
                    color = Color.Gainsboro;
                    break;
            }
            if (string.IsNullOrEmpty(message))
                message = string.Empty;
            else
            {
                message = message
                    .Replace("\0", "")       // ���� Null �r��
                    .Replace("\r", "")       // ���� CR
                    .Replace("\t", "    ");  // Tab �����Ů�
            }

            // 3?? ��h���r
            var lines = message.Split('\n');
            foreach (var line in lines)
            {
                richTextBoxLogs.SelectionStart = richTextBoxLogs.TextLength;
                richTextBoxLogs.SelectionLength = 0;

                richTextBoxLogs.SelectionColor = color;
                richTextBoxLogs.AppendText($"[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{level.ToString().ToUpper()}] {line}{Environment.NewLine}");
                richTextBoxLogs.SelectionColor = richTextBoxLogs.ForeColor;
            }

            // 4?? �۰ʱ��ʨ쩳
            richTextBoxLogs.ScrollToCaret();
            //richTextBoxLogs.SelectionStart = richTextBoxLogs.TextLength;
            //richTextBoxLogs.SelectionLength = 0;

            //richTextBoxLogs.SelectionColor = color;
            //richTextBoxLogs.AppendText($"[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{level.ToString().ToUpper()}] {message}{Environment.NewLine}");
            //richTextBoxLogs.SelectionColor = richTextBoxLogs.ForeColor;

            //// Auto-scroll to the end
            //richTextBoxLogs.ScrollToCaret();
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
                    csvBuilder.AppendLine("Timestamp,Level,Message"); // CSV Header

                    foreach (var log in logs)
                    {
                        string timestamp = log.Item1.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        string level = log.Item2.ToString();
                        string message = $"\"{log.Item3.Replace("\"", "\"\"")}\""; // Escape quotes
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

        //private List<Tuple<DateTime, LogLevel, string>> GetLogsInRange(DateTime start, DateTime end)
        //{
        //    var logs = new List<Tuple<DateTime, LogLevel, string>>();
        //    // Updated Regex to capture the new EquipmentID field
        //    var logPattern = new Regex(@"^\[(.*?)\]\s+\[(.*?)\]\s+\[(.*?)\]\s+(.*)$", RegexOptions.Compiled);
        //    string equipmentTag = $"[{_equipment.Id}]";

        //    // Iterate through each hour from the start time to the end time
        //    for (DateTime currentHour = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0); currentHour <= end; currentHour = currentHour.AddHours(1))
        //    {
        //        // Construct the directory for the current hour
        //        string hourlyDirectory = Path.Combine("logs", currentHour.ToString("yyyyMMdd"));
        //        if (!Directory.Exists(hourlyDirectory)) continue;

        //        // Define the base file name for this hour
        //        string baseFileName = $"{currentHour:HH}.txt";

        //        // Get all log files for this hour, including rolled files
        //        // This will return files like "14.txt", "14.txt.1", "14.txt.2", etc.
        //        string[] hourFiles = Directory.GetFiles(hourlyDirectory, $"{baseFileName}*");

        //        // Sort files to ensure correct chronological order (e.g., 14.txt, 14.txt.1, 14.txt.2)
        //        Array.Sort(hourFiles, (f1, f2) => {
        //            string name1 = Path.GetFileName(f1);
        //            string name2 = Path.GetFileName(f2);

        //            if (name1 == baseFileName) return -1; // Base file (without suffix) comes first
        //            if (name2 == baseFileName) return 1;  // Base file (without suffix) comes first

        //            // For rolled files, sort numerically by suffix (e.g., .1 before .2)
        //            int suffix1 = 0;
        //            int suffix2 = 0;
        //            // Extract numeric suffix after the last dot
        //            if (name1.LastIndexOf('.') > name1.IndexOf('.')) 
        //                int.TryParse(name1.Substring(name1.LastIndexOf('.') + 1), out suffix1);
        //            if (name2.LastIndexOf('.') > name2.IndexOf('.')) 
        //                int.TryParse(name2.Substring(name2.LastIndexOf('.') + 1), out suffix2);

        //            return suffix1.CompareTo(suffix2);
        //        });

        //        foreach (string logFile in hourFiles)
        //        {
        //            string[] lines = File.ReadAllLines(logFile, Encoding.UTF8);
        //            foreach (string line in lines)
        //            {
        //                // Quick check to see if the line contains the equipment ID tag before using regex
        //                if (!line.Contains(equipmentTag)) continue;

        //                Match match = logPattern.Match(line);
        //                if (!match.Success) continue;

        //                // Group 3 is the new EquipmentID
        //                string timestampStr = match.Groups[1].Value;
        //                string levelStr = match.Groups[2].Value.Trim();
        //                string eqIdStr = match.Groups[3].Value.Trim();
        //                string message = match.Groups[4].Value;

        //                // Final check to ensure it's the correct equipment
        //                if (eqIdStr != _equipment.Id) continue;

        //                if (DateTime.TryParseExact(timestampStr, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timestamp))
        //                {
        //                    if (timestamp >= start && timestamp <= end)
        //                    {
        //                        if (Enum.TryParse(levelStr, true, out LogLevel level))
        //                        {
        //                            logs.Add(new Tuple<DateTime, LogLevel, string>(timestamp, level, message));
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return logs;
        //}
        private List<Tuple<DateTime, LogLevel, string>> GetLogsInRange(DateTime start, DateTime end)
        {
            var logs = new List<Tuple<DateTime, LogLevel, string>>();
            var logPattern = new Regex(@"^\[(.*?)\]\s+\[(.*?)\]\s+\[(.*?)\]\s+(.*)$", RegexOptions.Compiled);
            string equipmentTag = $"[{_equipment.Id}]";

            // �M���C�Ӥp��
            for (DateTime currentHour = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
                 currentHour <= end;
                 currentHour = currentHour.AddHours(1))
            {
                string hourlyDirectory = Path.Combine("logs", currentHour.ToString("yyyyMMdd"));
                if (!Directory.Exists(hourlyDirectory)) continue;

                string baseFileName = $"{currentHour:HH}.txt";
                string[] hourFiles = Directory.GetFiles(hourlyDirectory, $"{baseFileName}*");
                Array.Sort(hourFiles);

                foreach (var logFile in hourFiles)
                {
                    string[] lines;
                    try
                    {
                        lines = File.ReadAllLines(logFile, Encoding.UTF8);
                    }
                    catch { continue; }

                    string currentMessage = null;
                    DateTime currentTimestamp = DateTime.MinValue;
                    LogLevel currentLevel = LogLevel.Info;
                    string currentEqId = null;

                    foreach (var line in lines)
                    {
                        var match = logPattern.Match(line);
                        if (match.Success)
                        {
                            // �s log �}�l�A����W�@�� log �[�J���G
                            if (currentMessage != null &&
                                currentEqId == _equipment.Id &&
                                currentTimestamp >= start && currentTimestamp <= end)
                            {
                                logs.Add(new Tuple<DateTime, LogLevel, string>(currentTimestamp, currentLevel, currentMessage));
                            }

                            // ��l�Ʒs�� log
                            currentTimestamp = DateTime.ParseExact(match.Groups[1].Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            currentLevel = Enum.TryParse(match.Groups[2].Value.Trim(), true, out LogLevel lvl) ? lvl : LogLevel.Info;
                            currentEqId = match.Groups[3].Value.Trim();
                            currentMessage = match.Groups[4].Value;
                        }
                        else
                        {
                            // ������]�h�� JSON �Ψ�L�^�� ���[�� currentMessage
                            if (currentMessage != null)
                            {
                                currentMessage += Environment.NewLine + line;
                            }
                        }
                    }

                    // ��̫�@�� log �[�J���G
                    if (currentMessage != null &&
                        currentEqId == _equipment.Id &&
                        currentTimestamp >= start && currentTimestamp <= end)
                    {
                        logs.Add(new Tuple<DateTime, LogLevel, string>(currentTimestamp, currentLevel, currentMessage));
                    }
                }
            }

            return logs;
        }
    }
}
