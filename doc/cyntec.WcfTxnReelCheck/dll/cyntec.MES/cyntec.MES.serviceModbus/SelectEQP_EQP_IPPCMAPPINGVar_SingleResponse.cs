using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_EQP_IPPCMAPPINGVar_SingleResponse
{
	[MessageBodyMember(Name = "SelectEQP_EQP_IPPCMAPPINGVar_SingleResponse", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_EQP_IPPCMAPPINGVar_SingleResponseBody Body;

	public SelectEQP_EQP_IPPCMAPPINGVar_SingleResponse()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGVar_SingleResponse(SelectEQP_EQP_IPPCMAPPINGVar_SingleResponseBody Body)
	{
		this.Body = Body;
	}
}
