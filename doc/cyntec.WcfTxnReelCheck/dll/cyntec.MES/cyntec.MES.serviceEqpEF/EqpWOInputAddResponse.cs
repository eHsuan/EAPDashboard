using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceEqpEF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(WrapperName = "EqpWOInputAddResponse", WrapperNamespace = "http://CyntecMES", IsWrapped = true)]
public class EqpWOInputAddResponse
{
	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 0)]
	public Basic_Result EqpWOInputAddResult;

	public EqpWOInputAddResponse()
	{
	}

	public EqpWOInputAddResponse(Basic_Result EqpWOInputAddResult)
	{
		this.EqpWOInputAddResult = EqpWOInputAddResult;
	}
}
