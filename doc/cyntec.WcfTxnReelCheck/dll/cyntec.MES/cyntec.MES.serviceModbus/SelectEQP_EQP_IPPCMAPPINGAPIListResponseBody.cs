using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class SelectEQP_EQP_IPPCMAPPINGAPIListResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string SelectEQP_EQP_IPPCMAPPINGAPIListResult;

	public SelectEQP_EQP_IPPCMAPPINGAPIListResponseBody()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGAPIListResponseBody(string SelectEQP_EQP_IPPCMAPPINGAPIListResult)
	{
		this.SelectEQP_EQP_IPPCMAPPINGAPIListResult = SelectEQP_EQP_IPPCMAPPINGAPIListResult;
	}
}
