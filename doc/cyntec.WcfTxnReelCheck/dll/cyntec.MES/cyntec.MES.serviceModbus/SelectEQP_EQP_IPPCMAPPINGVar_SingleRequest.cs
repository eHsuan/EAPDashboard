using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_EQP_IPPCMAPPINGVar_SingleRequest
{
	[MessageBodyMember(Name = "SelectEQP_EQP_IPPCMAPPINGVar_Single", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_EQP_IPPCMAPPINGVar_SingleRequestBody Body;

	public SelectEQP_EQP_IPPCMAPPINGVar_SingleRequest()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGVar_SingleRequest(SelectEQP_EQP_IPPCMAPPINGVar_SingleRequestBody Body)
	{
		this.Body = Body;
	}
}
