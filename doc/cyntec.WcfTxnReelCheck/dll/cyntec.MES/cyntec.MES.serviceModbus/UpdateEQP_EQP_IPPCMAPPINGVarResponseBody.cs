using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class UpdateEQP_EQP_IPPCMAPPINGVarResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string UpdateEQP_EQP_IPPCMAPPINGVarResult;

	public UpdateEQP_EQP_IPPCMAPPINGVarResponseBody()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGVarResponseBody(string UpdateEQP_EQP_IPPCMAPPINGVarResult)
	{
		this.UpdateEQP_EQP_IPPCMAPPINGVarResult = UpdateEQP_EQP_IPPCMAPPINGVarResult;
	}
}
