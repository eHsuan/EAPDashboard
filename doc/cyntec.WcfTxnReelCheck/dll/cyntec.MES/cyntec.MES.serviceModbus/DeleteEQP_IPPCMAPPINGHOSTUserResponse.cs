using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class DeleteEQP_IPPCMAPPINGHOSTUserResponse
{
	[MessageBodyMember(Name = "DeleteEQP_IPPCMAPPINGHOSTUserResponse", Namespace = "CyntecMES", Order = 0)]
	public DeleteEQP_IPPCMAPPINGHOSTUserResponseBody Body;

	public DeleteEQP_IPPCMAPPINGHOSTUserResponse()
	{
	}

	public DeleteEQP_IPPCMAPPINGHOSTUserResponse(DeleteEQP_IPPCMAPPINGHOSTUserResponseBody Body)
	{
		this.Body = Body;
	}
}
