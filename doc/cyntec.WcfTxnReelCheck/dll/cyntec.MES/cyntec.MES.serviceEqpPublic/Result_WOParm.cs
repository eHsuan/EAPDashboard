using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpPublic;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "CyntecMES")]
public class Result_WOParm : Basic_Result
{
	private Param[] _modelListField;

	private DataSet _resultDsField;

	[XmlArray(Order = 0)]
	public Param[] _modelList
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

	[XmlElement(Order = 1)]
	public DataSet _resultDs
	{
		get
		{
			return _resultDsField;
		}
		set
		{
			_resultDsField = value;
			RaisePropertyChanged("_resultDs");
		}
	}
}
