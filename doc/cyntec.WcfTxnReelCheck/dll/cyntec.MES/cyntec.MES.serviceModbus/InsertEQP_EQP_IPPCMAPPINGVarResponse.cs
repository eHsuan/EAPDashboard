using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class InsertEQP_EQP_IPPCMAPPINGVarResponse
{
	[MessageBodyMember(Name = "InsertEQP_EQP_IPPCMAPPINGVarResponse", Namespace = "CyntecMES", Order = 0)]
	public InsertEQP_EQP_IPPCMAPPINGVarResponseBody Body;

	public InsertEQP_EQP_IPPCMAPPINGVarResponse()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGVarResponse(InsertEQP_EQP_IPPCMAPPINGVarResponseBody Body)
	{
		this.Body = Body;
	}
}
