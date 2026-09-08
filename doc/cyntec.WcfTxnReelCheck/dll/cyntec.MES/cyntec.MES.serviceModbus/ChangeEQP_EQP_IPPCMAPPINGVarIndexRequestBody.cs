using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class ChangeEQP_EQP_IPPCMAPPINGVarIndexRequestBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string pParameter;

	public ChangeEQP_EQP_IPPCMAPPINGVarIndexRequestBody()
	{
	}

	public ChangeEQP_EQP_IPPCMAPPINGVarIndexRequestBody(string pParameter)
	{
		this.pParameter = pParameter;
	}
}
