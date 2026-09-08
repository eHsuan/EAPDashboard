using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_EQP_IPPCMAPPINGPcCmdRequest
{
	[MessageBodyMember(Name = "SelectEQP_EQP_IPPCMAPPINGPcCmd", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_EQP_IPPCMAPPINGPcCmdRequestBody Body;

	public SelectEQP_EQP_IPPCMAPPINGPcCmdRequest()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGPcCmdRequest(SelectEQP_EQP_IPPCMAPPINGPcCmdRequestBody Body)
	{
		this.Body = Body;
	}
}
