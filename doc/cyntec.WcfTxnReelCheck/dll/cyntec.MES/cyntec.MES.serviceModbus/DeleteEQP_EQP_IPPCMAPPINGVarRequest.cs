using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class DeleteEQP_EQP_IPPCMAPPINGVarRequest
{
	[MessageBodyMember(Name = "DeleteEQP_EQP_IPPCMAPPINGVar", Namespace = "CyntecMES", Order = 0)]
	public DeleteEQP_EQP_IPPCMAPPINGVarRequestBody Body;

	public DeleteEQP_EQP_IPPCMAPPINGVarRequest()
	{
	}

	public DeleteEQP_EQP_IPPCMAPPINGVarRequest(DeleteEQP_EQP_IPPCMAPPINGVarRequestBody Body)
	{
		this.Body = Body;
	}
}
