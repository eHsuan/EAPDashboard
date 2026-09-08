/*================================================================================================================================================================
 Description: 
----------------------------------------------------------------------------------------------------------------------------------------------------------------
 Revision History:
 Date          Name                 Issue No.               Description
----------------------------------------------------------------------------------------------------------------------------------------------------------------
 2019/11/06     Haley               V.1                     砍相同程式, 依照設定
================================================================================================================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace cyntec.Windows
{
    static class Program
    {

        public static string UpdateExecutePath = "";
        public static string UpdateExecuteParam = "";
        public static string WinClientParamXML = "";
        public static string SecondParamXML = "";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.Run(new frmReelCheck());
            }
            catch (Exception ex)
            {
                MessageBox.Show("程式異常終止, " + ex.Message);
                string strLogPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log") + @"\";
                cyntec.Tools.cyntecLog.WirteLog(strLogPath, "log.txt", "程式異常終止," + ex.Message);
            }
        }
    }
}
