using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class UpdateEQP_EQP_IPPCMAPPINGPcCmdRequest
{
	[MessageBodyMember(Name = "UpdateEQP_EQP_IPPCMAPPINGPcCmd", Namespace = "CyntecMES", Order = 0)]
	public UpdateEQP_EQP_IPPCMAPPINGPcCmdRequestBody Body;

	public UpdateEQP_EQP_IPPCMAPPINGPcCmdRequest()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGPcCmdRequest(UpdateEQP_EQP_IPPCMAPPINGPcCmdRequestBody Body)
	{
		this.Body = Body;
	}
}
