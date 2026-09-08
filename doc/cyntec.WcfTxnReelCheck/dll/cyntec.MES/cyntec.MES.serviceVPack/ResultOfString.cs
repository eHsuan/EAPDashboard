using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceVPack;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://tempuri.org/")]
public class ResultOfString : INotifyPropertyChanged
{
	private string codeField;

	private string msgField;

	private string dataField;

	[XmlElement(Order = 0)]
	public string code
	{
		get
		{
			return codeField;
		}
		set
		{
			codeField = value;
			RaisePropertyChanged("code");
		}
	}

	[XmlElement(Order = 1)]
	public string msg
	{
		get
		{
			return msgField;
		}
		set
		{
			msgField = value;
			RaisePropertyChanged("msg");
		}
	}

	[XmlElement(Order = 2)]
	public string data
	{
		get
		{
			return dataField;
		}
		set
		{
			dataField = value;
			RaisePropertyChanged("data");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
