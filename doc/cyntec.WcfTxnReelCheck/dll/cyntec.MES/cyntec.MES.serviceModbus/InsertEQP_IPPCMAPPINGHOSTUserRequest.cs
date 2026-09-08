using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class InsertEQP_IPPCMAPPINGHOSTUserRequest
{
	[MessageBodyMember(Name = "InsertEQP_IPPCMAPPINGHOSTUser", Namespace = "CyntecMES", Order = 0)]
	public InsertEQP_IPPCMAPPINGHOSTUserRequestBody Body;

	public InsertEQP_IPPCMAPPINGHOSTUserRequest()
	{
	}

	public InsertEQP_IPPCMAPPINGHOSTUserRequest(InsertEQP_IPPCMAPPINGHOSTUserRequestBody Body)
	{
		this.Body = Body;
	}
}
