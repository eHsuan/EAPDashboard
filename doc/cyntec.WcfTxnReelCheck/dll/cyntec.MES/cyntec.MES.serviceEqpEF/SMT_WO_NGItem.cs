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
public class SMT_WO_NGItem : INotifyPropertyChanged
{
	private string _NGCodeField;

	private decimal _QTYField;

	private SMT_NGItem[] _ComponentNGListField;

	[XmlElement(Order = 0)]
	public string _NGCode
	{
		get
		{
			return _NGCodeField;
		}
		set
		{
			_NGCodeField = value;
			RaisePropertyChanged("_NGCode");
		}
	}

	[XmlElement(Order = 1)]
	public decimal _QTY
	{
		get
		{
			return _QTYField;
		}
		set
		{
			_QTYField = value;
			RaisePropertyChanged("_QTY");
		}
	}

	[XmlArray(Order = 2)]
	public SMT_NGItem[] _ComponentNGList
	{
		get
		{
			return _ComponentNGListField;
		}
		set
		{
			_ComponentNGListField = value;
			RaisePropertyChanged("_ComponentNGList");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
