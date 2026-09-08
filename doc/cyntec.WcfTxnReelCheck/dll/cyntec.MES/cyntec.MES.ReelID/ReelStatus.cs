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
public class ReelStatus : INotifyPropertyChanged
{
	private string codeField;

	private string codeNameField;

	private string codeTypeField;

	[XmlElement(Order = 0)]
	public string Code
	{
		get
		{
			return codeField;
		}
		set
		{
			codeField = value;
			RaisePropertyChanged("Code");
		}
	}

	[XmlElement(Order = 1)]
	public string CodeName
	{
		get
		{
			return codeNameField;
		}
		set
		{
			codeNameField = value;
			RaisePropertyChanged("CodeName");
		}
	}

	[XmlElement(Order = 2)]
	public string CodeType
	{
		get
		{
			return codeTypeField;
		}
		set
		{
			codeTypeField = value;
			RaisePropertyChanged("CodeType");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
