using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TraceMatCode", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class TraceMatCode : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string ChangeDateField;

	[OptionalField]
	private string ChangeUserField;

	[OptionalField]
	private string CreateDateField;

	[OptionalField]
	private string CreateUserField;

	[OptionalField]
	private bool EnableField;

	[OptionalField]
	private string MatGroupNoField;

	[OptionalField]
	private string PlantNoField;

	[OptionalField]
	private string RefTypeField;

	[OptionalField]
	private bool RequireField;

	[OptionalField]
	private string TraceMatNameField;

	[OptionalField]
	private string WorkCenterNoField;

	[OptionalField]
	private string traceMatCodeField;

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
	public string ChangeDate
	{
		get
		{
			return ChangeDateField;
		}
		set
		{
			if ((object)ChangeDateField != value)
			{
				ChangeDateField = value;
				RaisePropertyChanged("ChangeDate");
			}
		}
	}

	[DataMember]
	public string ChangeUser
	{
		get
		{
			return ChangeUserField;
		}
		set
		{
			if ((object)ChangeUserField != value)
			{
				ChangeUserField = value;
				RaisePropertyChanged("ChangeUser");
			}
		}
	}

	[DataMember]
	public string CreateDate
	{
		get
		{
			return CreateDateField;
		}
		set
		{
			if ((object)CreateDateField != value)
			{
				CreateDateField = value;
				RaisePropertyChanged("CreateDate");
			}
		}
	}

	[DataMember]
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

	[DataMember]
	public bool Enable
	{
		get
		{
			return EnableField;
		}
		set
		{
			if (!EnableField.Equals(value))
			{
				EnableField = value;
				RaisePropertyChanged("Enable");
			}
		}
	}

	[DataMember]
	public string MatGroupNo
	{
		get
		{
			return MatGroupNoField;
		}
		set
		{
			if ((object)MatGroupNoField != value)
			{
				MatGroupNoField = value;
				RaisePropertyChanged("MatGroupNo");
			}
		}
	}

	[DataMember]
	public string PlantNo
	{
		get
		{
			return PlantNoField;
		}
		set
		{
			if ((object)PlantNoField != value)
			{
				PlantNoField = value;
				RaisePropertyChanged("PlantNo");
			}
		}
	}

	[DataMember]
	public string RefType
	{
		get
		{
			return RefTypeField;
		}
		set
		{
			if ((object)RefTypeField != value)
			{
				RefTypeField = value;
				RaisePropertyChanged("RefType");
			}
		}
	}

	[DataMember]
	public bool Require
	{
		get
		{
			return RequireField;
		}
		set
		{
			if (!RequireField.Equals(value))
			{
				RequireField = value;
				RaisePropertyChanged("Require");
			}
		}
	}

	[DataMember]
	public string TraceMatName
	{
		get
		{
			return TraceMatNameField;
		}
		set
		{
			if ((object)TraceMatNameField != value)
			{
				TraceMatNameField = value;
				RaisePropertyChanged("TraceMatName");
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

	[DataMember]
	public string traceMatCode
	{
		get
		{
			return traceMatCodeField;
		}
		set
		{
			if ((object)traceMatCodeField != value)
			{
				traceMatCodeField = value;
				RaisePropertyChanged("traceMatCode");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
