using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class UpdateEQP_EQP_IPPCMAPPINGPcCmdResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string UpdateEQP_EQP_IPPCMAPPINGPcCmdResult;

	public UpdateEQP_EQP_IPPCMAPPINGPcCmdResponseBody()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGPcCmdResponseBody(string UpdateEQP_EQP_IPPCMAPPINGPcCmdResult)
	{
		this.UpdateEQP_EQP_IPPCMAPPINGPcCmdResult = UpdateEQP_EQP_IPPCMAPPINGPcCmdResult;
	}
}
