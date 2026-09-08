using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.WoParam;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "http://172.28.113.107:8085/websrv/")]
public class GetListByWorkCenterRequestBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string WONo;

	[DataMember(EmitDefaultValue = false, Order = 1)]
	public string WorkCenterNo;

	public GetListByWorkCenterRequestBody()
	{
	}

	public GetListByWorkCenterRequestBody(string WONo, string WorkCenterNo)
	{
		this.WONo = WONo;
		this.WorkCenterNo = WorkCenterNo;
	}
}
