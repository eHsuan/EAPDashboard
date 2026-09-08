using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_IPPCMAPPINGRequest
{
	[MessageBodyMember(Name = "SelectEQP_IPPCMAPPING", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_IPPCMAPPINGRequestBody Body;

	public SelectEQP_IPPCMAPPINGRequest()
	{
	}

	public SelectEQP_IPPCMAPPINGRequest(SelectEQP_IPPCMAPPINGRequestBody Body)
	{
		this.Body = Body;
	}
}
