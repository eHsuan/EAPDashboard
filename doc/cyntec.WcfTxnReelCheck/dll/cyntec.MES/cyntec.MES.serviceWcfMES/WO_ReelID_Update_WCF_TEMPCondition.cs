using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_ReelID_Update_WCF_TEMPCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_ReelID_Update_WCF_TEMPCondition : Condition
{
	[OptionalField]
	private string reelIDField;

	[OptionalField]
	private string statusCodeField;

	[OptionalField]
	private string statusRemarkField;

	[OptionalField]
	private string machNoField;

	[OptionalField]
	private string startTimeField;

	[OptionalField]
	private string endTimeField;

	[OptionalField]
	private string UpdateUserField;

	[DataMember]
	public string reelID
	{
		get
		{
			return reelIDField;
		}
		set
		{
			if ((object)reelIDField != value)
			{
				reelIDField = value;
				RaisePropertyChanged("reelID");
			}
		}
	}

	[DataMember]
	public string statusCode
	{
		get
		{
			return statusCodeField;
		}
		set
		{
			if ((object)statusCodeField != value)
			{
				statusCodeField = value;
				RaisePropertyChanged("statusCode");
			}
		}
	}

	[DataMember]
	public string statusRemark
	{
		get
		{
			return statusRemarkField;
		}
		set
		{
			if ((object)statusRemarkField != value)
			{
				statusRemarkField = value;
				RaisePropertyChanged("statusRemark");
			}
		}
	}

	[DataMember(Order = 3)]
	public string machNo
	{
		get
		{
			return machNoField;
		}
		set
		{
			if ((object)machNoField != value)
			{
				machNoField = value;
				RaisePropertyChanged("machNo");
			}
		}
	}

	[DataMember(Order = 4)]
	public string startTime
	{
		get
		{
			return startTimeField;
		}
		set
		{
			if ((object)startTimeField != value)
			{
				startTimeField = value;
				RaisePropertyChanged("startTime");
			}
		}
	}

	[DataMember(Order = 5)]
	public string endTime
	{
		get
		{
			return endTimeField;
		}
		set
		{
			if ((object)endTimeField != value)
			{
				endTimeField = value;
				RaisePropertyChanged("endTime");
			}
		}
	}

	[DataMember(Order = 6)]
	public string UpdateUser
	{
		get
		{
			return UpdateUserField;
		}
		set
		{
			if ((object)UpdateUserField != value)
			{
				UpdateUserField = value;
				RaisePropertyChanged("UpdateUser");
			}
		}
	}
}
