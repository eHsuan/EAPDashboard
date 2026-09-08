using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class UpdateEQP_EQP_IPPCMAPPINGPcCmdResponse
{
	[MessageBodyMember(Name = "UpdateEQP_EQP_IPPCMAPPINGPcCmdResponse", Namespace = "CyntecMES", Order = 0)]
	public UpdateEQP_EQP_IPPCMAPPINGPcCmdResponseBody Body;

	public UpdateEQP_EQP_IPPCMAPPINGPcCmdResponse()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGPcCmdResponse(UpdateEQP_EQP_IPPCMAPPINGPcCmdResponseBody Body)
	{
		this.Body = Body;
	}
}
