using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class UpdateEQP_EQP_IPPCMAPPINGAPIListRequest
{
	[MessageBodyMember(Name = "UpdateEQP_EQP_IPPCMAPPINGAPIList", Namespace = "CyntecMES", Order = 0)]
	public UpdateEQP_EQP_IPPCMAPPINGAPIListRequestBody Body;

	public UpdateEQP_EQP_IPPCMAPPINGAPIListRequest()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGAPIListRequest(UpdateEQP_EQP_IPPCMAPPINGAPIListRequestBody Body)
	{
		this.Body = Body;
	}
}
