using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class DeleteEQP_EQP_IPPCMAPPINGAPIListResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string DeleteEQP_EQP_IPPCMAPPINGAPIListResult;

	public DeleteEQP_EQP_IPPCMAPPINGAPIListResponseBody()
	{
	}

	public DeleteEQP_EQP_IPPCMAPPINGAPIListResponseBody(string DeleteEQP_EQP_IPPCMAPPINGAPIListResult)
	{
		this.DeleteEQP_EQP_IPPCMAPPINGAPIListResult = DeleteEQP_EQP_IPPCMAPPINGAPIListResult;
	}
}
