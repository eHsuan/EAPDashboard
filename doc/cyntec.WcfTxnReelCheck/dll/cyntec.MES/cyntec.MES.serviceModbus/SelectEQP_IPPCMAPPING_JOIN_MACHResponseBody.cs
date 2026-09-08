using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class SelectEQP_IPPCMAPPING_JOIN_MACHResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string SelectEQP_IPPCMAPPING_JOIN_MACHResult;

	public SelectEQP_IPPCMAPPING_JOIN_MACHResponseBody()
	{
	}

	public SelectEQP_IPPCMAPPING_JOIN_MACHResponseBody(string SelectEQP_IPPCMAPPING_JOIN_MACHResult)
	{
		this.SelectEQP_IPPCMAPPING_JOIN_MACHResult = SelectEQP_IPPCMAPPING_JOIN_MACHResult;
	}
}
