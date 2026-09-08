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
public class ReelIDSet : INotifyPropertyChanged
{
	private string wONoField;

	private string matNoField;

	private string batchNoField;

	private string reelIDField;

	private string refReelIDField;

	private string statusField;

	private string tPStatusField;

	private string tPStartTimeField;

	private string tPEndTimeField;

	private string tPMachNoField;

	private string tCStatusField;

	private string fQCStatusField;

	[XmlElement(Order = 0)]
	public string WONo
	{
		get
		{
			return wONoField;
		}
		set
		{
			wONoField = value;
			RaisePropertyChanged("WONo");
		}
	}

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
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

	[XmlElement(Order = 3)]
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

	[XmlElement(Order = 4)]
	public string RefReelID
	{
		get
		{
			return refReelIDField;
		}
		set
		{
			refReelIDField = value;
			RaisePropertyChanged("RefReelID");
		}
	}

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
	public string TPStatus
	{
		get
		{
			return tPStatusField;
		}
		set
		{
			tPStatusField = value;
			RaisePropertyChanged("TPStatus");
		}
	}

	[XmlElement(Order = 7)]
	public string TPStartTime
	{
		get
		{
			return tPStartTimeField;
		}
		set
		{
			tPStartTimeField = value;
			RaisePropertyChanged("TPStartTime");
		}
	}

	[XmlElement(Order = 8)]
	public string TPEndTime
	{
		get
		{
			return tPEndTimeField;
		}
		set
		{
			tPEndTimeField = value;
			RaisePropertyChanged("TPEndTime");
		}
	}

	[XmlElement(Order = 9)]
	public string TPMachNo
	{
		get
		{
			return tPMachNoField;
		}
		set
		{
			tPMachNoField = value;
			RaisePropertyChanged("TPMachNo");
		}
	}

	[XmlElement(Order = 10)]
	public string TCStatus
	{
		get
		{
			return tCStatusField;
		}
		set
		{
			tCStatusField = value;
			RaisePropertyChanged("TCStatus");
		}
	}

	[XmlElement(Order = 11)]
	public string FQCStatus
	{
		get
		{
			return fQCStatusField;
		}
		set
		{
			fQCStatusField = value;
			RaisePropertyChanged("FQCStatus");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
