using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.WoParam;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class GetListByMatNoResponse
{
	[MessageBodyMember(Name = "GetListByMatNoResponse", Namespace = "http://172.28.113.107:8085/websrv/", Order = 0)]
	public GetListByMatNoResponseBody Body;

	public GetListByMatNoResponse()
	{
	}

	public GetListByMatNoResponse(GetListByMatNoResponseBody Body)
	{
		this.Body = Body;
	}
}
