using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class DeleteEQP_IPPCMAPPINGHOSTUserResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string DeleteEQP_IPPCMAPPINGHOSTUserResult;

	public DeleteEQP_IPPCMAPPINGHOSTUserResponseBody()
	{
	}

	public DeleteEQP_IPPCMAPPINGHOSTUserResponseBody(string DeleteEQP_IPPCMAPPINGHOSTUserResult)
	{
		this.DeleteEQP_IPPCMAPPINGHOSTUserResult = DeleteEQP_IPPCMAPPINGHOSTUserResult;
	}
}
