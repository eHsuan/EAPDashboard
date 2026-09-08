using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class DeleteEQP_EQP_IPPCMAPPINGHOSTResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string DeleteEQP_EQP_IPPCMAPPINGHOSTResult;

	public DeleteEQP_EQP_IPPCMAPPINGHOSTResponseBody()
	{
	}

	public DeleteEQP_EQP_IPPCMAPPINGHOSTResponseBody(string DeleteEQP_EQP_IPPCMAPPINGHOSTResult)
	{
		this.DeleteEQP_EQP_IPPCMAPPINGHOSTResult = DeleteEQP_EQP_IPPCMAPPINGHOSTResult;
	}
}
