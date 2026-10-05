using System;
using System.Drawing;
using System.Windows.Forms;
using EAPDashboard.Models;

namespace EAPDashboard
{
    public partial class ConfigDiffForm : Form
    {
        private readonly ConfigDiffResult _diff;

        public ConfigDiffForm(ConfigDiffResult diff)
        {
            InitializeComponent();
            _diff = diff ?? new ConfigDiffResult();
            RenderDiff();
        }

        private void RenderDiff()
        {
            // 1. 橫幅狀態與警示提示
            if (_diff.HasGlobalChanges)
            {
                pnlTopBanner.BackColor = Color.FromArgb(120, 35, 35);
                lblBannerText.ForeColor = Color.Gold;
                lblBannerText.Text = "⚠️ 警示：本次修改包含「全域參數」，套用後一律需【重新啟動程式】才能完全生效！";
            }
            else
            {
                pnlTopBanner.BackColor = Color.FromArgb(28, 68, 45);
                lblBannerText.ForeColor = Color.White;
                lblBannerText.Text = "💡 提示：本次修改僅涉及機台設定，未變更之機台維持正常連線，套用後【無需重啟】即時生效。";
            }

            // 2. 呈現全域變更表格
            dgvGlobalChanges.Rows.Clear();
            if (_diff.GlobalChanges != null && _diff.GlobalChanges.Count > 0)
            {
                foreach (var g in _diff.GlobalChanges)
                {
                    int idx = dgvGlobalChanges.Rows.Add(g.PropertyCategory, g.PropertyName, g.OldValue, g.NewValue);
                    dgvGlobalChanges.Rows[idx].DefaultCellStyle.ForeColor = Color.Yellow;
                }
            }
            else
            {
                int idx = dgvGlobalChanges.Rows.Add("全域設定", "(無任何全域參數變更)", "-", "-");
                dgvGlobalChanges.Rows[idx].DefaultCellStyle.ForeColor = Color.LightGray;
            }

            // 3. 呈現機台設備差分表格
            dgvEquipmentDiffs.Rows.Clear();
            int unchangedCount = 0;
            int addedCount = 0;
            int removedCount = 0;
            int modifiedCount = 0;

            if (_diff.EquipmentDiffs != null && _diff.EquipmentDiffs.Count > 0)
            {
                foreach (var eq in _diff.EquipmentDiffs)
                {
                    string details = eq.ChangeDetails != null && eq.ChangeDetails.Count > 0
                        ? string.Join("；", eq.ChangeDetails)
                        : "-";

                    int idx = dgvEquipmentDiffs.Rows.Add(eq.DiffTypeDisplay, eq.EquipmentId, eq.EquipmentName, details);
                    var row = dgvEquipmentDiffs.Rows[idx];

                    switch (eq.DiffType)
                    {
                        case EquipmentDiffType.Added:
                            addedCount++;
                            row.DefaultCellStyle.ForeColor = Color.LightGreen;
                            break;
                        case EquipmentDiffType.Removed:
                            removedCount++;
                            row.DefaultCellStyle.ForeColor = Color.Salmon;
                            break;
                        case EquipmentDiffType.Modified:
                            modifiedCount++;
                            row.DefaultCellStyle.ForeColor = Color.Khaki;
                            break;
                        case EquipmentDiffType.Unchanged:
                        default:
                            unchangedCount++;
                            row.DefaultCellStyle.ForeColor = Color.LightGray;
                            break;
                    }
                }
            }
            else
            {
                int idx = dgvEquipmentDiffs.Rows.Add("⚪ 維持不變", "-", "-", "無機台變更");
                dgvEquipmentDiffs.Rows[idx].DefaultCellStyle.ForeColor = Color.LightGray;
            }

            // 4. 底部統計摘要
            int globalCount = _diff.GlobalChanges?.Count ?? 0;
            lblSummary.Text = $"變更摘要：全域變更 {globalCount} 項 ｜ 機台狀態：新增 {addedCount} 台、停用 {removedCount} 台、變更 {modifiedCount} 台、維持不變 {unchangedCount} 台 (連線不中斷)";
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
