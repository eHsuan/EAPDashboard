using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class UpdateEQP_EQP_IPPCMAPPINGVarRequest
{
	[MessageBodyMember(Name = "UpdateEQP_EQP_IPPCMAPPINGVar", Namespace = "CyntecMES", Order = 0)]
	public UpdateEQP_EQP_IPPCMAPPINGVarRequestBody Body;

	public UpdateEQP_EQP_IPPCMAPPINGVarRequest()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGVarRequest(UpdateEQP_EQP_IPPCMAPPINGVarRequestBody Body)
	{
		this.Body = Body;
	}
}
