using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.WoParam;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Param", Namespace = "http://172.28.113.107:8085/websrv/")]
public class Param : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	private int? IDField;

	[OptionalField]
	private string WONoField;

	[OptionalField]
	private string WorkCenterNoField;

	[OptionalField]
	private string SourceTypeField;

	[OptionalField]
	private string SourceNoField;

	[OptionalField]
	private string ParamCodeField;

	[OptionalField]
	private string ParamNameField;

	[OptionalField]
	private string RefFieldCodeField;

	[OptionalField]
	private string ParamValueField;

	[OptionalField]
	private string UpperValueField;

	[OptionalField]
	private string LowerValueField;

	[OptionalField]
	private string UnitField;

	[OptionalField]
	private string ReferDocNoField;

	[OptionalField]
	private string ModifyByField;

	[OptionalField]
	private string CreateUserField;

	private DateTime? CreateDateField;

	[OptionalField]
	private string ReleaseUserField;

	private DateTime? ReleaseDateField;

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

	[DataMember(IsRequired = true)]
	public int? ID
	{
		get
		{
			return IDField;
		}
		set
		{
			if (!IDField.Equals(value))
			{
				IDField = value;
				RaisePropertyChanged("ID");
			}
		}
	}

	[DataMember(EmitDefaultValue = false)]
	public string WONo
	{
		get
		{
			return WONoField;
		}
		set
		{
			if ((object)WONoField != value)
			{
				WONoField = value;
				RaisePropertyChanged("WONo");
			}
		}
	}

	[DataMember(EmitDefaultValue = false)]
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

	[DataMember(EmitDefaultValue = false, Order = 3)]
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

	[DataMember(EmitDefaultValue = false, Order = 4)]
	public string SourceNo
	{
		get
		{
			return SourceNoField;
		}
		set
		{
			if ((object)SourceNoField != value)
			{
				SourceNoField = value;
				RaisePropertyChanged("SourceNo");
			}
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 5)]
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

	[DataMember(EmitDefaultValue = false, Order = 6)]
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

	[DataMember(EmitDefaultValue = false, Order = 7)]
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

	[DataMember(EmitDefaultValue = false, Order = 8)]
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

	[DataMember(EmitDefaultValue = false, Order = 9)]
	public string UpperValue
	{
		get
		{
			return UpperValueField;
		}
		set
		{
			if ((object)UpperValueField != value)
			{
				UpperValueField = value;
				RaisePropertyChanged("UpperValue");
			}
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 10)]
	public string LowerValue
	{
		get
		{
			return LowerValueField;
		}
		set
		{
			if ((object)LowerValueField != value)
			{
				LowerValueField = value;
				RaisePropertyChanged("LowerValue");
			}
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 11)]
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

	[DataMember(EmitDefaultValue = false, Order = 12)]
	public string ReferDocNo
	{
		get
		{
			return ReferDocNoField;
		}
		set
		{
			if ((object)ReferDocNoField != value)
			{
				ReferDocNoField = value;
				RaisePropertyChanged("ReferDocNo");
			}
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 13)]
	public string ModifyBy
	{
		get
		{
			return ModifyByField;
		}
		set
		{
			if ((object)ModifyByField != value)
			{
				ModifyByField = value;
				RaisePropertyChanged("ModifyBy");
			}
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 14)]
	public string CreateUser
	{
		get
		{
			return CreateUserField;
		}
		set
		{
			if ((object)CreateUserField != value)
			{
				CreateUserField = value;
				RaisePropertyChanged("CreateUser");
			}
		}
	}

	[DataMember(IsRequired = true, Order = 15)]
	public DateTime? CreateDate
	{
		get
		{
			return CreateDateField;
		}
		set
		{
			if (!CreateDateField.Equals(value))
			{
				CreateDateField = value;
				RaisePropertyChanged("CreateDate");
			}
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 16)]
	public string ReleaseUser
	{
		get
		{
			return ReleaseUserField;
		}
		set
		{
			if ((object)ReleaseUserField != value)
			{
				ReleaseUserField = value;
				RaisePropertyChanged("ReleaseUser");
			}
		}
	}

	[DataMember(IsRequired = true, Order = 17)]
	public DateTime? ReleaseDate
	{
		get
		{
			return ReleaseDateField;
		}
		set
		{
			if (!ReleaseDateField.Equals(value))
			{
				ReleaseDateField = value;
				RaisePropertyChanged("ReleaseDate");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
