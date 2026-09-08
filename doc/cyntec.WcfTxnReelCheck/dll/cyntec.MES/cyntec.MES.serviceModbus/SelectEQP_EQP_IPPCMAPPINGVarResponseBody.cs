using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class SelectEQP_EQP_IPPCMAPPINGVarResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string SelectEQP_EQP_IPPCMAPPINGVarResult;

	public SelectEQP_EQP_IPPCMAPPINGVarResponseBody()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGVarResponseBody(string SelectEQP_EQP_IPPCMAPPINGVarResult)
	{
		this.SelectEQP_EQP_IPPCMAPPINGVarResult = SelectEQP_EQP_IPPCMAPPINGVarResult;
	}
}
