using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class InsertEQP_EQP_IPPCMAPPINGHOSTResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string InsertEQP_EQP_IPPCMAPPINGHOSTResult;

	public InsertEQP_EQP_IPPCMAPPINGHOSTResponseBody()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGHOSTResponseBody(string InsertEQP_EQP_IPPCMAPPINGHOSTResult)
	{
		this.InsertEQP_EQP_IPPCMAPPINGHOSTResult = InsertEQP_EQP_IPPCMAPPINGHOSTResult;
	}
}
