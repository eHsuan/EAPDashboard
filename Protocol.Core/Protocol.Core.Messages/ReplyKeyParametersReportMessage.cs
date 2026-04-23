using System;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages
{
    [CommandMapping(CommandType.ReplyKeyParametersReport)]
    public class ReplyKeyParametersReportMessage : ReplyBase
    {
        public ReplyKeyParametersReportMessage()
        {
            CMD = CommandType.ReplyKeyParametersReport;
        }
    }
}
