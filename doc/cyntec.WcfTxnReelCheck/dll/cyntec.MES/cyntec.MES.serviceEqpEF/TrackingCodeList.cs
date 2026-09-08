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
public class TrackingCodeList : INotifyPropertyChanged
{
	private string componentNoField;

	private string trackingCodeField;

	private bool resultField;

	private string messageField;

	private string oldTrackingCodeField;

	[XmlElement(Order = 0)]
	public string ComponentNo
	{
		get
		{
			return componentNoField;
		}
		set
		{
			componentNoField = value;
			RaisePropertyChanged("ComponentNo");
		}
	}

	[XmlElement(Order = 1)]
	public string TrackingCode
	{
		get
		{
			return trackingCodeField;
		}
		set
		{
			trackingCodeField = value;
			RaisePropertyChanged("TrackingCode");
		}
	}

	[XmlElement(Order = 2)]
	public bool Result
	{
		get
		{
			return resultField;
		}
		set
		{
			resultField = value;
			RaisePropertyChanged("Result");
		}
	}

	[XmlElement(Order = 3)]
	public string Message
	{
		get
		{
			return messageField;
		}
		set
		{
			messageField = value;
			RaisePropertyChanged("Message");
		}
	}

	[XmlElement(Order = 4)]
	public string OldTrackingCode
	{
		get
		{
			return oldTrackingCodeField;
		}
		set
		{
			oldTrackingCodeField = value;
			RaisePropertyChanged("OldTrackingCode");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
