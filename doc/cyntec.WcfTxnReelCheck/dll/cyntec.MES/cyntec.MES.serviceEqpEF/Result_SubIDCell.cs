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
public class Result_SubIDCell : INotifyPropertyChanged
{
	private string requesterField;

	private string subIDKeyField;

	private string subIDValueField;

	[XmlElement(Order = 0)]
	public string Requester
	{
		get
		{
			return requesterField;
		}
		set
		{
			requesterField = value;
			RaisePropertyChanged("Requester");
		}
	}

	[XmlElement(Order = 1)]
	public string SubIDKey
	{
		get
		{
			return subIDKeyField;
		}
		set
		{
			subIDKeyField = value;
			RaisePropertyChanged("SubIDKey");
		}
	}

	[XmlElement(Order = 2)]
	public string SubIDValue
	{
		get
		{
			return subIDValueField;
		}
		set
		{
			subIDValueField = value;
			RaisePropertyChanged("SubIDValue");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
