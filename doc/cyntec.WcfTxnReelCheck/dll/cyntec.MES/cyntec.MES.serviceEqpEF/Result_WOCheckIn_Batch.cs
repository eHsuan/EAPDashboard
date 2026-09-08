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
public class Result_WOCheckIn_Batch : Basic_Result
{
	private WOCheckItem[] _pWOCheckItemListField;

	[XmlArray(Order = 0)]
	public WOCheckItem[] _pWOCheckItemList
	{
		get
		{
			return _pWOCheckItemListField;
		}
		set
		{
			_pWOCheckItemListField = value;
			RaisePropertyChanged("_pWOCheckItemList");
		}
	}
}
