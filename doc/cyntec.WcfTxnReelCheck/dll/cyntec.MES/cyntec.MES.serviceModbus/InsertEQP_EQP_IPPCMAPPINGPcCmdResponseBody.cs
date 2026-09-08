using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class InsertEQP_EQP_IPPCMAPPINGPcCmdResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string InsertEQP_EQP_IPPCMAPPINGPcCmdResult;

	public InsertEQP_EQP_IPPCMAPPINGPcCmdResponseBody()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGPcCmdResponseBody(string InsertEQP_EQP_IPPCMAPPINGPcCmdResult)
	{
		this.InsertEQP_EQP_IPPCMAPPINGPcCmdResult = InsertEQP_EQP_IPPCMAPPINGPcCmdResult;
	}
}
