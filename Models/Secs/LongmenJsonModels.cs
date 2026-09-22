using System;
using System.Collections.Generic;

namespace EAPDashboard.Models.Secs
{
    public class ConverAlarmMsg
    {
        public byte[] FileContent { get; set; } = Array.Empty<byte>();
    }

    public class PlatingRawPayload
    {
        public string FileContent { get; set; } = string.Empty;
    }

    public class LongmenJson_Data
    {
        public string Rtn_Result { get; set; } = string.Empty;
        public List<RtnValue> Rtn_Value { get; set; } = new List<RtnValue>();
        public string Rtn_Msg { get; set; } = string.Empty;
        public string Rtn_DateTime { get; set; } = string.Empty;
    }

    public class RtnValue
    {
        public string Col1 { get; set; } = string.Empty;
        public string Col2 { get; set; } = string.Empty;
        public string Col3 { get; set; } = string.Empty;
        public string Col4 { get; set; } = string.Empty;
        public string Col5 { get; set; } = string.Empty;
        public string Col6 { get; set; } = string.Empty;
        public string Col7 { get; set; } = string.Empty;
        public string Col8 { get; set; } = string.Empty;
        public string Col9 { get; set; } = string.Empty;
        public string Col10 { get; set; } = string.Empty;
        public string Col11 { get; set; } = string.Empty;
        public string Col12 { get; set; } = string.Empty;
        public string Col13 { get; set; } = string.Empty;
        public string Col14 { get; set; } = string.Empty;
        public string Col15 { get; set; } = string.Empty;
        public string Col16 { get; set; } = string.Empty;
        public string Col17 { get; set; } = string.Empty;
        public string Col18 { get; set; } = string.Empty;
    }
}
