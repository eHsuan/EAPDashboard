using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class UpdateEQP_EQP_IPPCMAPPINGResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string UpdateEQP_EQP_IPPCMAPPINGResult;

	public UpdateEQP_EQP_IPPCMAPPINGResponseBody()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGResponseBody(string UpdateEQP_EQP_IPPCMAPPINGResult)
	{
		this.UpdateEQP_EQP_IPPCMAPPINGResult = UpdateEQP_EQP_IPPCMAPPINGResult;
	}
}
