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
public class CompLabelCheckInfo : INotifyPropertyChanged
{
	private int totalField;

	private int unCheckField;

	private int checkedField;

	private int normalField;

	private int ngField;

	private int lessField;

	private int markUpField;

	[XmlElement(Order = 0)]
	public int Total
	{
		get
		{
			return totalField;
		}
		set
		{
			totalField = value;
			RaisePropertyChanged("Total");
		}
	}

	[XmlElement(Order = 1)]
	public int UnCheck
	{
		get
		{
			return unCheckField;
		}
		set
		{
			unCheckField = value;
			RaisePropertyChanged("UnCheck");
		}
	}

	[XmlElement(Order = 2)]
	public int Checked
	{
		get
		{
			return checkedField;
		}
		set
		{
			checkedField = value;
			RaisePropertyChanged("Checked");
		}
	}

	[XmlElement(Order = 3)]
	public int Normal
	{
		get
		{
			return normalField;
		}
		set
		{
			normalField = value;
			RaisePropertyChanged("Normal");
		}
	}

	[XmlElement(Order = 4)]
	public int NG
	{
		get
		{
			return ngField;
		}
		set
		{
			ngField = value;
			RaisePropertyChanged("NG");
		}
	}

	[XmlElement(Order = 5)]
	public int Less
	{
		get
		{
			return lessField;
		}
		set
		{
			lessField = value;
			RaisePropertyChanged("Less");
		}
	}

	[XmlElement(Order = 6)]
	public int MarkUp
	{
		get
		{
			return markUpField;
		}
		set
		{
			markUpField = value;
			RaisePropertyChanged("MarkUp");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
