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
public class WOCheckItem : Basic_Result
{
	private string _EqpNoField;

	private string _WONOField;

	private string _UserIDField;

	private string _RefWOField;

	private string _RefWOGroupIDField;

	private bool _IsJustCheckField;

	private bool _IsCheckEmpCertField;

	private MatItem[] _MatItemListField;

	private WOCarrierItem[] _WOCarrierListField;

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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
	public bool _IsJustCheck
	{
		get
		{
			return _IsJustCheckField;
		}
		set
		{
			_IsJustCheckField = value;
			RaisePropertyChanged("_IsJustCheck");
		}
	}

	[XmlElement(Order = 6)]
	public bool _IsCheckEmpCert
	{
		get
		{
			return _IsCheckEmpCertField;
		}
		set
		{
			_IsCheckEmpCertField = value;
			RaisePropertyChanged("_IsCheckEmpCert");
		}
	}

	[XmlArray(Order = 7)]
	public MatItem[] _MatItemList
	{
		get
		{
			return _MatItemListField;
		}
		set
		{
			_MatItemListField = value;
			RaisePropertyChanged("_MatItemList");
		}
	}

	[XmlArray(Order = 8)]
	public WOCarrierItem[] _WOCarrierList
	{
		get
		{
			return _WOCarrierListField;
		}
		set
		{
			_WOCarrierListField = value;
			RaisePropertyChanged("_WOCarrierList");
		}
	}

	[XmlArray(Order = 9)]
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
