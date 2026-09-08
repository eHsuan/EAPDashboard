using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class InsertEQP_EQP_IPPCMAPPINGAPIListResponse
{
	[MessageBodyMember(Name = "InsertEQP_EQP_IPPCMAPPINGAPIListResponse", Namespace = "CyntecMES", Order = 0)]
	public InsertEQP_EQP_IPPCMAPPINGAPIListResponseBody Body;

	public InsertEQP_EQP_IPPCMAPPINGAPIListResponse()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGAPIListResponse(InsertEQP_EQP_IPPCMAPPINGAPIListResponseBody Body)
	{
		this.Body = Body;
	}
}
