using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_IPPCMAPPINGHOSTResponse
{
	[MessageBodyMember(Name = "SelectEQP_IPPCMAPPINGHOSTResponse", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_IPPCMAPPINGHOSTResponseBody Body;

	public SelectEQP_IPPCMAPPINGHOSTResponse()
	{
	}

	public SelectEQP_IPPCMAPPINGHOSTResponse(SelectEQP_IPPCMAPPINGHOSTResponseBody Body)
	{
		this.Body = Body;
	}
}
