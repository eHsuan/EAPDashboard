using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class UpdateEQP_EQP_IPPCMAPPINGAPIListResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string UpdateEQP_EQP_IPPCMAPPINGAPIListResult;

	public UpdateEQP_EQP_IPPCMAPPINGAPIListResponseBody()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGAPIListResponseBody(string UpdateEQP_EQP_IPPCMAPPINGAPIListResult)
	{
		this.UpdateEQP_EQP_IPPCMAPPINGAPIListResult = UpdateEQP_EQP_IPPCMAPPINGAPIListResult;
	}
}
