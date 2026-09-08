using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class SelectEQP_IPPCMAPPINGHOSTUserResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string SelectEQP_IPPCMAPPINGHOSTUserResult;

	public SelectEQP_IPPCMAPPINGHOSTUserResponseBody()
	{
	}

	public SelectEQP_IPPCMAPPINGHOSTUserResponseBody(string SelectEQP_IPPCMAPPINGHOSTUserResult)
	{
		this.SelectEQP_IPPCMAPPINGHOSTUserResult = SelectEQP_IPPCMAPPINGHOSTUserResult;
	}
}
