using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class ChangeEQP_EQP_IPPCMAPPINGVarIndexRequest
{
	[MessageBodyMember(Name = "ChangeEQP_EQP_IPPCMAPPINGVarIndex", Namespace = "CyntecMES", Order = 0)]
	public ChangeEQP_EQP_IPPCMAPPINGVarIndexRequestBody Body;

	public ChangeEQP_EQP_IPPCMAPPINGVarIndexRequest()
	{
	}

	public ChangeEQP_EQP_IPPCMAPPINGVarIndexRequest(ChangeEQP_EQP_IPPCMAPPINGVarIndexRequestBody Body)
	{
		this.Body = Body;
	}
}
