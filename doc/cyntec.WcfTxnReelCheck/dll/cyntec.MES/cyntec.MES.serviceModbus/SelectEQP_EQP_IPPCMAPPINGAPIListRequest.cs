using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_EQP_IPPCMAPPINGAPIListRequest
{
	[MessageBodyMember(Name = "SelectEQP_EQP_IPPCMAPPINGAPIList", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_EQP_IPPCMAPPINGAPIListRequestBody Body;

	public SelectEQP_EQP_IPPCMAPPINGAPIListRequest()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGAPIListRequest(SelectEQP_EQP_IPPCMAPPINGAPIListRequestBody Body)
	{
		this.Body = Body;
	}
}
