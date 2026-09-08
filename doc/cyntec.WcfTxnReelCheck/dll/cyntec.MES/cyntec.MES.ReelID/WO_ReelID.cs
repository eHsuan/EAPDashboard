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
public class WO_ReelID : INotifyPropertyChanged
{
	private string reelIDField;

	private string areaField;

	private string wONoField;

	private string matNoField;

	private string batchNoField;

	private string sernoField;

	private decimal qtyField;

	private string rePrtField;

	private string sLabelField;

	private string statusField;

	private string refReelIDField;

	private string remarkField;

	private DateTime createDateField;

	private string userIDField;

	private string pcField;

	private string sAPStatusField;

	private string sAPRemarkField;

	private string tPStatusField;

	private string tPRemarkField;

	private string tPMachNoField;

	private DateTime? tPStartTimeField;

	private DateTime? tPEndTimeField;

	private string tCStatusField;

	private string tCRemarkField;

	private string fQCStatusField;

	private string fQCRemarkField;

	private string updateUserField;

	private DateTime updateDateField;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
	public string Area
	{
		get
		{
			return areaField;
		}
		set
		{
			areaField = value;
			RaisePropertyChanged("Area");
		}
	}

	[XmlElement(Order = 2)]
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

	[XmlElement(Order = 3)]
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
	public string Serno
	{
		get
		{
			return sernoField;
		}
		set
		{
			sernoField = value;
			RaisePropertyChanged("Serno");
		}
	}

	[XmlElement(Order = 6)]
	public decimal Qty
	{
		get
		{
			return qtyField;
		}
		set
		{
			qtyField = value;
			RaisePropertyChanged("Qty");
		}
	}

	[XmlElement(Order = 7)]
	public string RePrt
	{
		get
		{
			return rePrtField;
		}
		set
		{
			rePrtField = value;
			RaisePropertyChanged("RePrt");
		}
	}

	[XmlElement(Order = 8)]
	public string SLabel
	{
		get
		{
			return sLabelField;
		}
		set
		{
			sLabelField = value;
			RaisePropertyChanged("SLabel");
		}
	}

	[XmlElement(Order = 9)]
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

	[XmlElement(Order = 10)]
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

	[XmlElement(Order = 11)]
	public string Remark
	{
		get
		{
			return remarkField;
		}
		set
		{
			remarkField = value;
			RaisePropertyChanged("Remark");
		}
	}

	[XmlElement(Order = 12)]
	public DateTime CreateDate
	{
		get
		{
			return createDateField;
		}
		set
		{
			createDateField = value;
			RaisePropertyChanged("CreateDate");
		}
	}

	[XmlElement(Order = 13)]
	public string UserID
	{
		get
		{
			return userIDField;
		}
		set
		{
			userIDField = value;
			RaisePropertyChanged("UserID");
		}
	}

	[XmlElement(Order = 14)]
	public string PC
	{
		get
		{
			return pcField;
		}
		set
		{
			pcField = value;
			RaisePropertyChanged("PC");
		}
	}

	[XmlElement(Order = 15)]
	public string SAPStatus
	{
		get
		{
			return sAPStatusField;
		}
		set
		{
			sAPStatusField = value;
			RaisePropertyChanged("SAPStatus");
		}
	}

	[XmlElement(Order = 16)]
	public string SAPRemark
	{
		get
		{
			return sAPRemarkField;
		}
		set
		{
			sAPRemarkField = value;
			RaisePropertyChanged("SAPRemark");
		}
	}

	[XmlElement(Order = 17)]
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

	[XmlElement(Order = 18)]
	public string TPRemark
	{
		get
		{
			return tPRemarkField;
		}
		set
		{
			tPRemarkField = value;
			RaisePropertyChanged("TPRemark");
		}
	}

	[XmlElement(Order = 19)]
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

	[XmlElement(IsNullable = true, Order = 20)]
	public DateTime? TPStartTime
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

	[XmlElement(IsNullable = true, Order = 21)]
	public DateTime? TPEndTime
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

	[XmlElement(Order = 22)]
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

	[XmlElement(Order = 23)]
	public string TCRemark
	{
		get
		{
			return tCRemarkField;
		}
		set
		{
			tCRemarkField = value;
			RaisePropertyChanged("TCRemark");
		}
	}

	[XmlElement(Order = 24)]
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

	[XmlElement(Order = 25)]
	public string FQCRemark
	{
		get
		{
			return fQCRemarkField;
		}
		set
		{
			fQCRemarkField = value;
			RaisePropertyChanged("FQCRemark");
		}
	}

	[XmlElement(Order = 26)]
	public string UpdateUser
	{
		get
		{
			return updateUserField;
		}
		set
		{
			updateUserField = value;
			RaisePropertyChanged("UpdateUser");
		}
	}

	[XmlElement(Order = 27)]
	public DateTime UpdateDate
	{
		get
		{
			return updateDateField;
		}
		set
		{
			updateDateField = value;
			RaisePropertyChanged("UpdateDate");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
