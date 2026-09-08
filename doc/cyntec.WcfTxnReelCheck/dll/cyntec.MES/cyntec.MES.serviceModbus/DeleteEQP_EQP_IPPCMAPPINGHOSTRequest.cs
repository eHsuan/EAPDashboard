using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class DeleteEQP_EQP_IPPCMAPPINGHOSTRequest
{
	[MessageBodyMember(Name = "DeleteEQP_EQP_IPPCMAPPINGHOST", Namespace = "CyntecMES", Order = 0)]
	public DeleteEQP_EQP_IPPCMAPPINGHOSTRequestBody Body;

	public DeleteEQP_EQP_IPPCMAPPINGHOSTRequest()
	{
	}

	public DeleteEQP_EQP_IPPCMAPPINGHOSTRequest(DeleteEQP_EQP_IPPCMAPPINGHOSTRequestBody Body)
	{
		this.Body = Body;
	}
}
