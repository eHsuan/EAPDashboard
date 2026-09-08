using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class SelectEQP_IPPCMAPPING_JOIN_MACHRequestBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string pParameter;

	public SelectEQP_IPPCMAPPING_JOIN_MACHRequestBody()
	{
	}

	public SelectEQP_IPPCMAPPING_JOIN_MACHRequestBody(string pParameter)
	{
		this.pParameter = pParameter;
	}
}
