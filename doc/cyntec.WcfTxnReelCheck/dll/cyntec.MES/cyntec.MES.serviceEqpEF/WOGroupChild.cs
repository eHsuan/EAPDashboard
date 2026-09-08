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
public class WOGroupChild : INotifyPropertyChanged
{
	private int _IndexField;

	private string _ParentWoNoField;

	private string _ChildWoNoField;

	[XmlElement(Order = 0)]
	public int _Index
	{
		get
		{
			return _IndexField;
		}
		set
		{
			_IndexField = value;
			RaisePropertyChanged("_Index");
		}
	}

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
	public string _ChildWoNo
	{
		get
		{
			return _ChildWoNoField;
		}
		set
		{
			_ChildWoNoField = value;
			RaisePropertyChanged("_ChildWoNo");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
