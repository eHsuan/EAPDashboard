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
public class ReeIDInfo : INotifyPropertyChanged
{
	private string matNoField;

	private string batchNoField;

	private string reelIDField;

	[XmlElement(Order = 0)]
	public string MatNo
	{
		get
		{
			return matNoField;
		}
		set
		{
			matNoField = value;
			RaisePropertyChanged("MatNo");
		}
	}

	[XmlElement(Order = 1)]
	public string BatchNo
	{
		get
		{
			return batchNoField;
		}
		set
		{
			batchNoField = value;
			RaisePropertyChanged("BatchNo");
		}
	}

	[XmlElement(Order = 2)]
	public string ReelID
	{
		get
		{
			return reelIDField;
		}
		set
		{
			reelIDField = value;
			RaisePropertyChanged("ReelID");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
