using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class InsertEQP_EQP_IPPCMAPPINGVarRequest
{
	[MessageBodyMember(Name = "InsertEQP_EQP_IPPCMAPPINGVar", Namespace = "CyntecMES", Order = 0)]
	public InsertEQP_EQP_IPPCMAPPINGVarRequestBody Body;

	public InsertEQP_EQP_IPPCMAPPINGVarRequest()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGVarRequest(InsertEQP_EQP_IPPCMAPPINGVarRequestBody Body)
	{
		this.Body = Body;
	}
}
