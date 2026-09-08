using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpEF;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://CyntecMES")]
public class Result_WOCheckOutCreateSPCData : Basic_Result
{
	private DataSet _dicSPCDataField;

	[XmlElement(Order = 0)]
	public DataSet _dicSPCData
	{
		get
		{
			return _dicSPCDataField;
		}
		set
		{
			_dicSPCDataField = value;
			RaisePropertyChanged("_dicSPCData");
		}
	}
}
