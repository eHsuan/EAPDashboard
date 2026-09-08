using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.ReelID;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://cncynmes/")]
public class ReturnMsg : INotifyPropertyChanged
{
	private string statusField;

	private string msgCodeField;

	private string msgField;

	[XmlElement(Order = 0)]
	public string Status
	{
		get
		{
			return statusField;
		}
		set
		{
			statusField = value;
			RaisePropertyChanged("Status");
		}
	}

	[XmlElement(Order = 1)]
	public string MsgCode
	{
		get
		{
			return msgCodeField;
		}
		set
		{
			msgCodeField = value;
			RaisePropertyChanged("MsgCode");
		}
	}

	[XmlElement(Order = 2)]
	public string Msg
	{
		get
		{
			return msgField;
		}
		set
		{
			msgField = value;
			RaisePropertyChanged("Msg");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
