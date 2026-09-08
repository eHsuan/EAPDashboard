using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.WoParam;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class GetListByWorkCenterRequest
{
	[MessageBodyMember(Name = "GetListByWorkCenter", Namespace = "http://172.28.113.107:8085/websrv/", Order = 0)]
	public GetListByWorkCenterRequestBody Body;

	public GetListByWorkCenterRequest()
	{
	}

	public GetListByWorkCenterRequest(GetListByWorkCenterRequestBody Body)
	{
		this.Body = Body;
	}
}
