using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Protocol.Core.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum CommandType
{
	ALARM,
	SelectParameter,
	ReplySelectParameter,
	EditParameter,
	ReplyEditParameter,
	PP_SELECT,
	ReplyPP_SELECT,
	START,
	ReplySTART,
	InitTraceData,
	ReplyTraceDataMessage,
	ProcessDataReport,
	ReplyProcessDataReport,
	StatusReq,
	ReplyStatusReq,
	StatusChangeReport,
	ReplyStatusChangeReport,
	AreYouThere,
	IamHere,
	TimeCalibrate,
	ReplyTimeCalibrate,
	TrackInReq,
	ReplyTrackInReq,
	TrackOutReq,
	ReplyTrackOutReq,
	TerminalDisplay,
	ReplyTerminalDisplay,
	UserID_Verify,
	ReplyUserID_Verify,
	BatchProcessDataReport,
	ReplyBatchProcessDataReport,
	STOP,
	ReplySTOP,
	PanelInReport,
	ReplyPanelInReport,
	PanelOutReport,
	ReplyPanelOutReport,
	KeyParametersReport,
	ReplyKeyParametersReport
}
