using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace cyntec.MES.serviceEqpEF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(WrapperName = "UploadTestResultFileResponse", WrapperNamespace = "http://CyntecMES", IsWrapped = true)]
public class UploadTestResultFileResponse
{
	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 0)]
	public Basic_Result UploadTestResultFileResult;

	public UploadTestResultFileResponse()
	{
	}

	public UploadTestResultFileResponse(Basic_Result UploadTestResultFileResult)
	{
		this.UploadTestResultFileResult = UploadTestResultFileResult;
	}
}
