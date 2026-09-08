using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class UpdateEQP_EQP_IPPCMAPPINGVarResponse
{
	[MessageBodyMember(Name = "UpdateEQP_EQP_IPPCMAPPINGVarResponse", Namespace = "CyntecMES", Order = 0)]
	public UpdateEQP_EQP_IPPCMAPPINGVarResponseBody Body;

	public UpdateEQP_EQP_IPPCMAPPINGVarResponse()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGVarResponse(UpdateEQP_EQP_IPPCMAPPINGVarResponseBody Body)
	{
		this.Body = Body;
	}
}
