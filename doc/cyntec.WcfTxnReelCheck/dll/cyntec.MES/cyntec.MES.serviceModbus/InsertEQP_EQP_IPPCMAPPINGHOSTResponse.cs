using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class InsertEQP_EQP_IPPCMAPPINGHOSTResponse
{
	[MessageBodyMember(Name = "InsertEQP_EQP_IPPCMAPPINGHOSTResponse", Namespace = "CyntecMES", Order = 0)]
	public InsertEQP_EQP_IPPCMAPPINGHOSTResponseBody Body;

	public InsertEQP_EQP_IPPCMAPPINGHOSTResponse()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGHOSTResponse(InsertEQP_EQP_IPPCMAPPINGHOSTResponseBody Body)
	{
		this.Body = Body;
	}
}
