using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "CyntecMES")]
public class SelectEQP_EQP_IPPCMAPPINGVar_SingleRequestBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string pLineSysID;

	[DataMember(EmitDefaultValue = false, Order = 1)]
	public string pSysID;

	[DataMember(EmitDefaultValue = false, Order = 2)]
	public string pUserID;

	public SelectEQP_EQP_IPPCMAPPINGVar_SingleRequestBody()
	{
	}

	public SelectEQP_EQP_IPPCMAPPINGVar_SingleRequestBody(string pLineSysID, string pSysID, string pUserID)
	{
		this.pLineSysID = pLineSysID;
		this.pSysID = pSysID;
		this.pUserID = pUserID;
	}
}
