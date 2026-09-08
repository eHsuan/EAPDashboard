using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.WoParam;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "http://172.28.113.107:8085/websrv/")]
public class GetListByMatNoRequestBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string WONo;

	[DataMember(EmitDefaultValue = false, Order = 1)]
	public string WorkCenterNo;

	[DataMember(EmitDefaultValue = false, Order = 2)]
	public string MatNo;

	public GetListByMatNoRequestBody()
	{
	}

	public GetListByMatNoRequestBody(string WONo, string WorkCenterNo, string MatNo)
	{
		this.WONo = WONo;
		this.WorkCenterNo = WorkCenterNo;
		this.MatNo = MatNo;
	}
}
