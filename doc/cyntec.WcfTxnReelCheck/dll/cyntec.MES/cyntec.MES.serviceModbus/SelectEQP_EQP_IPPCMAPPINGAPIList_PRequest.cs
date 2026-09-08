using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_EQP_IPPCMAPPINGAPIList_PRequest
{
	[MessageBodyMember(Name = "SelectEQP_EQP_IPPCMAPPINGAPIList_P", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_EQP_IPPCMAPPINGAPIList_PRequestBody Body;

	public SelectEQP_EQP_IPPCMAPPINGAPIList_PRequest()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGAPIList_PRequest(SelectEQP_EQP_IPPCMAPPINGAPIList_PRequestBody Body)
	{
		this.Body = Body;
	}
}
