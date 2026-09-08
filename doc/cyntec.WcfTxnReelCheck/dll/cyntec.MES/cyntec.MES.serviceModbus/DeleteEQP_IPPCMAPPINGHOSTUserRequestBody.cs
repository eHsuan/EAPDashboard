using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class DeleteEQP_IPPCMAPPINGHOSTUserRequestBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string pParameter;

	public DeleteEQP_IPPCMAPPINGHOSTUserRequestBody()
	{
	}

	public DeleteEQP_IPPCMAPPINGHOSTUserRequestBody(string pParameter)
	{
		this.pParameter = pParameter;
	}
}
