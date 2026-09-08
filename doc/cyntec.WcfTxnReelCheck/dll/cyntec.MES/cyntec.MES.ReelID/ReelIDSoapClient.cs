using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace cyntec.MES.ReelID;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ReelIDSoapClient : ClientBase<ReelIDSoap>, ReelIDSoap
{
	public ReelIDSoapClient()
	{
	}

	public ReelIDSoapClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ReelIDSoapClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ReelIDSoapClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ReelIDSoapClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public string HelloWorld()
	{
		return base.Channel.HelloWorld();
	}

	public ReelBarcode GetDecodeBarcode(string encodeBarcode)
	{
		return base.Channel.GetDecodeBarcode(encodeBarcode);
	}

	public ReturnMsg UpdateReelID(string reelID, string workCenterTypeCode, string statusCode, string statusRemark, string updateUser)
	{
		return base.Channel.UpdateReelID(reelID, workCenterTypeCode, statusCode, statusRemark, updateUser);
	}

	public ReturnMsg UpdateReelID2(string reelID, string workCenterTypeCode, string statusCode, string statusRemark, string updateUser, string logKeyword)
	{
		return base.Channel.UpdateReelID2(reelID, workCenterTypeCode, statusCode, statusRemark, updateUser, logKeyword);
	}

	public ReturnMsg UpdateTPReelID(string reelID, string statusCode, string statusRemark, string machNo, string startTime, string endTime, string updateUser)
	{
		return base.Channel.UpdateTPReelID(reelID, statusCode, statusRemark, machNo, startTime, endTime, updateUser);
	}

	public ReturnMsg UpdateTPReelID2(string reelID, string statusCode, string statusRemark, string machNo, string startTime, string endTime, string updateUser, string logKeyword)
	{
		return base.Channel.UpdateTPReelID2(reelID, statusCode, statusRemark, machNo, startTime, endTime, updateUser, logKeyword);
	}

	public ReelStatus[] GetReelStatus()
	{
		return base.Channel.GetReelStatus();
	}

	public WO_ReelID GetByReelID(string reelID)
	{
		return base.Channel.GetByReelID(reelID);
	}

	public WO_ReelID GetByReelID2(string reelID, string logKeyword)
	{
		return base.Channel.GetByReelID2(reelID, logKeyword);
	}

	public WO_ReelID[] GetListByWONo(string woNo)
	{
		return base.Channel.GetListByWONo(woNo);
	}

	public bool AddLog(WO_ReelIDLog pModel)
	{
		return base.Channel.AddLog(pModel);
	}

	public WO_ReelIDLog[] GetLog(string pReelID)
	{
		return base.Channel.GetLog(pReelID);
	}
}
