using System;
using System.Windows.Forms;

namespace EAP.ReelCheck.App;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            MessageBox.Show($"未預期系統異常：{(e.ExceptionObject as Exception)?.Message}", "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        };

        Application.Run(new MainForm());
    }
}
