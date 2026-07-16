using System;
using System.Collections.Generic;
using Secs4Net;

namespace EAPDashboard.Models
{
    /// <summary>
    /// SECS-II 訊息強型別靜態工廠，預設好每個 SxFy 訊息的內部欄位結構，
    /// 提供維護開發人員極佳的 IntelliSense 提示與開發安全性。
    /// </summary>
    public static class SecsMessages
    {
        /// <summary>
        /// S1F1: Are You There Request
        /// </summary>
        public static SecsMessage S1F1()
        {
            return new SecsMessage(1, 1) { Name = "Are You There Request" };
        }

        /// <summary>
        /// S1F13: Establish Communications Request
        /// </summary>
        public static SecsMessage S1F13()
        {
            return new SecsMessage(1, 13) 
            { 
                Name = "Establish Communications Request", 
                SecsItem = Item.L() 
            };
        }

        /// <summary>
        /// S1F17: Request ON-LINE
        /// </summary>
        public static SecsMessage S1F17()
        {
            return new SecsMessage(1, 17) { Name = "Request ON-LINE" };
        }

        /// <summary>
        /// S2F23: Trace Initialize Request
        /// </summary>
        public static SecsMessage S2F23(uint trid, uint dsper, uint tots, List<string> svids)
        {
            var svidItems = new List<Item>();
            if (svids != null)
            {
                foreach (var svid in svids)
                {
                    if (uint.TryParse(svid, out uint numericSvid))
                    {
                        svidItems.Add(Item.U4(numericSvid));
                    }
                    else
                    {
                        svidItems.Add(Item.A(svid));
                    }
                }
            }

            string dsperStr = dsper.ToString("D6"); 
            return new SecsMessage(2, 23)
            {
                Name = "Trace Initialize Request",
                SecsItem = Item.L(
                    Item.U4(trid),
                    Item.A(dsperStr),
                    Item.U4(tots),
                    Item.U4(1), // REPGSZ
                    Item.L(svidItems)
                )
            };
        }

        /// <summary>
        /// S2F31: Date and Time Set Request
        /// </summary>
        public static SecsMessage S2F31(string timeStr)
        {
            return new SecsMessage(2, 31) 
            { 
                Name = "Date and Time Set Request", 
                SecsItem = Item.A(timeStr) 
            };
        }

        /// <summary>
        /// S2F37: Enable/Disable Event Report
        /// </summary>
        public static SecsMessage S2F37(bool enable)
        {
            return new SecsMessage(2, 37)
            {
                Name = "Enable/Disable Event Report",
                SecsItem = Item.L(
                    Item.Boolean(enable),
                    Item.L() // Empty list means ALL Events
                )
            };
        }

        /// <summary>
        /// S2F41: Host Command Send
        /// </summary>
        public static SecsMessage S2F41(string rcmd, Dictionary<string, string> parameters = null)
        {
            var paramList = new List<Item>();
            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    paramList.Add(Item.L(Item.A(kvp.Key), Item.A(kvp.Value)));
                }
            }

            return new SecsMessage(2, 41)
            {
                Name = "Host Command Send",
                SecsItem = Item.L(
                    Item.A(rcmd),
                    Item.L(paramList)
                )
            };
        }

        /// <summary>
        /// S5F3: Enable/Disable Alarm Send
        /// </summary>
        public static SecsMessage S5F3(bool enable)
        {
            byte aled = enable ? (byte)0x80 : (byte)0x00;
            return new SecsMessage(5, 3)
            {
                Name = "Enable/Disable Alarm Send",
                SecsItem = Item.L(
                    Item.B(aled),
                    Item.U4(0) // 0 means ALL ALIDs
                )
            };
        }
    }
}
