using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_BOM", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_BOM : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string BackflushField;

	[OptionalField]
	private string BatchField;

	[OptionalField]
	private string CreateDateField;

	[OptionalField]
	private string CreateUserField;

	[OptionalField]
	private int IDField;

	[OptionalField]
	private string MatNoField;

	[OptionalField]
	private string MoveTypeField;

	[OptionalField]
	private string OperationField;

	[OptionalField]
	private string PlantNoField;

	[OptionalField]
	private string QtyField;

	[OptionalField]
	private string SortStringField;

	[OptionalField]
	private string UnitField;

	[OptionalField]
	private string UpdateDateField;

	[OptionalField]
	private string UpdateUserField;

	[OptionalField]
	private string WONoField;

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
	public string Backflush
	{
		get
		{
			return BackflushField;
		}
		set
		{
			if ((object)BackflushField != value)
			{
				BackflushField = value;
				RaisePropertyChanged("Backflush");
			}
		}
	}

	[DataMember]
	public string Batch
	{
		get
		{
			return BatchField;
		}
		set
		{
			if ((object)BatchField != value)
			{
				BatchField = value;
				RaisePropertyChanged("Batch");
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
	public int ID
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
	public string MoveType
	{
		get
		{
			return MoveTypeField;
		}
		set
		{
			if ((object)MoveTypeField != value)
			{
				MoveTypeField = value;
				RaisePropertyChanged("MoveType");
			}
		}
	}

	[DataMember]
	public string Operation
	{
		get
		{
			return OperationField;
		}
		set
		{
			if ((object)OperationField != value)
			{
				OperationField = value;
				RaisePropertyChanged("Operation");
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
	public string Qty
	{
		get
		{
			return QtyField;
		}
		set
		{
			if ((object)QtyField != value)
			{
				QtyField = value;
				RaisePropertyChanged("Qty");
			}
		}
	}

	[DataMember]
	public string SortString
	{
		get
		{
			return SortStringField;
		}
		set
		{
			if ((object)SortStringField != value)
			{
				SortStringField = value;
				RaisePropertyChanged("SortString");
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
	public string UpdateDate
	{
		get
		{
			return UpdateDateField;
		}
		set
		{
			if ((object)UpdateDateField != value)
			{
				UpdateDateField = value;
				RaisePropertyChanged("UpdateDate");
			}
		}
	}

	[DataMember]
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

	[DataMember]
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
