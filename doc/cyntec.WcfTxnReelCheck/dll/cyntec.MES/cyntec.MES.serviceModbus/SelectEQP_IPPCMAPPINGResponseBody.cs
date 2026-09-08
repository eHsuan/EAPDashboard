using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class SelectEQP_IPPCMAPPINGResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string SelectEQP_IPPCMAPPINGResult;

	public SelectEQP_IPPCMAPPINGResponseBody()
	{
	}

	public SelectEQP_IPPCMAPPINGResponseBody(string SelectEQP_IPPCMAPPINGResult)
	{
		this.SelectEQP_IPPCMAPPINGResult = SelectEQP_IPPCMAPPINGResult;
	}
}
