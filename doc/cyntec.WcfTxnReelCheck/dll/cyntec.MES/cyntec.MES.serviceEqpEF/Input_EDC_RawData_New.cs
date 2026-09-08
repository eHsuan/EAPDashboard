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
public class Input_EDC_RawData_New : INotifyPropertyChanged
{
	private string componentNoField;

	private string fieldCodeField;

	private string valueField;

	private string memoField;

	private int sortIndexField;

	[XmlElement(Order = 0)]
	public string componentNo
	{
		get
		{
			return componentNoField;
		}
		set
		{
			componentNoField = value;
			RaisePropertyChanged("componentNo");
		}
	}

	[XmlElement(Order = 1)]
	public string fieldCode
	{
		get
		{
			return fieldCodeField;
		}
		set
		{
			fieldCodeField = value;
			RaisePropertyChanged("fieldCode");
		}
	}

	[XmlElement(Order = 2)]
	public string value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
			RaisePropertyChanged("value");
		}
	}

	[XmlElement(Order = 3)]
	public string memo
	{
		get
		{
			return memoField;
		}
		set
		{
			memoField = value;
			RaisePropertyChanged("memo");
		}
	}

	[XmlElement(Order = 4)]
	public int sortIndex
	{
		get
		{
			return sortIndexField;
		}
		set
		{
			sortIndexField = value;
			RaisePropertyChanged("sortIndex");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
