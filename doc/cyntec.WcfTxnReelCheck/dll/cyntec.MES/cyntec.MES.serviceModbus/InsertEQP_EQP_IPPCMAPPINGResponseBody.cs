using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class InsertEQP_EQP_IPPCMAPPINGResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string InsertEQP_EQP_IPPCMAPPINGResult;

	public InsertEQP_EQP_IPPCMAPPINGResponseBody()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGResponseBody(string InsertEQP_EQP_IPPCMAPPINGResult)
	{
		this.InsertEQP_EQP_IPPCMAPPINGResult = InsertEQP_EQP_IPPCMAPPINGResult;
	}
}
