using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class UpdateEQP_EQP_IPPCMAPPINGRequest
{
	[MessageBodyMember(Name = "UpdateEQP_EQP_IPPCMAPPING", Namespace = "CyntecMES", Order = 0)]
	public UpdateEQP_EQP_IPPCMAPPINGRequestBody Body;

	public UpdateEQP_EQP_IPPCMAPPINGRequest()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGRequest(UpdateEQP_EQP_IPPCMAPPINGRequestBody Body)
	{
		this.Body = Body;
	}
}
