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
public class Result_GetDataTable_EF : Basic_Result
{
	private DataTable _resultDtField;

	private int _resultCountField;

	[XmlElement(Order = 0)]
	public DataTable _resultDt
	{
		get
		{
			return _resultDtField;
		}
		set
		{
			_resultDtField = value;
			RaisePropertyChanged("_resultDt");
		}
	}

	[XmlElement(Order = 1)]
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
}
