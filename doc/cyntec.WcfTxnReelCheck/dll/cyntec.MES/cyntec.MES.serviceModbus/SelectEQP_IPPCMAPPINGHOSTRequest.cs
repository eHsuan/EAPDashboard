using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_IPPCMAPPINGHOSTRequest
{
	[MessageBodyMember(Name = "SelectEQP_IPPCMAPPINGHOST", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_IPPCMAPPINGHOSTRequestBody Body;

	public SelectEQP_IPPCMAPPINGHOSTRequest()
	{
	}

	public SelectEQP_IPPCMAPPINGHOSTRequest(SelectEQP_IPPCMAPPINGHOSTRequestBody Body)
	{
		this.Body = Body;
	}
}
