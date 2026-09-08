using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_IPPCMAPPINGHOSTUserRequest
{
	[MessageBodyMember(Name = "SelectEQP_IPPCMAPPINGHOSTUser", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_IPPCMAPPINGHOSTUserRequestBody Body;

	public SelectEQP_IPPCMAPPINGHOSTUserRequest()
	{
	}

	public SelectEQP_IPPCMAPPINGHOSTUserRequest(SelectEQP_IPPCMAPPINGHOSTUserRequestBody Body)
	{
		this.Body = Body;
	}
}
