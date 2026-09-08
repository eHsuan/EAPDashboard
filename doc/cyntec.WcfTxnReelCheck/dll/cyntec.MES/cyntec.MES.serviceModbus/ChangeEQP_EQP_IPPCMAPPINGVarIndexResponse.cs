using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class ChangeEQP_EQP_IPPCMAPPINGVarIndexResponse
{
	[MessageBodyMember(Name = "ChangeEQP_EQP_IPPCMAPPINGVarIndexResponse", Namespace = "CyntecMES", Order = 0)]
	public ChangeEQP_EQP_IPPCMAPPINGVarIndexResponseBody Body;

	public ChangeEQP_EQP_IPPCMAPPINGVarIndexResponse()
	{
	}

	public ChangeEQP_EQP_IPPCMAPPINGVarIndexResponse(ChangeEQP_EQP_IPPCMAPPINGVarIndexResponseBody Body)
	{
		this.Body = Body;
	}
}
