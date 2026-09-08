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
public class Result_WOGroup : Basic_Result
{
	private string[] _pWoNoListField;

	private WOGroupParent[] _WOGroupItemListField;

	[XmlArray(Order = 0)]
	public string[] _pWoNoList
	{
		get
		{
			return _pWoNoListField;
		}
		set
		{
			_pWoNoListField = value;
			RaisePropertyChanged("_pWoNoList");
		}
	}

	[XmlArray(Order = 1)]
	public WOGroupParent[] _WOGroupItemList
	{
		get
		{
			return _WOGroupItemListField;
		}
		set
		{
			_WOGroupItemListField = value;
			RaisePropertyChanged("_WOGroupItemList");
		}
	}
}
