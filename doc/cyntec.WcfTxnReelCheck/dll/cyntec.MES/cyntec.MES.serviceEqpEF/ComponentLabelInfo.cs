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
public class ComponentLabelInfo : INotifyPropertyChanged
{
	private string serialNoField;

	private string labelContent_QRField;

	private string labelContent1Field;

	private string labelContent2Field;

	private string labelContent3Field;

	private string labelContent4Field;

	private string labelContent5Field;

	private string labelContent6Field;

	[XmlElement(Order = 0)]
	public string SerialNo
	{
		get
		{
			return serialNoField;
		}
		set
		{
			serialNoField = value;
			RaisePropertyChanged("SerialNo");
		}
	}

	[XmlElement(Order = 1)]
	public string LabelContent_QR
	{
		get
		{
			return labelContent_QRField;
		}
		set
		{
			labelContent_QRField = value;
			RaisePropertyChanged("LabelContent_QR");
		}
	}

	[XmlElement(Order = 2)]
	public string LabelContent1
	{
		get
		{
			return labelContent1Field;
		}
		set
		{
			labelContent1Field = value;
			RaisePropertyChanged("LabelContent1");
		}
	}

	[XmlElement(Order = 3)]
	public string LabelContent2
	{
		get
		{
			return labelContent2Field;
		}
		set
		{
			labelContent2Field = value;
			RaisePropertyChanged("LabelContent2");
		}
	}

	[XmlElement(Order = 4)]
	public string LabelContent3
	{
		get
		{
			return labelContent3Field;
		}
		set
		{
			labelContent3Field = value;
			RaisePropertyChanged("LabelContent3");
		}
	}

	[XmlElement(Order = 5)]
	public string LabelContent4
	{
		get
		{
			return labelContent4Field;
		}
		set
		{
			labelContent4Field = value;
			RaisePropertyChanged("LabelContent4");
		}
	}

	[XmlElement(Order = 6)]
	public string LabelContent5
	{
		get
		{
			return labelContent5Field;
		}
		set
		{
			labelContent5Field = value;
			RaisePropertyChanged("LabelContent5");
		}
	}

	[XmlElement(Order = 7)]
	public string LabelContent6
	{
		get
		{
			return labelContent6Field;
		}
		set
		{
			labelContent6Field = value;
			RaisePropertyChanged("LabelContent6");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
