using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_IPPCMAPPINGResponse
{
	[MessageBodyMember(Name = "SelectEQP_IPPCMAPPINGResponse", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_IPPCMAPPINGResponseBody Body;

	public SelectEQP_IPPCMAPPINGResponse()
	{
	}

	public SelectEQP_IPPCMAPPINGResponse(SelectEQP_IPPCMAPPINGResponseBody Body)
	{
		this.Body = Body;
	}
}
