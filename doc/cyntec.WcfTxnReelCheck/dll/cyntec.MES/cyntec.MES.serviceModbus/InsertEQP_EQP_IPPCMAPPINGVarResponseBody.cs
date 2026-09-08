using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class InsertEQP_EQP_IPPCMAPPINGVarResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string InsertEQP_EQP_IPPCMAPPINGVarResult;

	public InsertEQP_EQP_IPPCMAPPINGVarResponseBody()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGVarResponseBody(string InsertEQP_EQP_IPPCMAPPINGVarResult)
	{
		this.InsertEQP_EQP_IPPCMAPPINGVarResult = InsertEQP_EQP_IPPCMAPPINGVarResult;
	}
}
