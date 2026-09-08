using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class SelectEQP_EQP_IPPCMAPPINGPcCmdResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string SelectEQP_EQP_IPPCMAPPINGPcCmdResult;

	public SelectEQP_EQP_IPPCMAPPINGPcCmdResponseBody()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGPcCmdResponseBody(string SelectEQP_EQP_IPPCMAPPINGPcCmdResult)
	{
		this.SelectEQP_EQP_IPPCMAPPINGPcCmdResult = SelectEQP_EQP_IPPCMAPPINGPcCmdResult;
	}
}
