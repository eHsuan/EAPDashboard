using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class SelectEQP_EQP_IPPCMAPPINGAPIList_PResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string SelectEQP_EQP_IPPCMAPPINGAPIList_PResult;

	public SelectEQP_EQP_IPPCMAPPINGAPIList_PResponseBody()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGAPIList_PResponseBody(string SelectEQP_EQP_IPPCMAPPINGAPIList_PResult)
	{
		this.SelectEQP_EQP_IPPCMAPPINGAPIList_PResult = SelectEQP_EQP_IPPCMAPPINGAPIList_PResult;
	}
}
