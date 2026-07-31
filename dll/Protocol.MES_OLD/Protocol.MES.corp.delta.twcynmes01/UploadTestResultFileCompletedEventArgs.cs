using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace Protocol.MES.corp.delta.twcynmes01;

[GeneratedCode("System.Web.Services", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class UploadTestResultFileCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public string Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[0];
		}
	}

	internal UploadTestResultFileCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
