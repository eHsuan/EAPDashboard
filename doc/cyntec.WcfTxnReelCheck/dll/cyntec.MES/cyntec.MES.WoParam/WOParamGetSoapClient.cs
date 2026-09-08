using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace cyntec.MES.WoParam;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class WOParamGetSoapClient : ClientBase<WOParamGetSoap>, WOParamGetSoap
{
	public WOParamGetSoapClient()
	{
	}

	public WOParamGetSoapClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public WOParamGetSoapClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public WOParamGetSoapClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public WOParamGetSoapClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	HelloWorldResponse WOParamGetSoap.HelloWorld(HelloWorldRequest request)
	{
		return base.Channel.HelloWorld(request);
	}

	public string HelloWorld()
	{
		HelloWorldRequest helloWorldRequest = new HelloWorldRequest();
		helloWorldRequest.Body = new HelloWorldRequestBody();
		HelloWorldResponse helloWorldResponse = ((WOParamGetSoap)this).HelloWorld(helloWorldRequest);
		return helloWorldResponse.Body.HelloWorldResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetListResponse WOParamGetSoap.GetList(GetListRequest request)
	{
		return base.Channel.GetList(request);
	}

	public Param[] GetList(string WONo)
	{
		GetListRequest getListRequest = new GetListRequest();
		getListRequest.Body = new GetListRequestBody();
		getListRequest.Body.WONo = WONo;
		GetListResponse list = ((WOParamGetSoap)this).GetList(getListRequest);
		return list.Body.GetListResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetListByMatNoResponse WOParamGetSoap.GetListByMatNo(GetListByMatNoRequest request)
	{
		return base.Channel.GetListByMatNo(request);
	}

	public Param[] GetListByMatNo(string WONo, string WorkCenterNo, string MatNo)
	{
		GetListByMatNoRequest getListByMatNoRequest = new GetListByMatNoRequest();
		getListByMatNoRequest.Body = new GetListByMatNoRequestBody();
		getListByMatNoRequest.Body.WONo = WONo;
		getListByMatNoRequest.Body.WorkCenterNo = WorkCenterNo;
		getListByMatNoRequest.Body.MatNo = MatNo;
		GetListByMatNoResponse listByMatNo = ((WOParamGetSoap)this).GetListByMatNo(getListByMatNoRequest);
		return listByMatNo.Body.GetListByMatNoResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetListByWorkCenterResponse WOParamGetSoap.GetListByWorkCenter(GetListByWorkCenterRequest request)
	{
		return base.Channel.GetListByWorkCenter(request);
	}

	public Param[] GetListByWorkCenter(string WONo, string WorkCenterNo)
	{
		GetListByWorkCenterRequest getListByWorkCenterRequest = new GetListByWorkCenterRequest();
		getListByWorkCenterRequest.Body = new GetListByWorkCenterRequestBody();
		getListByWorkCenterRequest.Body.WONo = WONo;
		getListByWorkCenterRequest.Body.WorkCenterNo = WorkCenterNo;
		GetListByWorkCenterResponse listByWorkCenter = ((WOParamGetSoap)this).GetListByWorkCenter(getListByWorkCenterRequest);
		return listByWorkCenter.Body.GetListByWorkCenterResult;
	}
}
