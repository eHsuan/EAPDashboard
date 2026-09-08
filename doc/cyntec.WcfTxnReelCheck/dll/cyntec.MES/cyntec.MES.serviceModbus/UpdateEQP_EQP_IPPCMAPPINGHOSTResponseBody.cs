using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class UpdateEQP_EQP_IPPCMAPPINGHOSTResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string UpdateEQP_EQP_IPPCMAPPINGHOSTResult;

	public UpdateEQP_EQP_IPPCMAPPINGHOSTResponseBody()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGHOSTResponseBody(string UpdateEQP_EQP_IPPCMAPPINGHOSTResult)
	{
		this.UpdateEQP_EQP_IPPCMAPPINGHOSTResult = UpdateEQP_EQP_IPPCMAPPINGHOSTResult;
	}
}
