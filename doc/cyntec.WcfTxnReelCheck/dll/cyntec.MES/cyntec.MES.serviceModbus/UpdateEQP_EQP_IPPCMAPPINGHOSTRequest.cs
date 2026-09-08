using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class UpdateEQP_EQP_IPPCMAPPINGHOSTRequest
{
	[MessageBodyMember(Name = "UpdateEQP_EQP_IPPCMAPPINGHOST", Namespace = "CyntecMES", Order = 0)]
	public UpdateEQP_EQP_IPPCMAPPINGHOSTRequestBody Body;

	public UpdateEQP_EQP_IPPCMAPPINGHOSTRequest()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGHOSTRequest(UpdateEQP_EQP_IPPCMAPPINGHOSTRequestBody Body)
	{
		this.Body = Body;
	}
}
