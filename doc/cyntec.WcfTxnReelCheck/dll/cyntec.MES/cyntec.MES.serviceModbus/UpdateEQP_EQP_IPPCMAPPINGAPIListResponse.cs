using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class UpdateEQP_EQP_IPPCMAPPINGAPIListResponse
{
	[MessageBodyMember(Name = "UpdateEQP_EQP_IPPCMAPPINGAPIListResponse", Namespace = "CyntecMES", Order = 0)]
	public UpdateEQP_EQP_IPPCMAPPINGAPIListResponseBody Body;

	public UpdateEQP_EQP_IPPCMAPPINGAPIListResponse()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGAPIListResponse(UpdateEQP_EQP_IPPCMAPPINGAPIListResponseBody Body)
	{
		this.Body = Body;
	}
}
