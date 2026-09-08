using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class InsertEQP_EQP_IPPCMAPPINGPcCmdResponse
{
	[MessageBodyMember(Name = "InsertEQP_EQP_IPPCMAPPINGPcCmdResponse", Namespace = "CyntecMES", Order = 0)]
	public InsertEQP_EQP_IPPCMAPPINGPcCmdResponseBody Body;

	public InsertEQP_EQP_IPPCMAPPINGPcCmdResponse()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGPcCmdResponse(InsertEQP_EQP_IPPCMAPPINGPcCmdResponseBody Body)
	{
		this.Body = Body;
	}
}
