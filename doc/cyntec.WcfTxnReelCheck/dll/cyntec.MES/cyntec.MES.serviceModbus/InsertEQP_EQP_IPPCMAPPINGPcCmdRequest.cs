using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class InsertEQP_EQP_IPPCMAPPINGPcCmdRequest
{
	[MessageBodyMember(Name = "InsertEQP_EQP_IPPCMAPPINGPcCmd", Namespace = "CyntecMES", Order = 0)]
	public InsertEQP_EQP_IPPCMAPPINGPcCmdRequestBody Body;

	public InsertEQP_EQP_IPPCMAPPINGPcCmdRequest()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGPcCmdRequest(InsertEQP_EQP_IPPCMAPPINGPcCmdRequestBody Body)
	{
		this.Body = Body;
	}
}
