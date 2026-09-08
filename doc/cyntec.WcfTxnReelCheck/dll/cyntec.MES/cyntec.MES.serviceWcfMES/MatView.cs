using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MatView", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class MatView : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool EnableField;

	[OptionalField]
	private string MarkingFormatField;

	[OptionalField]
	private string MatClassCodeField;

	[OptionalField]
	private string MatGroupNoField;

	[OptionalField]
	private string MatNoField;

	[OptionalField]
	private string PlantNoField;

	[OptionalField]
	private decimal ReelTapeMinTimeField;

	[OptionalField]
	private string RemarkField;

	[OptionalField]
	private int UnitQtyField;

	[OptionalField]
	private string UpdateDateField;

	[OptionalField]
	private string UpdateUserField;

	[OptionalField]
	private string ValClassNoField;

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
	public string MarkingFormat
	{
		get
		{
			return MarkingFormatField;
		}
		set
		{
			if ((object)MarkingFormatField != value)
			{
				MarkingFormatField = value;
				RaisePropertyChanged("MarkingFormat");
			}
		}
	}

	[DataMember]
	public string MatClassCode
	{
		get
		{
			return MatClassCodeField;
		}
		set
		{
			if ((object)MatClassCodeField != value)
			{
				MatClassCodeField = value;
				RaisePropertyChanged("MatClassCode");
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
	public decimal ReelTapeMinTime
	{
		get
		{
			return ReelTapeMinTimeField;
		}
		set
		{
			if (!ReelTapeMinTimeField.Equals(value))
			{
				ReelTapeMinTimeField = value;
				RaisePropertyChanged("ReelTapeMinTime");
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
	public int UnitQty
	{
		get
		{
			return UnitQtyField;
		}
		set
		{
			if (!UnitQtyField.Equals(value))
			{
				UnitQtyField = value;
				RaisePropertyChanged("UnitQty");
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
	public string ValClassNo
	{
		get
		{
			return ValClassNoField;
		}
		set
		{
			if ((object)ValClassNoField != value)
			{
				ValClassNoField = value;
				RaisePropertyChanged("ValClassNo");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
