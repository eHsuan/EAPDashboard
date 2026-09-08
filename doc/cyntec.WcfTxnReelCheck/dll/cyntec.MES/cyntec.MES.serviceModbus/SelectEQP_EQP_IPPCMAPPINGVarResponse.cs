using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_EQP_IPPCMAPPINGVarResponse
{
	[MessageBodyMember(Name = "SelectEQP_EQP_IPPCMAPPINGVarResponse", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_EQP_IPPCMAPPINGVarResponseBody Body;

	public SelectEQP_EQP_IPPCMAPPINGVarResponse()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGVarResponse(SelectEQP_EQP_IPPCMAPPINGVarResponseBody Body)
	{
		this.Body = Body;
	}
}
