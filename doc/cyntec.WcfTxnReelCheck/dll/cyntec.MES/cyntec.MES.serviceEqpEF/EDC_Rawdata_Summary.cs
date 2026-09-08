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
public class EDC_Rawdata_Summary : INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private DateTime changeDateField;

	private string changeUserField;

	private string commentsField;

	private int componentQtyField;

	private DateTime createDateField;

	private string createUserField;

	private string dataSourceField;

	private string edcConfirmSysIDField;

	private string edcPlanSysIDField;

	private string edcRawdataSummarySysIDField;

	private string eqpNoField;

	private string errorMessageField;

	private int flowIDField;

	private string holdReasonField;

	private bool isAvailableField;

	private bool isNgField;

	private bool isReworkRetestField;

	private string matGroupNoField;

	private int ngProcessRuleField;

	private string planNameField;

	private int planSequenceNoField;

	private int ponintQtyField;

	private string reworkReasonField;

	private string woNoField;

	private string workCenterNameField;

	private string workCenterNoField;

	[XmlElement(Order = 0)]
	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
			RaisePropertyChanged("ExtensionData");
		}
	}

	[XmlElement(Order = 1)]
	public DateTime ChangeDate
	{
		get
		{
			return changeDateField;
		}
		set
		{
			changeDateField = value;
			RaisePropertyChanged("ChangeDate");
		}
	}

	[XmlElement(Order = 2)]
	public string ChangeUser
	{
		get
		{
			return changeUserField;
		}
		set
		{
			changeUserField = value;
			RaisePropertyChanged("ChangeUser");
		}
	}

	[XmlElement(Order = 3)]
	public string Comments
	{
		get
		{
			return commentsField;
		}
		set
		{
			commentsField = value;
			RaisePropertyChanged("Comments");
		}
	}

	[XmlElement(Order = 4)]
	public int ComponentQty
	{
		get
		{
			return componentQtyField;
		}
		set
		{
			componentQtyField = value;
			RaisePropertyChanged("ComponentQty");
		}
	}

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
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

	[XmlElement(Order = 7)]
	public string DataSource
	{
		get
		{
			return dataSourceField;
		}
		set
		{
			dataSourceField = value;
			RaisePropertyChanged("DataSource");
		}
	}

	[XmlElement(Order = 8)]
	public string EdcConfirmSysID
	{
		get
		{
			return edcConfirmSysIDField;
		}
		set
		{
			edcConfirmSysIDField = value;
			RaisePropertyChanged("EdcConfirmSysID");
		}
	}

	[XmlElement(Order = 9)]
	public string EdcPlanSysID
	{
		get
		{
			return edcPlanSysIDField;
		}
		set
		{
			edcPlanSysIDField = value;
			RaisePropertyChanged("EdcPlanSysID");
		}
	}

	[XmlElement(Order = 10)]
	public string EdcRawdataSummarySysID
	{
		get
		{
			return edcRawdataSummarySysIDField;
		}
		set
		{
			edcRawdataSummarySysIDField = value;
			RaisePropertyChanged("EdcRawdataSummarySysID");
		}
	}

	[XmlElement(Order = 11)]
	public string EqpNo
	{
		get
		{
			return eqpNoField;
		}
		set
		{
			eqpNoField = value;
			RaisePropertyChanged("EqpNo");
		}
	}

	[XmlElement(Order = 12)]
	public string ErrorMessage
	{
		get
		{
			return errorMessageField;
		}
		set
		{
			errorMessageField = value;
			RaisePropertyChanged("ErrorMessage");
		}
	}

	[XmlElement(Order = 13)]
	public int FlowID
	{
		get
		{
			return flowIDField;
		}
		set
		{
			flowIDField = value;
			RaisePropertyChanged("FlowID");
		}
	}

	[XmlElement(Order = 14)]
	public string HoldReason
	{
		get
		{
			return holdReasonField;
		}
		set
		{
			holdReasonField = value;
			RaisePropertyChanged("HoldReason");
		}
	}

	[XmlElement(Order = 15)]
	public bool IsAvailable
	{
		get
		{
			return isAvailableField;
		}
		set
		{
			isAvailableField = value;
			RaisePropertyChanged("IsAvailable");
		}
	}

	[XmlElement(Order = 16)]
	public bool IsNg
	{
		get
		{
			return isNgField;
		}
		set
		{
			isNgField = value;
			RaisePropertyChanged("IsNg");
		}
	}

	[XmlElement(Order = 17)]
	public bool IsReworkRetest
	{
		get
		{
			return isReworkRetestField;
		}
		set
		{
			isReworkRetestField = value;
			RaisePropertyChanged("IsReworkRetest");
		}
	}

	[XmlElement(Order = 18)]
	public string MatGroupNo
	{
		get
		{
			return matGroupNoField;
		}
		set
		{
			matGroupNoField = value;
			RaisePropertyChanged("MatGroupNo");
		}
	}

	[XmlElement(Order = 19)]
	public int NgProcessRule
	{
		get
		{
			return ngProcessRuleField;
		}
		set
		{
			ngProcessRuleField = value;
			RaisePropertyChanged("NgProcessRule");
		}
	}

	[XmlElement(Order = 20)]
	public string PlanName
	{
		get
		{
			return planNameField;
		}
		set
		{
			planNameField = value;
			RaisePropertyChanged("PlanName");
		}
	}

	[XmlElement(Order = 21)]
	public int PlanSequenceNo
	{
		get
		{
			return planSequenceNoField;
		}
		set
		{
			planSequenceNoField = value;
			RaisePropertyChanged("PlanSequenceNo");
		}
	}

	[XmlElement(Order = 22)]
	public int PonintQty
	{
		get
		{
			return ponintQtyField;
		}
		set
		{
			ponintQtyField = value;
			RaisePropertyChanged("PonintQty");
		}
	}

	[XmlElement(Order = 23)]
	public string ReworkReason
	{
		get
		{
			return reworkReasonField;
		}
		set
		{
			reworkReasonField = value;
			RaisePropertyChanged("ReworkReason");
		}
	}

	[XmlElement(Order = 24)]
	public string WoNo
	{
		get
		{
			return woNoField;
		}
		set
		{
			woNoField = value;
			RaisePropertyChanged("WoNo");
		}
	}

	[XmlElement(Order = 25)]
	public string WorkCenterName
	{
		get
		{
			return workCenterNameField;
		}
		set
		{
			workCenterNameField = value;
			RaisePropertyChanged("WorkCenterName");
		}
	}

	[XmlElement(Order = 26)]
	public string WorkCenterNo
	{
		get
		{
			return workCenterNoField;
		}
		set
		{
			workCenterNoField = value;
			RaisePropertyChanged("WorkCenterNo");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
