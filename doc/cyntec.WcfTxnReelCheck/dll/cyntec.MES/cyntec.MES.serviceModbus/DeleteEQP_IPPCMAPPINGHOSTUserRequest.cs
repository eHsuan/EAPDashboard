using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class DeleteEQP_IPPCMAPPINGHOSTUserRequest
{
	[MessageBodyMember(Name = "DeleteEQP_IPPCMAPPINGHOSTUser", Namespace = "CyntecMES", Order = 0)]
	public DeleteEQP_IPPCMAPPINGHOSTUserRequestBody Body;

	public DeleteEQP_IPPCMAPPINGHOSTUserRequest()
	{
	}

	public DeleteEQP_IPPCMAPPINGHOSTUserRequest(DeleteEQP_IPPCMAPPINGHOSTUserRequestBody Body)
	{
		this.Body = Body;
	}
}
