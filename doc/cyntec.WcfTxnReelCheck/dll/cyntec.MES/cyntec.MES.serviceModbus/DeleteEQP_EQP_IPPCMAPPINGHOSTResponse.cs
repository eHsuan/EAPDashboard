using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class DeleteEQP_EQP_IPPCMAPPINGHOSTResponse
{
	[MessageBodyMember(Name = "DeleteEQP_EQP_IPPCMAPPINGHOSTResponse", Namespace = "CyntecMES", Order = 0)]
	public DeleteEQP_EQP_IPPCMAPPINGHOSTResponseBody Body;

	public DeleteEQP_EQP_IPPCMAPPINGHOSTResponse()
	{
	}

	public DeleteEQP_EQP_IPPCMAPPINGHOSTResponse(DeleteEQP_EQP_IPPCMAPPINGHOSTResponseBody Body)
	{
		this.Body = Body;
	}
}
