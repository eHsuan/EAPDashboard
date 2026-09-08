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
public class Result_WOCheckOut_Batch : Basic_Result
{
	private WOCheckOutItem[] _WOCheckOutItemListField;

	[XmlArray(Order = 0)]
	public WOCheckOutItem[] _WOCheckOutItemList
	{
		get
		{
			return _WOCheckOutItemListField;
		}
		set
		{
			_WOCheckOutItemListField = value;
			RaisePropertyChanged("_WOCheckOutItemList");
		}
	}
}
