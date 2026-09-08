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
public class Result_EdcDataQuery : Basic_Result
{
	private EDC_Rawdata[] _RawdataListField;

	private string _WoNoField;

	[XmlArray(Order = 0)]
	public EDC_Rawdata[] _RawdataList
	{
		get
		{
			return _RawdataListField;
		}
		set
		{
			_RawdataListField = value;
			RaisePropertyChanged("_RawdataList");
		}
	}

	[XmlElement(Order = 1)]
	public string _WoNo
	{
		get
		{
			return _WoNoField;
		}
		set
		{
			_WoNoField = value;
			RaisePropertyChanged("_WoNo");
		}
	}
}
