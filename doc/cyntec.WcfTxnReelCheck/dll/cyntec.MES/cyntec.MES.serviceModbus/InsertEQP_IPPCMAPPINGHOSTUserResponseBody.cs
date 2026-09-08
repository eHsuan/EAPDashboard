using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class InsertEQP_IPPCMAPPINGHOSTUserResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string InsertEQP_IPPCMAPPINGHOSTUserResult;

	public InsertEQP_IPPCMAPPINGHOSTUserResponseBody()
	{
	}

	public InsertEQP_IPPCMAPPINGHOSTUserResponseBody(string InsertEQP_IPPCMAPPINGHOSTUserResult)
	{
		this.InsertEQP_IPPCMAPPINGHOSTUserResult = InsertEQP_IPPCMAPPINGHOSTUserResult;
	}
}
