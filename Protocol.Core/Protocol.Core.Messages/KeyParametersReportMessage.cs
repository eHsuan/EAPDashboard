using System;
using System.Collections.Generic;
using Protocol.Core.Base;
using Protocol.Core.Enums;
using Newtonsoft.Json;

namespace Protocol.Core.Messages
{
    [CommandMapping(CommandType.KeyParametersReport)]
    public class KeyParametersReportMessage : BaseMessage
    {
        public KeyParametersReportMessage()
        {
            CMD = CommandType.KeyParametersReport;
            ParameterList = new List<KeyParameter>();
        }

        [JsonProperty("ParameterList")]
        public List<KeyParameter> ParameterList { get; set; }
    }

    public class KeyParameter
    {
        [JsonProperty("Parameter")]
        public string Parameter { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("LSL", NullValueHandling = NullValueHandling.Ignore)]
        public string LSL { get; set; }

        [JsonProperty("USL", NullValueHandling = NullValueHandling.Ignore)]
        public string USL { get; set; }
    }
}
