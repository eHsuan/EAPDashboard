using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class InsertEQP_EQP_IPPCMAPPINGRequest
{
	[MessageBodyMember(Name = "InsertEQP_EQP_IPPCMAPPING", Namespace = "CyntecMES", Order = 0)]
	public InsertEQP_EQP_IPPCMAPPINGRequestBody Body;

	public InsertEQP_EQP_IPPCMAPPINGRequest()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGRequest(InsertEQP_EQP_IPPCMAPPINGRequestBody Body)
	{
		this.Body = Body;
	}
}
