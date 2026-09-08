using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class SelectEQP_IPPCMAPPINGHOSTResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string SelectEQP_IPPCMAPPINGHOSTResult;

	public SelectEQP_IPPCMAPPINGHOSTResponseBody()
	{
	}

	public SelectEQP_IPPCMAPPINGHOSTResponseBody(string SelectEQP_IPPCMAPPINGHOSTResult)
	{
		this.SelectEQP_IPPCMAPPINGHOSTResult = SelectEQP_IPPCMAPPINGHOSTResult;
	}
}
