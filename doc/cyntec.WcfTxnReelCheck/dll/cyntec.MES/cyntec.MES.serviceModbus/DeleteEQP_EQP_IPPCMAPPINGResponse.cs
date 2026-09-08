using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class DeleteEQP_EQP_IPPCMAPPINGResponse
{
	[MessageBodyMember(Name = "DeleteEQP_EQP_IPPCMAPPINGResponse", Namespace = "CyntecMES", Order = 0)]
	public DeleteEQP_EQP_IPPCMAPPINGResponseBody Body;

	public DeleteEQP_EQP_IPPCMAPPINGResponse()
	{
	}

	public DeleteEQP_EQP_IPPCMAPPINGResponse(DeleteEQP_EQP_IPPCMAPPINGResponseBody Body)
	{
		this.Body = Body;
	}
}
