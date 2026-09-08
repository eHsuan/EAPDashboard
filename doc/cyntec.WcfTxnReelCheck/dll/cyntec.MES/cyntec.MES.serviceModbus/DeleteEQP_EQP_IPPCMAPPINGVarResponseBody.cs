using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class DeleteEQP_EQP_IPPCMAPPINGVarResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string DeleteEQP_EQP_IPPCMAPPINGVarResult;

	public DeleteEQP_EQP_IPPCMAPPINGVarResponseBody()
	{
	}

	public DeleteEQP_EQP_IPPCMAPPINGVarResponseBody(string DeleteEQP_EQP_IPPCMAPPINGVarResult)
	{
		this.DeleteEQP_EQP_IPPCMAPPINGVarResult = DeleteEQP_EQP_IPPCMAPPINGVarResult;
	}
}
