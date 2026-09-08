using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_IPPCMAPPINGHOSTUserResponse
{
	[MessageBodyMember(Name = "SelectEQP_IPPCMAPPINGHOSTUserResponse", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_IPPCMAPPINGHOSTUserResponseBody Body;

	public SelectEQP_IPPCMAPPINGHOSTUserResponse()
	{
	}

	public SelectEQP_IPPCMAPPINGHOSTUserResponse(SelectEQP_IPPCMAPPINGHOSTUserResponseBody Body)
	{
		this.Body = Body;
	}
}
