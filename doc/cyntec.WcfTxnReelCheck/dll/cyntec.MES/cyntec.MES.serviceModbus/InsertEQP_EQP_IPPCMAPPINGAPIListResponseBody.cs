using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class InsertEQP_EQP_IPPCMAPPINGAPIListResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string InsertEQP_EQP_IPPCMAPPINGAPIListResult;

	public InsertEQP_EQP_IPPCMAPPINGAPIListResponseBody()
	{
	}

	public InsertEQP_EQP_IPPCMAPPINGAPIListResponseBody(string InsertEQP_EQP_IPPCMAPPINGAPIListResult)
	{
		this.InsertEQP_EQP_IPPCMAPPINGAPIListResult = InsertEQP_EQP_IPPCMAPPINGAPIListResult;
	}
}
