using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_EQP_IPPCMAPPINGVarRequest
{
	[MessageBodyMember(Name = "SelectEQP_EQP_IPPCMAPPINGVar", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_EQP_IPPCMAPPINGVarRequestBody Body;

	public SelectEQP_EQP_IPPCMAPPINGVarRequest()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGVarRequest(SelectEQP_EQP_IPPCMAPPINGVarRequestBody Body)
	{
		this.Body = Body;
	}
}
