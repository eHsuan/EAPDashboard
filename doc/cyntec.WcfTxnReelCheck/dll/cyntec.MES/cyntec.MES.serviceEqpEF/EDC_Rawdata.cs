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
public class EDC_Rawdata : INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private DateTime changeDateField;

	private string changeUserField;

	private string commentsField;

	private int componentNoField;

	private DateTime createDateField;

	private string createUserField;

	private double[] dataListField;

	private string edcPlanItemSysIDField;

	private string edcPlanSysIDField;

	private string edcRawdataSummarySysIDField;

	private string edcRawdataSysIDField;

	private string eqpNoField;

	private int flowIDField;

	private bool isAvailableField;

	private string itemNameField;

	private int itemSequenceNoField;

	private int machIDField;

	private string matGroupNoField;

	private string matNoField;

	private int minPointQtyField;

	private int ponitQtyField;

	private string resultField;

	private string specMAXField;

	private string specMINField;

	private string unitField;

	private decimal unitRateField;

	private string woAVGField;

	private string woMAXField;

	private string woMINField;

	private string woNoField;

	private string workCenterNoField;

	private string dataTypeField;

	private string dataItemListField;

	private string dataStringField;

	private string componentIDField;

	private EDC_Rawdata_Summary summaryField;

	private EDC_Confirm confirmField;

	private string[] dataListStringField;

	private string refFieldCodeField;

	private string dataRangeField;

	private string dataSTDField;

	private bool isFirstCheckField;

	private string machAliasField;

	private string specValueField;

	private string woSTDField;

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
	public int ComponentNo
	{
		get
		{
			return componentNoField;
		}
		set
		{
			componentNoField = value;
			RaisePropertyChanged("ComponentNo");
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

	[XmlArray(Order = 7)]
	public double[] DataList
	{
		get
		{
			return dataListField;
		}
		set
		{
			dataListField = value;
			RaisePropertyChanged("DataList");
		}
	}

	[XmlElement(Order = 8)]
	public string EdcPlanItemSysID
	{
		get
		{
			return edcPlanItemSysIDField;
		}
		set
		{
			edcPlanItemSysIDField = value;
			RaisePropertyChanged("EdcPlanItemSysID");
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
	public string EdcRawdataSysID
	{
		get
		{
			return edcRawdataSysIDField;
		}
		set
		{
			edcRawdataSysIDField = value;
			RaisePropertyChanged("EdcRawdataSysID");
		}
	}

	[XmlElement(Order = 12)]
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

	[XmlElement(Order = 15)]
	public string ItemName
	{
		get
		{
			return itemNameField;
		}
		set
		{
			itemNameField = value;
			RaisePropertyChanged("ItemName");
		}
	}

	[XmlElement(Order = 16)]
	public int ItemSequenceNo
	{
		get
		{
			return itemSequenceNoField;
		}
		set
		{
			itemSequenceNoField = value;
			RaisePropertyChanged("ItemSequenceNo");
		}
	}

	[XmlElement(Order = 17)]
	public int MachID
	{
		get
		{
			return machIDField;
		}
		set
		{
			machIDField = value;
			RaisePropertyChanged("MachID");
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
	public string MatNo
	{
		get
		{
			return matNoField;
		}
		set
		{
			matNoField = value;
			RaisePropertyChanged("MatNo");
		}
	}

	[XmlElement(Order = 20)]
	public int MinPointQty
	{
		get
		{
			return minPointQtyField;
		}
		set
		{
			minPointQtyField = value;
			RaisePropertyChanged("MinPointQty");
		}
	}

	[XmlElement(Order = 21)]
	public int PonitQty
	{
		get
		{
			return ponitQtyField;
		}
		set
		{
			ponitQtyField = value;
			RaisePropertyChanged("PonitQty");
		}
	}

	[XmlElement(Order = 22)]
	public string Result
	{
		get
		{
			return resultField;
		}
		set
		{
			resultField = value;
			RaisePropertyChanged("Result");
		}
	}

	[XmlElement(Order = 23)]
	public string SpecMAX
	{
		get
		{
			return specMAXField;
		}
		set
		{
			specMAXField = value;
			RaisePropertyChanged("SpecMAX");
		}
	}

	[XmlElement(Order = 24)]
	public string SpecMIN
	{
		get
		{
			return specMINField;
		}
		set
		{
			specMINField = value;
			RaisePropertyChanged("SpecMIN");
		}
	}

	[XmlElement(Order = 25)]
	public string Unit
	{
		get
		{
			return unitField;
		}
		set
		{
			unitField = value;
			RaisePropertyChanged("Unit");
		}
	}

	[XmlElement(Order = 26)]
	public decimal UnitRate
	{
		get
		{
			return unitRateField;
		}
		set
		{
			unitRateField = value;
			RaisePropertyChanged("UnitRate");
		}
	}

	[XmlElement(Order = 27)]
	public string WoAVG
	{
		get
		{
			return woAVGField;
		}
		set
		{
			woAVGField = value;
			RaisePropertyChanged("WoAVG");
		}
	}

	[XmlElement(Order = 28)]
	public string WoMAX
	{
		get
		{
			return woMAXField;
		}
		set
		{
			woMAXField = value;
			RaisePropertyChanged("WoMAX");
		}
	}

	[XmlElement(Order = 29)]
	public string WoMIN
	{
		get
		{
			return woMINField;
		}
		set
		{
			woMINField = value;
			RaisePropertyChanged("WoMIN");
		}
	}

	[XmlElement(Order = 30)]
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

	[XmlElement(Order = 31)]
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

	[XmlElement(Order = 32)]
	public string DataType
	{
		get
		{
			return dataTypeField;
		}
		set
		{
			dataTypeField = value;
			RaisePropertyChanged("DataType");
		}
	}

	[XmlElement(Order = 33)]
	public string DataItemList
	{
		get
		{
			return dataItemListField;
		}
		set
		{
			dataItemListField = value;
			RaisePropertyChanged("DataItemList");
		}
	}

	[XmlElement(Order = 34)]
	public string DataString
	{
		get
		{
			return dataStringField;
		}
		set
		{
			dataStringField = value;
			RaisePropertyChanged("DataString");
		}
	}

	[XmlElement(Order = 35)]
	public string ComponentID
	{
		get
		{
			return componentIDField;
		}
		set
		{
			componentIDField = value;
			RaisePropertyChanged("ComponentID");
		}
	}

	[XmlElement(Order = 36)]
	public EDC_Rawdata_Summary Summary
	{
		get
		{
			return summaryField;
		}
		set
		{
			summaryField = value;
			RaisePropertyChanged("Summary");
		}
	}

	[XmlElement(Order = 37)]
	public EDC_Confirm Confirm
	{
		get
		{
			return confirmField;
		}
		set
		{
			confirmField = value;
			RaisePropertyChanged("Confirm");
		}
	}

	[XmlArray(Order = 38)]
	public string[] DataListString
	{
		get
		{
			return dataListStringField;
		}
		set
		{
			dataListStringField = value;
			RaisePropertyChanged("DataListString");
		}
	}

	[XmlElement(Order = 39)]
	public string RefFieldCode
	{
		get
		{
			return refFieldCodeField;
		}
		set
		{
			refFieldCodeField = value;
			RaisePropertyChanged("RefFieldCode");
		}
	}

	[XmlElement(Order = 40)]
	public string DataRange
	{
		get
		{
			return dataRangeField;
		}
		set
		{
			dataRangeField = value;
			RaisePropertyChanged("DataRange");
		}
	}

	[XmlElement(Order = 41)]
	public string DataSTD
	{
		get
		{
			return dataSTDField;
		}
		set
		{
			dataSTDField = value;
			RaisePropertyChanged("DataSTD");
		}
	}

	[XmlElement(Order = 42)]
	public bool IsFirstCheck
	{
		get
		{
			return isFirstCheckField;
		}
		set
		{
			isFirstCheckField = value;
			RaisePropertyChanged("IsFirstCheck");
		}
	}

	[XmlElement(Order = 43)]
	public string MachAlias
	{
		get
		{
			return machAliasField;
		}
		set
		{
			machAliasField = value;
			RaisePropertyChanged("MachAlias");
		}
	}

	[XmlElement(Order = 44)]
	public string SpecValue
	{
		get
		{
			return specValueField;
		}
		set
		{
			specValueField = value;
			RaisePropertyChanged("SpecValue");
		}
	}

	[XmlElement(Order = 45)]
	public string WoSTD
	{
		get
		{
			return woSTDField;
		}
		set
		{
			woSTDField = value;
			RaisePropertyChanged("WoSTD");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
