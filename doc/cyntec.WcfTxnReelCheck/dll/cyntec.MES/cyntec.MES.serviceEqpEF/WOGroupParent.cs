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
public class WOGroupParent : INotifyPropertyChanged
{
	private string _ParentWoNoField;

	private bool _IsWoGroupLotField;

	private WOGroupChild[] _ChildWoNoListField;

	[XmlElement(Order = 0)]
	public string _ParentWoNo
	{
		get
		{
			return _ParentWoNoField;
		}
		set
		{
			_ParentWoNoField = value;
			RaisePropertyChanged("_ParentWoNo");
		}
	}

	[XmlElement(Order = 1)]
	public bool _IsWoGroupLot
	{
		get
		{
			return _IsWoGroupLotField;
		}
		set
		{
			_IsWoGroupLotField = value;
			RaisePropertyChanged("_IsWoGroupLot");
		}
	}

	[XmlArray(Order = 2)]
	public WOGroupChild[] _ChildWoNoList
	{
		get
		{
			return _ChildWoNoListField;
		}
		set
		{
			_ChildWoNoListField = value;
			RaisePropertyChanged("_ChildWoNoList");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
