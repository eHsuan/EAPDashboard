using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_Leave", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_Leave : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private decimal AddMatQtyField;

	[OptionalField]
	private string AodField;

	[OptionalField]
	private string AodDateField;

	[OptionalField]
	private string AodReleaseDateField;

	[OptionalField]
	private string AodReleaseUserField;

	[OptionalField]
	private string AodRemarkField;

	[OptionalField]
	private string AodUserField;

	[OptionalField]
	private string BatchNoField;

	[OptionalField]
	private string CreateDateField;

	[OptionalField]
	private string ExpiredMsgField;

	[OptionalField]
	private bool IsExpiredField;

	[OptionalField]
	private bool IsFedMatField;

	[OptionalField]
	private bool IsScrapField;

	[OptionalField]
	private bool IsUsedField;

	[OptionalField]
	private string LocField;

	[OptionalField]
	private string MatGroupNoField;

	[OptionalField]
	private string MatNoField;

	[OptionalField]
	private string NowField;

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
	private string Property9Field;

	[OptionalField]
	private decimal QtyField;

	[OptionalField]
	private string ReelIDField;

	[OptionalField]
	private string ScrapField;

	[OptionalField]
	private string ScrapDateField;

	[OptionalField]
	private string ScrapMsgField;

	[OptionalField]
	private string ScrapReleaseDateField;

	[OptionalField]
	private string ScrapReleaseUserField;

	[OptionalField]
	private string ScrapRemarkField;

	[OptionalField]
	private string ScrapUserField;

	[OptionalField]
	private string UpdateDateField;

	[OptionalField]
	private string UpdateUserField;

	[OptionalField]
	private string ValidDateField;

	[OptionalField]
	private string WONoField;

	[OptionalField]
	private string WorkCenterNoField;

	[OptionalField]
	private string createUserField;

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
	public decimal AddMatQty
	{
		get
		{
			return AddMatQtyField;
		}
		set
		{
			if (!AddMatQtyField.Equals(value))
			{
				AddMatQtyField = value;
				RaisePropertyChanged("AddMatQty");
			}
		}
	}

	[DataMember]
	public string Aod
	{
		get
		{
			return AodField;
		}
		set
		{
			if ((object)AodField != value)
			{
				AodField = value;
				RaisePropertyChanged("Aod");
			}
		}
	}

	[DataMember]
	public string AodDate
	{
		get
		{
			return AodDateField;
		}
		set
		{
			if ((object)AodDateField != value)
			{
				AodDateField = value;
				RaisePropertyChanged("AodDate");
			}
		}
	}

	[DataMember]
	public string AodReleaseDate
	{
		get
		{
			return AodReleaseDateField;
		}
		set
		{
			if ((object)AodReleaseDateField != value)
			{
				AodReleaseDateField = value;
				RaisePropertyChanged("AodReleaseDate");
			}
		}
	}

	[DataMember]
	public string AodReleaseUser
	{
		get
		{
			return AodReleaseUserField;
		}
		set
		{
			if ((object)AodReleaseUserField != value)
			{
				AodReleaseUserField = value;
				RaisePropertyChanged("AodReleaseUser");
			}
		}
	}

	[DataMember]
	public string AodRemark
	{
		get
		{
			return AodRemarkField;
		}
		set
		{
			if ((object)AodRemarkField != value)
			{
				AodRemarkField = value;
				RaisePropertyChanged("AodRemark");
			}
		}
	}

	[DataMember]
	public string AodUser
	{
		get
		{
			return AodUserField;
		}
		set
		{
			if ((object)AodUserField != value)
			{
				AodUserField = value;
				RaisePropertyChanged("AodUser");
			}
		}
	}

	[DataMember]
	public string BatchNo
	{
		get
		{
			return BatchNoField;
		}
		set
		{
			if ((object)BatchNoField != value)
			{
				BatchNoField = value;
				RaisePropertyChanged("BatchNo");
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
	public string ExpiredMsg
	{
		get
		{
			return ExpiredMsgField;
		}
		set
		{
			if ((object)ExpiredMsgField != value)
			{
				ExpiredMsgField = value;
				RaisePropertyChanged("ExpiredMsg");
			}
		}
	}

	[DataMember]
	public bool IsExpired
	{
		get
		{
			return IsExpiredField;
		}
		set
		{
			if (!IsExpiredField.Equals(value))
			{
				IsExpiredField = value;
				RaisePropertyChanged("IsExpired");
			}
		}
	}

	[DataMember]
	public bool IsFedMat
	{
		get
		{
			return IsFedMatField;
		}
		set
		{
			if (!IsFedMatField.Equals(value))
			{
				IsFedMatField = value;
				RaisePropertyChanged("IsFedMat");
			}
		}
	}

	[DataMember]
	public bool IsScrap
	{
		get
		{
			return IsScrapField;
		}
		set
		{
			if (!IsScrapField.Equals(value))
			{
				IsScrapField = value;
				RaisePropertyChanged("IsScrap");
			}
		}
	}

	[DataMember]
	public bool IsUsed
	{
		get
		{
			return IsUsedField;
		}
		set
		{
			if (!IsUsedField.Equals(value))
			{
				IsUsedField = value;
				RaisePropertyChanged("IsUsed");
			}
		}
	}

	[DataMember]
	public string Loc
	{
		get
		{
			return LocField;
		}
		set
		{
			if ((object)LocField != value)
			{
				LocField = value;
				RaisePropertyChanged("Loc");
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
	public string Now
	{
		get
		{
			return NowField;
		}
		set
		{
			if ((object)NowField != value)
			{
				NowField = value;
				RaisePropertyChanged("Now");
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
	public decimal Qty
	{
		get
		{
			return QtyField;
		}
		set
		{
			if (!QtyField.Equals(value))
			{
				QtyField = value;
				RaisePropertyChanged("Qty");
			}
		}
	}

	[DataMember]
	public string ReelID
	{
		get
		{
			return ReelIDField;
		}
		set
		{
			if ((object)ReelIDField != value)
			{
				ReelIDField = value;
				RaisePropertyChanged("ReelID");
			}
		}
	}

	[DataMember]
	public string Scrap
	{
		get
		{
			return ScrapField;
		}
		set
		{
			if ((object)ScrapField != value)
			{
				ScrapField = value;
				RaisePropertyChanged("Scrap");
			}
		}
	}

	[DataMember]
	public string ScrapDate
	{
		get
		{
			return ScrapDateField;
		}
		set
		{
			if ((object)ScrapDateField != value)
			{
				ScrapDateField = value;
				RaisePropertyChanged("ScrapDate");
			}
		}
	}

	[DataMember]
	public string ScrapMsg
	{
		get
		{
			return ScrapMsgField;
		}
		set
		{
			if ((object)ScrapMsgField != value)
			{
				ScrapMsgField = value;
				RaisePropertyChanged("ScrapMsg");
			}
		}
	}

	[DataMember]
	public string ScrapReleaseDate
	{
		get
		{
			return ScrapReleaseDateField;
		}
		set
		{
			if ((object)ScrapReleaseDateField != value)
			{
				ScrapReleaseDateField = value;
				RaisePropertyChanged("ScrapReleaseDate");
			}
		}
	}

	[DataMember]
	public string ScrapReleaseUser
	{
		get
		{
			return ScrapReleaseUserField;
		}
		set
		{
			if ((object)ScrapReleaseUserField != value)
			{
				ScrapReleaseUserField = value;
				RaisePropertyChanged("ScrapReleaseUser");
			}
		}
	}

	[DataMember]
	public string ScrapRemark
	{
		get
		{
			return ScrapRemarkField;
		}
		set
		{
			if ((object)ScrapRemarkField != value)
			{
				ScrapRemarkField = value;
				RaisePropertyChanged("ScrapRemark");
			}
		}
	}

	[DataMember]
	public string ScrapUser
	{
		get
		{
			return ScrapUserField;
		}
		set
		{
			if ((object)ScrapUserField != value)
			{
				ScrapUserField = value;
				RaisePropertyChanged("ScrapUser");
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
	public string ValidDate
	{
		get
		{
			return ValidDateField;
		}
		set
		{
			if ((object)ValidDateField != value)
			{
				ValidDateField = value;
				RaisePropertyChanged("ValidDate");
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
	public string createUser
	{
		get
		{
			return createUserField;
		}
		set
		{
			if ((object)createUserField != value)
			{
				createUserField = value;
				RaisePropertyChanged("createUser");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
