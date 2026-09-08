using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceEqpEF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(WrapperName = "EqpWOParmValueAddResponse", WrapperNamespace = "http://CyntecMES", IsWrapped = true)]
public class EqpWOParmValueAddResponse
{
	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 0)]
	public Basic_Result EqpWOParmValueAddResult;

	public EqpWOParmValueAddResponse()
	{
	}

	public EqpWOParmValueAddResponse(Basic_Result EqpWOParmValueAddResult)
	{
		this.EqpWOParmValueAddResult = EqpWOParmValueAddResult;
	}
}
