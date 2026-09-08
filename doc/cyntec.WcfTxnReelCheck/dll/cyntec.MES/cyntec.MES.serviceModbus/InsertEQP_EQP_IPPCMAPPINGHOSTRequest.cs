using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class InsertEQP_EQP_IPPCMAPPINGHOSTRequest
{
	[MessageBodyMember(Name = "InsertEQP_EQP_IPPCMAPPINGHOST", Namespace = "CyntecMES", Order = 0)]
	public InsertEQP_EQP_IPPCMAPPINGHOSTRequestBody Body;

	public InsertEQP_EQP_IPPCMAPPINGHOSTRequest()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGHOSTRequest(InsertEQP_EQP_IPPCMAPPINGHOSTRequestBody Body)
	{
		this.Body = Body;
	}
}
