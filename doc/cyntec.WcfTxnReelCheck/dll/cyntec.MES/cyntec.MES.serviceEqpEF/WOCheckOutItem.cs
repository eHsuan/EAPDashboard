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
public class WOCheckOutItem : Basic_Result
{
	private string _EqpNoField;

	private string _WONOField;

	private string _UserIDField;

	private double _OutputQtyField;

	private double _NGQtyField;

	private string _RemarkField;

	private string _RefWOField;

	private string _RefWOGroupIDField;

	private double _RefOutputQtyField;

	private NGItem[] _WONGListField;

	private WOInputItem[] _WOInputItemListField;

	[XmlElement(Order = 0)]
	public string _EqpNo
	{
		get
		{
			return _EqpNoField;
		}
		set
		{
			_EqpNoField = value;
			RaisePropertyChanged("_EqpNo");
		}
	}

	[XmlElement(Order = 1)]
	public string _WONO
	{
		get
		{
			return _WONOField;
		}
		set
		{
			_WONOField = value;
			RaisePropertyChanged("_WONO");
		}
	}

	[XmlElement(Order = 2)]
	public string _UserID
	{
		get
		{
			return _UserIDField;
		}
		set
		{
			_UserIDField = value;
			RaisePropertyChanged("_UserID");
		}
	}

	[XmlElement(Order = 3)]
	public double _OutputQty
	{
		get
		{
			return _OutputQtyField;
		}
		set
		{
			_OutputQtyField = value;
			RaisePropertyChanged("_OutputQty");
		}
	}

	[XmlElement(Order = 4)]
	public double _NGQty
	{
		get
		{
			return _NGQtyField;
		}
		set
		{
			_NGQtyField = value;
			RaisePropertyChanged("_NGQty");
		}
	}

	[XmlElement(Order = 5)]
	public string _Remark
	{
		get
		{
			return _RemarkField;
		}
		set
		{
			_RemarkField = value;
			RaisePropertyChanged("_Remark");
		}
	}

	[XmlElement(Order = 6)]
	public string _RefWO
	{
		get
		{
			return _RefWOField;
		}
		set
		{
			_RefWOField = value;
			RaisePropertyChanged("_RefWO");
		}
	}

	[XmlElement(Order = 7)]
	public string _RefWOGroupID
	{
		get
		{
			return _RefWOGroupIDField;
		}
		set
		{
			_RefWOGroupIDField = value;
			RaisePropertyChanged("_RefWOGroupID");
		}
	}

	[XmlElement(Order = 8)]
	public double _RefOutputQty
	{
		get
		{
			return _RefOutputQtyField;
		}
		set
		{
			_RefOutputQtyField = value;
			RaisePropertyChanged("_RefOutputQty");
		}
	}

	[XmlArray(Order = 9)]
	public NGItem[] _WONGList
	{
		get
		{
			return _WONGListField;
		}
		set
		{
			_WONGListField = value;
			RaisePropertyChanged("_WONGList");
		}
	}

	[XmlArray(Order = 10)]
	public WOInputItem[] _WOInputItemList
	{
		get
		{
			return _WOInputItemListField;
		}
		set
		{
			_WOInputItemListField = value;
			RaisePropertyChanged("_WOInputItemList");
		}
	}
}
