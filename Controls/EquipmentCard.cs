using EAPDashboard.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EAPDashboard.Controls
{
    public partial class EquipmentCard : UserControl
    {
        private EquipmentBase _equipment;

        public EquipmentCard(EquipmentBase equipment)
        {
            InitializeComponent();
            _equipment = equipment;
            _equipment.PropertyChanged += Equipment_PropertyChanged;
            
            if (btnReel != null)
            {
                btnReel.Click += (s, e) => OpenReelForm();
            }
            BindClickEvent(this); // Make the card and all its children clickable (excluding buttons)
            
            UpdateUI();
        }

        private void OpenLogForm()
        {
            // Check if a log form for this equipment is already open
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is LogForm logForm && logForm.Text.Contains(_equipment.Id))
                {
                    logForm.BringToFront();
                    return;
                }
            }

            var newLogForm = new LogForm(_equipment);
            newLogForm.Show();
        }

        private void OpenReelForm()
        {
            // Check if a reel form for this equipment is already open
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is EquipmentReelForm reelForm && reelForm.Text.Contains(_equipment.Id))
                {
                    reelForm.BringToFront();
                    return;
                }
            }

            var newReelForm = new EquipmentReelForm(_equipment);
            newReelForm.Show();
        }

        // Recursively add click event to all child controls except buttons
        private void BindClickEvent(Control control)
        {
            if (control is Button) return; // 避免與按鈕專屬點擊事件衝突

            control.Click += (s, e) => OpenLogForm();
            foreach (Control child in control.Controls)
            {
                BindClickEvent(child);
            }
        }

        private void Equipment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Ensure UI updates are on the UI thread
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateUI));
            }
            else
            {
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            // Update all controls based on the equipment's state
            lblEquipmentId.Text = _equipment.Id;
            lblEquipmentName.Text = !string.IsNullOrEmpty(_equipment.Name) ? _equipment.Name : "";
            lblProtocol.Text = _equipment.Protocol;
            lblRecipe.Text = $"Recipe: {_equipment.CurrentRecipe ?? "N/A"}";
            
            // Connection Status
            lblConnection.Text = _equipment.Connection.ToString();
            switch (_equipment.Connection)
            {
                case ConnectionStatus.Connected:
                    lblConnection.ForeColor = Color.LimeGreen;
                    break;
                case ConnectionStatus.Connecting:
                    lblConnection.ForeColor = Color.Orange;
                    break;
                case ConnectionStatus.Disconnected:
                    lblConnection.ForeColor = Color.Crimson;
                    break;
            }

            // State and Status Strip
            lblStatus.Text = _equipment.State.ToString();
            panelStatus.BackColor = EquipmentBase.GetColorForState(_equipment.State);

            // 若設備名稱或編號為 TP 開頭，則顯示 Reel 專案按鈕
            bool isTp = (_equipment.Name?.StartsWith("TP", StringComparison.OrdinalIgnoreCase) == true) ||
                        (_equipment.Id?.StartsWith("TP", StringComparison.OrdinalIgnoreCase) == true);
            if (btnReel != null)
            {
                btnReel.Visible = isTp;
            }
        }

        // Clean up event handler to prevent memory leaks
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_equipment != null)
                {
                    _equipment.PropertyChanged -= Equipment_PropertyChanged;
                }
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
