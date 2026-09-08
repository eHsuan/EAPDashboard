using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class InsertEQP_IPPCMAPPINGHOSTUserResponse
{
	[MessageBodyMember(Name = "InsertEQP_IPPCMAPPINGHOSTUserResponse", Namespace = "CyntecMES", Order = 0)]
	public InsertEQP_IPPCMAPPINGHOSTUserResponseBody Body;

	public InsertEQP_IPPCMAPPINGHOSTUserResponse()
	{
	}

	public InsertEQP_IPPCMAPPINGHOSTUserResponse(InsertEQP_IPPCMAPPINGHOSTUserResponseBody Body)
	{
		this.Body = Body;
	}
}
