using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class UpdateEQP_EQP_IPPCMAPPINGHOSTResponse
{
	[MessageBodyMember(Name = "UpdateEQP_EQP_IPPCMAPPINGHOSTResponse", Namespace = "CyntecMES", Order = 0)]
	public UpdateEQP_EQP_IPPCMAPPINGHOSTResponseBody Body;

	public UpdateEQP_EQP_IPPCMAPPINGHOSTResponse()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGHOSTResponse(UpdateEQP_EQP_IPPCMAPPINGHOSTResponseBody Body)
	{
		this.Body = Body;
	}
}
