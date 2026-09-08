using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class UpdateEQP_EQP_IPPCMAPPINGResponse
{
	[MessageBodyMember(Name = "UpdateEQP_EQP_IPPCMAPPINGResponse", Namespace = "CyntecMES", Order = 0)]
	public UpdateEQP_EQP_IPPCMAPPINGResponseBody Body;

	public UpdateEQP_EQP_IPPCMAPPINGResponse()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGResponse(UpdateEQP_EQP_IPPCMAPPINGResponseBody Body)
	{
		this.Body = Body;
	}
}
