using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class DeleteEQP_EQP_IPPCMAPPINGResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string DeleteEQP_EQP_IPPCMAPPINGResult;

	public DeleteEQP_EQP_IPPCMAPPINGResponseBody()
	{
	}

	public DeleteEQP_EQP_IPPCMAPPINGResponseBody(string DeleteEQP_EQP_IPPCMAPPINGResult)
	{
		this.DeleteEQP_EQP_IPPCMAPPINGResult = DeleteEQP_EQP_IPPCMAPPINGResult;
	}
}
