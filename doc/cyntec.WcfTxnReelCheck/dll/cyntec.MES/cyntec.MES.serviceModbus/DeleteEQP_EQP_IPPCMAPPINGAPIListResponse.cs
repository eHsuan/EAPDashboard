using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class DeleteEQP_EQP_IPPCMAPPINGAPIListResponse
{
	[MessageBodyMember(Name = "DeleteEQP_EQP_IPPCMAPPINGAPIListResponse", Namespace = "CyntecMES", Order = 0)]
	public DeleteEQP_EQP_IPPCMAPPINGAPIListResponseBody Body;

	public DeleteEQP_EQP_IPPCMAPPINGAPIListResponse()
	{
	}

	public DeleteEQP_EQP_IPPCMAPPINGAPIListResponse(DeleteEQP_EQP_IPPCMAPPINGAPIListResponseBody Body)
	{
		this.Body = Body;
	}
}
