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
public class EDC_Confirm : INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private string commentsField;

	private string confirmDateField;

	private string dataCollectionDateField;

	private string edcConfirmSysIDField;

	private string edcPlanSysIdField;

	private string edcRawdataSummarySysIDField;

	private string eqpNoField;

	private string errorMessageField;

	private int flowIDField;

	private string holdReasonField;

	private bool isHoldYetField;

	private bool isMainMatFinsihField;

	private bool isNGField;

	private bool isReworkRetestField;

	private bool isSubMatFinsihField;

	private int ngProcessRuleField;

	private string planNameField;

	private int planSequenceNoField;

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

	[XmlElement(Order = 2)]
	public string ConfirmDate
	{
		get
		{
			return confirmDateField;
		}
		set
		{
			confirmDateField = value;
			RaisePropertyChanged("ConfirmDate");
		}
	}

	[XmlElement(Order = 3)]
	public string DataCollectionDate
	{
		get
		{
			return dataCollectionDateField;
		}
		set
		{
			dataCollectionDateField = value;
			RaisePropertyChanged("DataCollectionDate");
		}
	}

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
	public string EdcPlanSysId
	{
		get
		{
			return edcPlanSysIdField;
		}
		set
		{
			edcPlanSysIdField = value;
			RaisePropertyChanged("EdcPlanSysId");
		}
	}

	[XmlElement(Order = 6)]
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

	[XmlElement(Order = 7)]
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

	[XmlElement(Order = 8)]
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

	[XmlElement(Order = 9)]
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

	[XmlElement(Order = 10)]
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

	[XmlElement(Order = 11)]
	public bool IsHoldYet
	{
		get
		{
			return isHoldYetField;
		}
		set
		{
			isHoldYetField = value;
			RaisePropertyChanged("IsHoldYet");
		}
	}

	[XmlElement(Order = 12)]
	public bool IsMainMatFinsih
	{
		get
		{
			return isMainMatFinsihField;
		}
		set
		{
			isMainMatFinsihField = value;
			RaisePropertyChanged("IsMainMatFinsih");
		}
	}

	[XmlElement(Order = 13)]
	public bool IsNG
	{
		get
		{
			return isNGField;
		}
		set
		{
			isNGField = value;
			RaisePropertyChanged("IsNG");
		}
	}

	[XmlElement(Order = 14)]
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

	[XmlElement(Order = 15)]
	public bool IsSubMatFinsih
	{
		get
		{
			return isSubMatFinsihField;
		}
		set
		{
			isSubMatFinsihField = value;
			RaisePropertyChanged("IsSubMatFinsih");
		}
	}

	[XmlElement(Order = 16)]
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

	[XmlElement(Order = 17)]
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

	[XmlElement(Order = 18)]
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

	[XmlElement(Order = 19)]
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

	[XmlElement(Order = 20)]
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

	[XmlElement(Order = 21)]
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
