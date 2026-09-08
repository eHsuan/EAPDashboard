using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MES_ParamItem", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class MES_ParamItem : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string EnableField;

	[OptionalField]
	private string IDField;

	[OptionalField]
	private bool IsSystemField;

	[OptionalField]
	private bool IsWoParamField;

	[OptionalField]
	private string LOLimitModeField;

	[OptionalField]
	private string LowerLimitField;

	[OptionalField]
	private string MatNoField;

	[OptionalField]
	private string ParamCodeField;

	[OptionalField]
	private string ParamNameField;

	[OptionalField]
	private string ParamValueField;

	[OptionalField]
	private string RefFieldCodeField;

	[OptionalField]
	private string RemarkField;

	[OptionalField]
	private string SourceTypeField;

	[OptionalField]
	private string UPLimitModeField;

	[OptionalField]
	private string UnitField;

	[OptionalField]
	private string UpperLimitField;

	[OptionalField]
	private string WorkCenterNoField;

	[Browsable(false)]
	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public string Enable
	{
		get
		{
			return EnableField;
		}
		set
		{
			if ((object)EnableField != value)
			{
				EnableField = value;
				RaisePropertyChanged("Enable");
			}
		}
	}

	[DataMember]
	public string ID
	{
		get
		{
			return IDField;
		}
		set
		{
			if ((object)IDField != value)
			{
				IDField = value;
				RaisePropertyChanged("ID");
			}
		}
	}

	[DataMember]
	public bool IsSystem
	{
		get
		{
			return IsSystemField;
		}
		set
		{
			if (!IsSystemField.Equals(value))
			{
				IsSystemField = value;
				RaisePropertyChanged("IsSystem");
			}
		}
	}

	[DataMember]
	public bool IsWoParam
	{
		get
		{
			return IsWoParamField;
		}
		set
		{
			if (!IsWoParamField.Equals(value))
			{
				IsWoParamField = value;
				RaisePropertyChanged("IsWoParam");
			}
		}
	}

	[DataMember]
	public string LOLimitMode
	{
		get
		{
			return LOLimitModeField;
		}
		set
		{
			if ((object)LOLimitModeField != value)
			{
				LOLimitModeField = value;
				RaisePropertyChanged("LOLimitMode");
			}
		}
	}

	[DataMember]
	public string LowerLimit
	{
		get
		{
			return LowerLimitField;
		}
		set
		{
			if ((object)LowerLimitField != value)
			{
				LowerLimitField = value;
				RaisePropertyChanged("LowerLimit");
			}
		}
	}

	[DataMember]
	public string MatNo
	{
		get
		{
			return MatNoField;
		}
		set
		{
			if ((object)MatNoField != value)
			{
				MatNoField = value;
				RaisePropertyChanged("MatNo");
			}
		}
	}

	[DataMember]
	public string ParamCode
	{
		get
		{
			return ParamCodeField;
		}
		set
		{
			if ((object)ParamCodeField != value)
			{
				ParamCodeField = value;
				RaisePropertyChanged("ParamCode");
			}
		}
	}

	[DataMember]
	public string ParamName
	{
		get
		{
			return ParamNameField;
		}
		set
		{
			if ((object)ParamNameField != value)
			{
				ParamNameField = value;
				RaisePropertyChanged("ParamName");
			}
		}
	}

	[DataMember]
	public string ParamValue
	{
		get
		{
			return ParamValueField;
		}
		set
		{
			if ((object)ParamValueField != value)
			{
				ParamValueField = value;
				RaisePropertyChanged("ParamValue");
			}
		}
	}

	[DataMember]
	public string RefFieldCode
	{
		get
		{
			return RefFieldCodeField;
		}
		set
		{
			if ((object)RefFieldCodeField != value)
			{
				RefFieldCodeField = value;
				RaisePropertyChanged("RefFieldCode");
			}
		}
	}

	[DataMember]
	public string Remark
	{
		get
		{
			return RemarkField;
		}
		set
		{
			if ((object)RemarkField != value)
			{
				RemarkField = value;
				RaisePropertyChanged("Remark");
			}
		}
	}

	[DataMember]
	public string SourceType
	{
		get
		{
			return SourceTypeField;
		}
		set
		{
			if ((object)SourceTypeField != value)
			{
				SourceTypeField = value;
				RaisePropertyChanged("SourceType");
			}
		}
	}

	[DataMember]
	public string UPLimitMode
	{
		get
		{
			return UPLimitModeField;
		}
		set
		{
			if ((object)UPLimitModeField != value)
			{
				UPLimitModeField = value;
				RaisePropertyChanged("UPLimitMode");
			}
		}
	}

	[DataMember]
	public string Unit
	{
		get
		{
			return UnitField;
		}
		set
		{
			if ((object)UnitField != value)
			{
				UnitField = value;
				RaisePropertyChanged("Unit");
			}
		}
	}

	[DataMember]
	public string UpperLimit
	{
		get
		{
			return UpperLimitField;
		}
		set
		{
			if ((object)UpperLimitField != value)
			{
				UpperLimitField = value;
				RaisePropertyChanged("UpperLimit");
			}
		}
	}

	[DataMember]
	public string WorkCenterNo
	{
		get
		{
			return WorkCenterNoField;
		}
		set
		{
			if ((object)WorkCenterNoField != value)
			{
				WorkCenterNoField = value;
				RaisePropertyChanged("WorkCenterNo");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
