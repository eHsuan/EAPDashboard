using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.WoParam;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "http://172.28.113.107:8085/websrv/")]
public class GetListByMatNoResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public Param[] GetListByMatNoResult;

	public GetListByMatNoResponseBody()
	{
	}

	public GetListByMatNoResponseBody(Param[] GetListByMatNoResult)
	{
		this.GetListByMatNoResult = GetListByMatNoResult;
	}
}
