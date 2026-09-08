using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_EQP_IPPCMAPPINGAPIList_PResponse
{
	[MessageBodyMember(Name = "SelectEQP_EQP_IPPCMAPPINGAPIList_PResponse", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_EQP_IPPCMAPPINGAPIList_PResponseBody Body;

	public SelectEQP_EQP_IPPCMAPPINGAPIList_PResponse()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGAPIList_PResponse(SelectEQP_EQP_IPPCMAPPINGAPIList_PResponseBody Body)
	{
		this.Body = Body;
	}
}
