using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class DeleteEQP_EQP_IPPCMAPPINGAPIListRequest
{
	[MessageBodyMember(Name = "DeleteEQP_EQP_IPPCMAPPINGAPIList", Namespace = "CyntecMES", Order = 0)]
	public DeleteEQP_EQP_IPPCMAPPINGAPIListRequestBody Body;

	public DeleteEQP_EQP_IPPCMAPPINGAPIListRequest()
	{
	}

	public DeleteEQP_EQP_IPPCMAPPINGAPIListRequest(DeleteEQP_EQP_IPPCMAPPINGAPIListRequestBody Body)
	{
		this.Body = Body;
	}
}
