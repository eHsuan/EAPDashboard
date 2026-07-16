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
        #region Stream 1: Communications & Connection
        
        /// <summary>
        /// S1F1: Are You There Request
        /// </summary>
        public static SecsMessage S1F1()
        {
            return new SecsMessage(1, 1) { Name = "Are You There Request" };
        }

        /// <summary>
        /// S1F2: Are You There Reply
        /// </summary>
        public static SecsMessage S1F2(string eqpName, string eqpId)
        {
            return new SecsMessage(1, 2)
            {
                Name = "Are You There Reply",
                SecsItem = Item.L(Item.A(eqpName), Item.A(eqpId))
            };
        }

        /// <summary>
        /// S1F3: Selected Status Request (Read VIDs)
        /// </summary>
        public static SecsMessage S1F3(List<uint> vids)
        {
            var vidItems = new List<Item>();
            if (vids != null)
            {
                foreach (var vid in vids)
                {
                    vidItems.Add(Item.U4(vid));
                }
            }
            return new SecsMessage(1, 3)
            {
                Name = "Selected Status Request",
                SecsItem = Item.L(vidItems)
            };
        }

        /// <summary>
        /// S1F11: Status Variable Namelist Request
        /// </summary>
        public static SecsMessage S1F11(List<uint> svids = null)
        {
            var svidItems = new List<Item>();
            if (svids != null)
            {
                foreach (var svid in svids)
                {
                    svidItems.Add(Item.U4(svid));
                }
            }
            return new SecsMessage(1, 11)
            {
                Name = "Status Variable Namelist Request",
                SecsItem = Item.L(svidItems)
            };
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
        /// S1F14: Establish Communications Reply
        /// </summary>
        public static SecsMessage S1F14(byte commack = 0)
        {
            return new SecsMessage(1, 14)
            {
                Name = "Establish Communications Reply",
                SecsItem = Item.B(commack)
            };
        }

        /// <summary>
        /// S1F15: Request OFF-LINE
        /// </summary>
        public static SecsMessage S1F15()
        {
            return new SecsMessage(1, 15) { Name = "Request OFF-LINE" };
        }

        /// <summary>
        /// S1F17: Request ON-LINE
        /// </summary>
        public static SecsMessage S1F17()
        {
            return new SecsMessage(1, 17) { Name = "Request ON-LINE" };
        }

        /// <summary>
        /// S1F21: Equipment Constant Namelist Request
        /// </summary>
        public static SecsMessage S1F21(List<uint> ecids = null)
        {
            var ecidItems = new List<Item>();
            if (ecids != null)
            {
                foreach (var ecid in ecids)
                {
                    ecidItems.Add(Item.U4(ecid));
                }
            }
            return new SecsMessage(1, 21)
            {
                Name = "Equipment Constant Namelist Request",
                SecsItem = Item.L(ecidItems)
            };
        }

        #endregion

        #region Stream 2: Equipment Constant & Configs

        /// <summary>
        /// S2F13: Equipment Constant Request (Read ECIDs)
        /// </summary>
        public static SecsMessage S2F13(List<uint> ecids = null)
        {
            var ecidItems = new List<Item>();
            if (ecids != null)
            {
                foreach (var ecid in ecids)
                {
                    ecidItems.Add(Item.U4(ecid));
                }
            }
            return new SecsMessage(2, 13)
            {
                Name = "Equipment Constant Request",
                SecsItem = Item.L(ecidItems)
            };
        }

        /// <summary>
        /// S2F15: New Equipment Constant Send (Write ECIDs)
        /// </summary>
        public static SecsMessage S2F15(Dictionary<uint, string> ecidValues)
        {
            var ecList = new List<Item>();
            if (ecidValues != null)
            {
                foreach (var kvp in ecidValues)
                {
                    ecList.Add(Item.L(
                        Item.U4(kvp.Key),
                        Item.A(kvp.Value)
                    ));
                }
            }
            return new SecsMessage(2, 15)
            {
                Name = "New Equipment Constant Send",
                SecsItem = Item.L(ecList)
            };
        }

        /// <summary>
        /// S2F17: Date and Time Request
        /// </summary>
        public static SecsMessage S2F17()
        {
            return new SecsMessage(2, 17) { Name = "Date and Time Request" };
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
        /// S2F33: Define Report
        /// </summary>
        public static SecsMessage S2F33(uint dataid, uint rptid, List<uint> svids)
        {
            var svidItems = new List<Item>();
            if (svids != null)
            {
                foreach (var svid in svids)
                {
                    svidItems.Add(Item.U4(svid));
                }
            }
            
            return new SecsMessage(2, 33)
            {
                Name = "Define Report",
                SecsItem = Item.L(
                    Item.U4(dataid),
                    Item.L(
                        Item.L(
                            Item.U4(rptid),
                            Item.L(svidItems)
                        )
                    )
                )
            };
        }

        /// <summary>
        /// S2F35: Link Event Report
        /// </summary>
        public static SecsMessage S2F35(uint dataid, uint ceid, List<uint> rptids)
        {
            var rptidItems = new List<Item>();
            if (rptids != null)
            {
                foreach (var rptid in rptids)
                {
                    rptidItems.Add(Item.U4(rptid));
                }
            }

            return new SecsMessage(2, 35)
            {
                Name = "Link Event Report",
                SecsItem = Item.L(
                    Item.U4(dataid),
                    Item.L(
                        Item.L(
                            Item.U4(ceid),
                            Item.L(rptidItems)
                        )
                    )
                )
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
        /// S2F43: Reset Spooling Data Request
        /// </summary>
        public static SecsMessage S2F43()
        {
            return new SecsMessage(2, 43) { Name = "Reset Spooling Data Request" };
        }

        #endregion

        #region Stream 5: Alarms

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

        /// <summary>
        /// S5F5: List Alarms Request
        /// </summary>
        public static SecsMessage S5F5(List<uint> alids = null)
        {
            var alidItems = new List<Item>();
            if (alids != null)
            {
                foreach (var alid in alids)
                {
                    alidItems.Add(Item.U4(alid));
                }
            }
            return new SecsMessage(5, 5)
            {
                Name = "List Alarms Request",
                SecsItem = Item.L(alidItems)
            };
        }

        /// <summary>
        /// S5F2: Alarm Report Acknowledge
        /// </summary>
        public static SecsMessage S5F2()
        {
            return new SecsMessage(5, 2)
            {
                Name = "Alarm Report Acknowledge",
                SecsItem = Item.B(0)
            };
        }

        #endregion

        #region Stream 6: Reports & Trace

        /// <summary>
        /// S6F2: Trace Data Acknowledge
        /// </summary>
        public static SecsMessage S6F2()
        {
            return new SecsMessage(6, 2)
            {
                Name = "Trace Data Acknowledge",
                SecsItem = Item.B(0)
            };
        }

        /// <summary>
        /// S6F12: Event Report Acknowledge
        /// </summary>
        public static SecsMessage S6F12()
        {
            return new SecsMessage(6, 12)
            {
                Name = "Event Report Acknowledge",
                SecsItem = Item.B(0)
            };
        }

        #endregion

        #region Stream 7: Process Programs (Recipes)

        /// <summary>
        /// S7F1: Process Program Request (Download Recipe)
        /// </summary>
        public static SecsMessage S7F1(string ppid)
        {
            return new SecsMessage(7, 1)
            {
                Name = "Process Program Request",
                SecsItem = Item.A(ppid)
            };
        }

        /// <summary>
        /// S7F3: Process Program Send (Upload Recipe)
        /// </summary>
        public static SecsMessage S7F3(string ppid, byte[] ppbody)
        {
            return new SecsMessage(7, 3)
            {
                Name = "Process Program Send",
                SecsItem = Item.L(
                    Item.A(ppid),
                    Item.B(ppbody)
                )
            };
        }

        /// <summary>
        /// S7F17: Process Program Delete
        /// </summary>
        public static SecsMessage S7F17(List<string> ppids = null)
        {
            var ppidItems = new List<Item>();
            if (ppids != null)
            {
                foreach (var ppid in ppids)
                {
                    ppidItems.Add(Item.A(ppid));
                }
            }
            return new SecsMessage(7, 17)
            {
                Name = "Process Program Delete",
                SecsItem = Item.L(ppidItems)
            };
        }

        /// <summary>
        /// S7F19: Current EPID Request (Recipe List Request)
        /// </summary>
        public static SecsMessage S7F19()
        {
            return new SecsMessage(7, 19) { Name = "Current EPID Request" };
        }

        #endregion
    }

    #region Decoded Receive Data Carriers

    /// <summary>
    /// S1F12: Status Variable Namelist Item
    /// </summary>
    public class S1F12Item
    {
        public uint Svid { get; set; }
        public string Name { get; set; }
        public string Units { get; set; }
    }

    /// <summary>
    /// S1F22: Equipment Constant Namelist Item
    /// </summary>
    public class S1F22Item
    {
        public uint Ecid { get; set; }
        public string Name { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public string DefValue { get; set; }
        public string Units { get; set; }
    }

    /// <summary>
    /// S5F1: Alarm Report Send 解包資料
    /// </summary>
    public class S5F1Data
    {
        public byte Alcd { get; set; }
        public uint Alid { get; set; }
        public string Altx { get; set; }
        public bool IsSet => (Alcd & 0x80) != 0 || Alcd == 1;
    }

    /// <summary>
    /// S5F6: List Alarm Item
    /// </summary>
    public class S5F6Item
    {
        public byte Alcd { get; set; }
        public uint Alid { get; set; }
        public string Altx { get; set; }
        public bool IsSet => (Alcd & 0x80) != 0 || Alcd == 1;
    }

    /// <summary>
    /// S6F1: Trace Data Send 解包資料
    /// </summary>
    public class S6F1Data
    {
        public uint Trid { get; set; }
        public uint Smpln { get; set; }
        public string Stime { get; set; }
        public Item Values { get; set; }
    }

    /// <summary>
    /// S6F11: Event Report Send 解包資料
    /// </summary>
    public class S6F11Data
    {
        public uint Dataid { get; set; }
        public uint Ceid { get; set; }
        public Item Reports { get; set; }
    }

    #endregion

    #region SECS Message Extensions

    public static class SecsMessageExtensions
    {
        /// <summary>
        /// 解包 S1F12: Status Variable Namelist Reply
        /// </summary>
        public static List<S1F12Item> ParseS1F12(this SecsMessage msg)
        {
            var root = msg.SecsItem;
            var result = new List<S1F12Item>();
            if (root == null || root.Format != SecsFormat.List) return result;

            foreach (var item in root.Items)
            {
                if (item.Format == SecsFormat.List && item.Count >= 3)
                {
                    result.Add(new S1F12Item
                    {
                        Svid = item.Items[0].FirstValue<uint>(),
                        Name = item.Items[1].GetString(),
                        Units = item.Items[2].GetString()
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 解包 S1F22: Equipment Constant Namelist Reply
        /// </summary>
        public static List<S1F22Item> ParseS1F22(this SecsMessage msg)
        {
            var root = msg.SecsItem;
            var result = new List<S1F22Item>();
            if (root == null || root.Format != SecsFormat.List) return result;

            foreach (var item in root.Items)
            {
                if (item.Format == SecsFormat.List && item.Count >= 6)
                {
                    result.Add(new S1F22Item
                    {
                        Ecid = item.Items[0].FirstValue<uint>(),
                        Name = item.Items[1].GetString(),
                        MinValue = ExtractSingleValue(item.Items[2]),
                        MaxValue = ExtractSingleValue(item.Items[3]),
                        DefValue = ExtractSingleValue(item.Items[4]),
                        Units = item.Items[5].GetString()
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 解包 S1F4: Selected Equipment Status Data (VID values list)
        /// </summary>
        public static List<string> ParseS1F4(this SecsMessage msg)
        {
            var root = msg.SecsItem;
            var result = new List<string>();
            if (root == null) return result;
            if (root.Format == SecsFormat.List)
            {
                foreach (var item in root.Items)
                {
                    result.Add(ExtractSingleValue(item));
                }
            }
            else
            {
                result.Add(ExtractSingleValue(root));
            }
            return result;
        }

        /// <summary>
        /// 解包 S2F18: Date and Time Reply
        /// </summary>
        public static string ParseS2F18(this SecsMessage msg)
        {
            var root = msg.SecsItem;
            if (root == null) return string.Empty;
            return root.GetString();
        }

        /// <summary>
        /// 解包 S5F1: Alarm Report Send
        /// </summary>
        public static S5F1Data ParseS5F1(this SecsMessage msg)
        {
            var root = msg.SecsItem;
            if (root == null || root.Count < 3) throw new Exception("Invalid S5F1 structure.");
            return new S5F1Data
            {
                Alcd = root.Items[0].FirstValue<byte>(),
                Alid = root.Items[1].FirstValue<uint>(),
                Altx = root.Items[2].GetString()
            };
        }

        /// <summary>
        /// 解包 S5F6: List Alarm Data
        /// </summary>
        public static List<S5F6Item> ParseS5F6(this SecsMessage msg)
        {
            var root = msg.SecsItem;
            var result = new List<S5F6Item>();
            if (root == null || root.Format != SecsFormat.List) return result;

            foreach (var item in root.Items)
            {
                if (item.Format == SecsFormat.List && item.Count >= 3)
                {
                    result.Add(new S5F6Item
                    {
                        Alcd = item.Items[0].FirstValue<byte>(),
                        Alid = item.Items[1].FirstValue<uint>(),
                        Altx = item.Items[2].GetString()
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 解包 S6F1: Trace Data Send
        /// </summary>
        public static S6F1Data ParseS6F1(this SecsMessage msg)
        {
            var root = msg.SecsItem;
            if (root == null || root.Count < 4) throw new Exception("Invalid S6F1 structure.");
            return new S6F1Data
            {
                Trid = root.Items[0].FirstValue<uint>(),
                Smpln = root.Items[1].FirstValue<uint>(),
                Stime = root.Items[2].GetString(),
                Values = root.Items[3]
            };
        }

        /// <summary>
        /// 解包 S6F11: Event Report Send
        /// </summary>
        public static S6F11Data ParseS6F11(this SecsMessage msg)
        {
            var root = msg.SecsItem;
            if (root == null || root.Count < 3) throw new Exception("Invalid S6F11 structure.");
            return new S6F11Data
            {
                Dataid = root.Items[0].FirstValue<uint>(),
                Ceid = root.Items[1].FirstValue<uint>(),
                Reports = root.Items[2]
            };
        }

        /// <summary>
        /// 解包 S7F20: Current EPID Data (Recipe List)
        /// </summary>
        public static List<string> ParseS7F20(this SecsMessage msg)
        {
            var root = msg.SecsItem;
            var result = new List<string>();
            if (root == null) return result;
            if (root.Format == SecsFormat.List)
            {
                foreach (var item in root.Items)
                {
                    result.Add(item.GetString());
                }
            }
            else
            {
                result.Add(root.GetString());
            }
            return result;
        }

        private static string ExtractSingleValue(Item item)
        {
            if (item == null) return string.Empty;
            switch (item.Format)
            {
                case SecsFormat.ASCII: return item.GetString();
                case SecsFormat.U1: case SecsFormat.Binary: return item.FirstValue<byte>().ToString();
                case SecsFormat.U2: return item.FirstValue<ushort>().ToString();
                case SecsFormat.U4: return item.FirstValue<uint>().ToString();
                case SecsFormat.I4: return item.FirstValue<int>().ToString();
                case SecsFormat.Boolean: return item.FirstValue<bool>().ToString();
                default: return string.Empty;
            }
        }
    }

    #endregion
}
