using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class UpdateEQP_EQP_IPPCMAPPINGAPIListRequestBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string pParameter;

	public UpdateEQP_EQP_IPPCMAPPINGAPIListRequestBody()
	{
	}

	public UpdateEQP_EQP_IPPCMAPPINGAPIListRequestBody(string pParameter)
	{
		this.pParameter = pParameter;
	}
}
