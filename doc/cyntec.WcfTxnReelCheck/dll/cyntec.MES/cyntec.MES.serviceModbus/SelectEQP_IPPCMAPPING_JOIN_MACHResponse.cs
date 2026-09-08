using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SelectEQP_IPPCMAPPING_JOIN_MACHResponse
{
	[MessageBodyMember(Name = "SelectEQP_IPPCMAPPING_JOIN_MACHResponse", Namespace = "CyntecMES", Order = 0)]
	public SelectEQP_IPPCMAPPING_JOIN_MACHResponseBody Body;

	public SelectEQP_IPPCMAPPING_JOIN_MACHResponse()
	{
	}

	public SelectEQP_IPPCMAPPING_JOIN_MACHResponse(SelectEQP_IPPCMAPPING_JOIN_MACHResponseBody Body)
	{
		this.Body = Body;
	}
}
