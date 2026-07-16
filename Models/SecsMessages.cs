using System;
using System.Collections.Generic;
using Secs4Net;

namespace EAPDashboard.Models
{
    /// <summary>
    /// S1F1: Are You There Request (Host or Eqp initiated)
    /// </summary>
    public class S1F1 : SecsMessage
    {
        public S1F1() : base(1, 1, "Are You There Request", replyExpected: true)
        {
        }
    }

    /// <summary>
    /// S1F13: Establish Communications Request
    /// </summary>
    public class S1F13 : SecsMessage
    {
        public S1F13() : base(1, 13, "Establish Communications Request", replyExpected: true)
        {
            SecsItem = Item.L(); // Empty List
        }
    }

    /// <summary>
    /// S1F17: Request ON-LINE
    /// </summary>
    public class S1F17 : SecsMessage
    {
        public S1F17() : base(1, 17, "Request ON-LINE", replyExpected: true)
        {
        }
    }

    /// <summary>
    /// S2F23: Trace Initialize Request
    /// </summary>
    public class S2F23 : SecsMessage
    {
        public S2F23(uint trid, uint dsper, uint tots, List<string> svids) 
            : base(2, 23, "Trace Initialize Request", replyExpected: true)
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
            SecsItem = Item.L(
                Item.U4(trid),
                Item.A(dsperStr),
                Item.U4(tots),
                Item.U4(1), // REPGSZ
                Item.L(svidItems)
            );
        }
    }

    /// <summary>
    /// S2F31: Date and Time Set Request
    /// </summary>
    public class S2F31 : SecsMessage
    {
        public S2F31(string timeStr) : base(2, 31, "Date and Time Set Request", replyExpected: true)
        {
            SecsItem = Item.A(timeStr);
        }
    }

    /// <summary>
    /// S2F37: Enable/Disable Event Report
    /// </summary>
    public class S2F37 : SecsMessage
    {
        public S2F37(bool enable) : base(2, 37, "Enable/Disable Event Report", replyExpected: true)
        {
            SecsItem = Item.L(
                Item.Boolean(enable),
                Item.L() // Empty list means ALL Events
            );
        }
    }

    /// <summary>
    /// S2F41: Host Command Send
    /// </summary>
    public class S2F41 : SecsMessage
    {
        public S2F41(string rcmd, Dictionary<string, string> parameters = null) 
            : base(2, 41, "Host Command Send", replyExpected: true)
        {
            var paramList = new List<Item>();
            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    paramList.Add(Item.L(Item.A(kvp.Key), Item.A(kvp.Value)));
                }
            }

            SecsItem = Item.L(
                Item.A(rcmd),
                Item.L(paramList)
            );
        }
    }

    /// <summary>
    /// S5F3: Enable/Disable Alarm Send
    /// </summary>
    public class S5F3 : SecsMessage
    {
        public S5F3(bool enable) : base(5, 3, "Enable/Disable Alarm Send", replyExpected: true)
        {
            byte aled = enable ? (byte)0x80 : (byte)0x00;
            SecsItem = Item.L(
                Item.B(aled),
                Item.U4(0) // 0 means ALL ALIDs
            );
        }
    }
}
