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
public class Result_EqpDynamicNGRecord_GetModelList : Basic_Result
{
	private int _resultCountField;

	private EqpDynamicNGRecord[] _modelListField;

	[XmlElement(Order = 0)]
	public int _resultCount
	{
		get
		{
			return _resultCountField;
		}
		set
		{
			_resultCountField = value;
			RaisePropertyChanged("_resultCount");
		}
	}

	[XmlArray(Order = 1)]
	public EqpDynamicNGRecord[] _modelList
	{
		get
		{
			return _modelListField;
		}
		set
		{
			_modelListField = value;
			RaisePropertyChanged("_modelList");
		}
	}
}
