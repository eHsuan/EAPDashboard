using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpEF;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://CyntecMES")]
public class Result_EdcRawdataGet : Basic_Result
{
	private Result_EdcRawdataCell[] _edcdataListField;

	[XmlArray(Order = 0)]
	public Result_EdcRawdataCell[] _edcdataList
	{
		get
		{
			return _edcdataListField;
		}
		set
		{
			_edcdataListField = value;
			RaisePropertyChanged("_edcdataList");
		}
	}
}
