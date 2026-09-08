using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class InsertEQP_EQP_IPPCMAPPINGResponse
{
	[MessageBodyMember(Name = "InsertEQP_EQP_IPPCMAPPINGResponse", Namespace = "CyntecMES", Order = 0)]
	public InsertEQP_EQP_IPPCMAPPINGResponseBody Body;

	public InsertEQP_EQP_IPPCMAPPINGResponse()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGResponse(InsertEQP_EQP_IPPCMAPPINGResponseBody Body)
	{
		this.Body = Body;
	}
}
