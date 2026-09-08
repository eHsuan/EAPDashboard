using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class SelectEQP_EQP_IPPCMAPPINGVar_SingleResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string SelectEQP_EQP_IPPCMAPPINGVar_SingleResult;

	public SelectEQP_EQP_IPPCMAPPINGVar_SingleResponseBody()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGVar_SingleResponseBody(string SelectEQP_EQP_IPPCMAPPINGVar_SingleResult)
	{
		this.SelectEQP_EQP_IPPCMAPPINGVar_SingleResult = SelectEQP_EQP_IPPCMAPPINGVar_SingleResult;
	}
}
