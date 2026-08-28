using System;
using System.Windows.Forms;

namespace EAPDashboard
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 初始化 MES 專屬 JSON 序列化器設定 (白名單過濾機制)
            Models.MesJsonContractResolver.ConfigureDefaultSettings();

            // 初始化 SQLite 資料庫
            try
            {
                Models.DatabaseHelper.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("資料庫初始化失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new MainForm());
        }
    }
}
