using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MatGroupNo", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class MatGroupNo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string CreateDateField;

	[OptionalField]
	private string CreateUserField;

	[OptionalField]
	private bool EnableField;

	[OptionalField]
	private bool EnableMachBarcodeField;

	[OptionalField]
	private bool IsAutomobileField;

	[OptionalField]
	private string MatGroupNameField;

	[OptionalField]
	private string PlantNoField;

	[OptionalField]
	private string ProductGroupCodeField;

	[OptionalField]
	private string Property1Field;

	[OptionalField]
	private string Property10Field;

	[OptionalField]
	private string Property11Field;

	[OptionalField]
	private string Property12Field;

	[OptionalField]
	private string Property13Field;

	[OptionalField]
	private string Property14Field;

	[OptionalField]
	private string Property15Field;

	[OptionalField]
	private string Property16Field;

	[OptionalField]
	private string Property17Field;

	[OptionalField]
	private string Property18Field;

	[OptionalField]
	private string Property19Field;

	[OptionalField]
	private string Property2Field;

	[OptionalField]
	private string Property20Field;

	[OptionalField]
	private string Property3Field;

	[OptionalField]
	private string Property4Field;

	[OptionalField]
	private string Property5Field;

	[OptionalField]
	private string Property6Field;

	[OptionalField]
	private string Property7Field;

	[OptionalField]
	private string Property8Field;

	[OptionalField]
	private string Property9Field;

	[OptionalField]
	private int SortIDField;

	[OptionalField]
	private string UpdateDateField;

	[OptionalField]
	private string UpdateUserField;

	[OptionalField]
	private int YieldField;

	[OptionalField]
	private string matGroupNoField;

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
	public bool EnableMachBarcode
	{
		get
		{
			return EnableMachBarcodeField;
		}
		set
		{
			if (!EnableMachBarcodeField.Equals(value))
			{
				EnableMachBarcodeField = value;
				RaisePropertyChanged("EnableMachBarcode");
			}
		}
	}

	[DataMember]
	public bool IsAutomobile
	{
		get
		{
			return IsAutomobileField;
		}
		set
		{
			if (!IsAutomobileField.Equals(value))
			{
				IsAutomobileField = value;
				RaisePropertyChanged("IsAutomobile");
			}
		}
	}

	[DataMember]
	public string MatGroupName
	{
		get
		{
			return MatGroupNameField;
		}
		set
		{
			if ((object)MatGroupNameField != value)
			{
				MatGroupNameField = value;
				RaisePropertyChanged("MatGroupName");
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
	public string ProductGroupCode
	{
		get
		{
			return ProductGroupCodeField;
		}
		set
		{
			if ((object)ProductGroupCodeField != value)
			{
				ProductGroupCodeField = value;
				RaisePropertyChanged("ProductGroupCode");
			}
		}
	}

	[DataMember]
	public string Property1
	{
		get
		{
			return Property1Field;
		}
		set
		{
			if ((object)Property1Field != value)
			{
				Property1Field = value;
				RaisePropertyChanged("Property1");
			}
		}
	}

	[DataMember]
	public string Property10
	{
		get
		{
			return Property10Field;
		}
		set
		{
			if ((object)Property10Field != value)
			{
				Property10Field = value;
				RaisePropertyChanged("Property10");
			}
		}
	}

	[DataMember]
	public string Property11
	{
		get
		{
			return Property11Field;
		}
		set
		{
			if ((object)Property11Field != value)
			{
				Property11Field = value;
				RaisePropertyChanged("Property11");
			}
		}
	}

	[DataMember]
	public string Property12
	{
		get
		{
			return Property12Field;
		}
		set
		{
			if ((object)Property12Field != value)
			{
				Property12Field = value;
				RaisePropertyChanged("Property12");
			}
		}
	}

	[DataMember]
	public string Property13
	{
		get
		{
			return Property13Field;
		}
		set
		{
			if ((object)Property13Field != value)
			{
				Property13Field = value;
				RaisePropertyChanged("Property13");
			}
		}
	}

	[DataMember]
	public string Property14
	{
		get
		{
			return Property14Field;
		}
		set
		{
			if ((object)Property14Field != value)
			{
				Property14Field = value;
				RaisePropertyChanged("Property14");
			}
		}
	}

	[DataMember]
	public string Property15
	{
		get
		{
			return Property15Field;
		}
		set
		{
			if ((object)Property15Field != value)
			{
				Property15Field = value;
				RaisePropertyChanged("Property15");
			}
		}
	}

	[DataMember]
	public string Property16
	{
		get
		{
			return Property16Field;
		}
		set
		{
			if ((object)Property16Field != value)
			{
				Property16Field = value;
				RaisePropertyChanged("Property16");
			}
		}
	}

	[DataMember]
	public string Property17
	{
		get
		{
			return Property17Field;
		}
		set
		{
			if ((object)Property17Field != value)
			{
				Property17Field = value;
				RaisePropertyChanged("Property17");
			}
		}
	}

	[DataMember]
	public string Property18
	{
		get
		{
			return Property18Field;
		}
		set
		{
			if ((object)Property18Field != value)
			{
				Property18Field = value;
				RaisePropertyChanged("Property18");
			}
		}
	}

	[DataMember]
	public string Property19
	{
		get
		{
			return Property19Field;
		}
		set
		{
			if ((object)Property19Field != value)
			{
				Property19Field = value;
				RaisePropertyChanged("Property19");
			}
		}
	}

	[DataMember]
	public string Property2
	{
		get
		{
			return Property2Field;
		}
		set
		{
			if ((object)Property2Field != value)
			{
				Property2Field = value;
				RaisePropertyChanged("Property2");
			}
		}
	}

	[DataMember]
	public string Property20
	{
		get
		{
			return Property20Field;
		}
		set
		{
			if ((object)Property20Field != value)
			{
				Property20Field = value;
				RaisePropertyChanged("Property20");
			}
		}
	}

	[DataMember]
	public string Property3
	{
		get
		{
			return Property3Field;
		}
		set
		{
			if ((object)Property3Field != value)
			{
				Property3Field = value;
				RaisePropertyChanged("Property3");
			}
		}
	}

	[DataMember]
	public string Property4
	{
		get
		{
			return Property4Field;
		}
		set
		{
			if ((object)Property4Field != value)
			{
				Property4Field = value;
				RaisePropertyChanged("Property4");
			}
		}
	}

	[DataMember]
	public string Property5
	{
		get
		{
			return Property5Field;
		}
		set
		{
			if ((object)Property5Field != value)
			{
				Property5Field = value;
				RaisePropertyChanged("Property5");
			}
		}
	}

	[DataMember]
	public string Property6
	{
		get
		{
			return Property6Field;
		}
		set
		{
			if ((object)Property6Field != value)
			{
				Property6Field = value;
				RaisePropertyChanged("Property6");
			}
		}
	}

	[DataMember]
	public string Property7
	{
		get
		{
			return Property7Field;
		}
		set
		{
			if ((object)Property7Field != value)
			{
				Property7Field = value;
				RaisePropertyChanged("Property7");
			}
		}
	}

	[DataMember]
	public string Property8
	{
		get
		{
			return Property8Field;
		}
		set
		{
			if ((object)Property8Field != value)
			{
				Property8Field = value;
				RaisePropertyChanged("Property8");
			}
		}
	}

	[DataMember]
	public string Property9
	{
		get
		{
			return Property9Field;
		}
		set
		{
			if ((object)Property9Field != value)
			{
				Property9Field = value;
				RaisePropertyChanged("Property9");
			}
		}
	}

	[DataMember]
	public int SortID
	{
		get
		{
			return SortIDField;
		}
		set
		{
			if (!SortIDField.Equals(value))
			{
				SortIDField = value;
				RaisePropertyChanged("SortID");
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
	public int Yield
	{
		get
		{
			return YieldField;
		}
		set
		{
			if (!YieldField.Equals(value))
			{
				YieldField = value;
				RaisePropertyChanged("Yield");
			}
		}
	}

	[DataMember]
	public string matGroupNo
	{
		get
		{
			return matGroupNoField;
		}
		set
		{
			if ((object)matGroupNoField != value)
			{
				matGroupNoField = value;
				RaisePropertyChanged("matGroupNo");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
