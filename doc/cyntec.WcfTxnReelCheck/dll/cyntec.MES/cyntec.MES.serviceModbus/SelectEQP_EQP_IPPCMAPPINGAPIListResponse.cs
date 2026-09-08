using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_EQP_IPPCMAPPINGAPIListResponse
{
	[MessageBodyMember(Name = "SelectEQP_EQP_IPPCMAPPINGAPIListResponse", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_EQP_IPPCMAPPINGAPIListResponseBody Body;

	public SelectEQP_EQP_IPPCMAPPINGAPIListResponse()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGAPIListResponse(SelectEQP_EQP_IPPCMAPPINGAPIListResponseBody Body)
	{
		this.Body = Body;
	}
}
