using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_EQP_IPPCMAPPINGPcCmdResponse
{
	[MessageBodyMember(Name = "SelectEQP_EQP_IPPCMAPPINGPcCmdResponse", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_EQP_IPPCMAPPINGPcCmdResponseBody Body;

	public SelectEQP_EQP_IPPCMAPPINGPcCmdResponse()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGPcCmdResponse(SelectEQP_EQP_IPPCMAPPINGPcCmdResponseBody Body)
	{
		this.Body = Body;
	}
}
