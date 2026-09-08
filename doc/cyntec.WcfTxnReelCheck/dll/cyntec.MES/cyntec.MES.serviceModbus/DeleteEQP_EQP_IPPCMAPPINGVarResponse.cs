using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class DeleteEQP_EQP_IPPCMAPPINGVarResponse
{
	[MessageBodyMember(Name = "DeleteEQP_EQP_IPPCMAPPINGVarResponse", Namespace = "CyntecMES", Order = 0)]
	public DeleteEQP_EQP_IPPCMAPPINGVarResponseBody Body;

	public DeleteEQP_EQP_IPPCMAPPINGVarResponse()
	{
	}

	public DeleteEQP_EQP_IPPCMAPPINGVarResponse(DeleteEQP_EQP_IPPCMAPPINGVarResponseBody Body)
	{
		this.Body = Body;
	}
}
