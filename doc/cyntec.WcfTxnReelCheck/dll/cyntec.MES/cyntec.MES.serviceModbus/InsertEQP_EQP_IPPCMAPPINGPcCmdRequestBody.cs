using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class InsertEQP_EQP_IPPCMAPPINGPcCmdRequestBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string pParameter;

	public InsertEQP_EQP_IPPCMAPPINGPcCmdRequestBody()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGPcCmdRequestBody(string pParameter)
	{
		this.pParameter = pParameter;
	}
}
