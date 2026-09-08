using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_IPPCMAPPING_JOIN_MACHRequest
{
	[MessageBodyMember(Name = "SelectEQP_IPPCMAPPING_JOIN_MACH", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_IPPCMAPPING_JOIN_MACHRequestBody Body;

	public SelectEQP_IPPCMAPPING_JOIN_MACHRequest()
	{
	}

	public SelectEQP_IPPCMAPPING_JOIN_MACHRequest(SelectEQP_IPPCMAPPING_JOIN_MACHRequestBody Body)
	{
		this.Body = Body;
	}
}
