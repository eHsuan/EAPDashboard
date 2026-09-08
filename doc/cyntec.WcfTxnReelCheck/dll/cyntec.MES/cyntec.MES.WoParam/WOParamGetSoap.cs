using System.CodeDom.Compiler;
using System.ServiceModel;

namespace cyntec.MES.WoParam;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://172.28.113.107:8085/websrv/", ConfigurationName = "WoParam.WOParamGetSoap")]
public interface WOParamGetSoap
{
	[OperationContract(Action = "http://172.28.113.107:8085/websrv/HelloWorld", ReplyAction = "*")]
	HelloWorldResponse HelloWorld(HelloWorldRequest request);

	[OperationContract(Action = "http://172.28.113.107:8085/websrv/GetList", ReplyAction = "*")]
	GetListResponse GetList(GetListRequest request);

	[OperationContract(Action = "http://172.28.113.107:8085/websrv/GetListByMatNo", ReplyAction = "*")]
	GetListByMatNoResponse GetListByMatNo(GetListByMatNoRequest request);

	[OperationContract(Action = "http://172.28.113.107:8085/websrv/GetListByWorkCenter", ReplyAction = "*")]
	GetListByWorkCenterResponse GetListByWorkCenter(GetListByWorkCenterRequest request);
}
