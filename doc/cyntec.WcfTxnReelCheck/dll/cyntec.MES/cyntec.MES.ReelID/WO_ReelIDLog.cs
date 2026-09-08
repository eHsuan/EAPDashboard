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
public class WO_ReelIDLog : INotifyPropertyChanged
{
	private string parent_ReelIDField;

	private string reelIDField;

	private string sMatNoField;

	private string sBatchNoField;

	private string matNoField;

	private string batchNoField;

	private string big_ReelIDField;

	private string wONOField;

	private string statusField;

	private string actionField;

	private string dNDOCField;

	private string dNITEMField;

	private string mMDOCField;

	private string mMITEMField;

	private bool isSAPLogField;

	private string userIDField;

	private DateTime createDateField;

	private string createUserField;

	private int confirmIDField;

	[XmlElement(Order = 0)]
	public string Parent_ReelID
	{
		get
		{
			return parent_ReelIDField;
		}
		set
		{
			parent_ReelIDField = value;
			RaisePropertyChanged("Parent_ReelID");
		}
	}

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
	public string SMatNo
	{
		get
		{
			return sMatNoField;
		}
		set
		{
			sMatNoField = value;
			RaisePropertyChanged("SMatNo");
		}
	}

	[XmlElement(Order = 3)]
	public string SBatchNo
	{
		get
		{
			return sBatchNoField;
		}
		set
		{
			sBatchNoField = value;
			RaisePropertyChanged("SBatchNo");
		}
	}

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
	public string Big_ReelID
	{
		get
		{
			return big_ReelIDField;
		}
		set
		{
			big_ReelIDField = value;
			RaisePropertyChanged("Big_ReelID");
		}
	}

	[XmlElement(Order = 7)]
	public string WONO
	{
		get
		{
			return wONOField;
		}
		set
		{
			wONOField = value;
			RaisePropertyChanged("WONO");
		}
	}

	[XmlElement(Order = 8)]
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

	[XmlElement(Order = 9)]
	public string Action
	{
		get
		{
			return actionField;
		}
		set
		{
			actionField = value;
			RaisePropertyChanged("Action");
		}
	}

	[XmlElement(Order = 10)]
	public string DNDOC
	{
		get
		{
			return dNDOCField;
		}
		set
		{
			dNDOCField = value;
			RaisePropertyChanged("DNDOC");
		}
	}

	[XmlElement(Order = 11)]
	public string DNITEM
	{
		get
		{
			return dNITEMField;
		}
		set
		{
			dNITEMField = value;
			RaisePropertyChanged("DNITEM");
		}
	}

	[XmlElement(Order = 12)]
	public string MMDOC
	{
		get
		{
			return mMDOCField;
		}
		set
		{
			mMDOCField = value;
			RaisePropertyChanged("MMDOC");
		}
	}

	[XmlElement(Order = 13)]
	public string MMITEM
	{
		get
		{
			return mMITEMField;
		}
		set
		{
			mMITEMField = value;
			RaisePropertyChanged("MMITEM");
		}
	}

	[XmlElement(Order = 14)]
	public bool IsSAPLog
	{
		get
		{
			return isSAPLogField;
		}
		set
		{
			isSAPLogField = value;
			RaisePropertyChanged("IsSAPLog");
		}
	}

	[XmlElement(Order = 15)]
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

	[XmlElement(Order = 16)]
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

	[XmlElement(Order = 17)]
	public string CreateUser
	{
		get
		{
			return createUserField;
		}
		set
		{
			createUserField = value;
			RaisePropertyChanged("CreateUser");
		}
	}

	[XmlElement(Order = 18)]
	public int ConfirmID
	{
		get
		{
			return confirmIDField;
		}
		set
		{
			confirmIDField = value;
			RaisePropertyChanged("ConfirmID");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
