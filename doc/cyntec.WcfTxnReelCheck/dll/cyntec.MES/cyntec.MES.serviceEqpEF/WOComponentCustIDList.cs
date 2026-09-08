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
public class WOComponentCustIDList : INotifyPropertyChanged
{
	private string componentNoField;

	private string wOComponentCustIDField;

	private string custTypeField;

	private bool resultField;

	private string messageField;

	private string oldWOComponentCustIDField;

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
	public string WOComponentCustID
	{
		get
		{
			return wOComponentCustIDField;
		}
		set
		{
			wOComponentCustIDField = value;
			RaisePropertyChanged("WOComponentCustID");
		}
	}

	[XmlElement(Order = 2)]
	public string CustType
	{
		get
		{
			return custTypeField;
		}
		set
		{
			custTypeField = value;
			RaisePropertyChanged("CustType");
		}
	}

	[XmlElement(Order = 3)]
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
	public string OldWOComponentCustID
	{
		get
		{
			return oldWOComponentCustIDField;
		}
		set
		{
			oldWOComponentCustIDField = value;
			RaisePropertyChanged("OldWOComponentCustID");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
