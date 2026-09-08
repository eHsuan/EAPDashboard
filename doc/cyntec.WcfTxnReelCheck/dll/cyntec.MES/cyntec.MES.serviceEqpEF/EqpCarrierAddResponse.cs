using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceEqpEF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(WrapperName = "EqpCarrierAddResponse", WrapperNamespace = "http://CyntecMES", IsWrapped = true)]
public class EqpCarrierAddResponse
{
	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 0)]
	public Basic_Result EqpCarrierAddResult;

	public EqpCarrierAddResponse()
	{
	}

	public EqpCarrierAddResponse(Basic_Result EqpCarrierAddResult)
	{
		this.EqpCarrierAddResult = EqpCarrierAddResult;
	}
}
