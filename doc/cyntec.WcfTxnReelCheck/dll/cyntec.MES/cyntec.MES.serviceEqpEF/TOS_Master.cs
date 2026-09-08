using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpEF;

[Serializable]
[XmlInclude(typeof(CUS_TOS_Master))]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://tempuri.org/")]
public class TOS_Master : INotifyPropertyChanged
{
	private int idField;

	private string toolNoField;

	private string toolTypeCodeField;

	private string toolNameField;

	private string matNoField;

	private string matGroupNoField;

	private string versionField;

	private string lifeTypeField;

	private decimal? lifeQuotaField;

	private string unitField;

	private DateTime validStartDateField;

	private DateTime validEndDateField;

	private decimal maintainQuotaField;

	private decimal checkQuotaField;

	private decimal defectQuotaField;

	private string machNoField;

	private string statusCodeField;

	private string needMaintainField;

	private string needCheckField;

	private string eOLField;

	private string expiredField;

	private string overedField;

	private string pONoField;

	private string vendorCodeField;

	private string layerField;

	private DateTime? onLineDateField;

	private string enableField;

	private string operateNoteField;

	private string remarkField;

	private DateTime createDateField;

	private string createUserField;

	private DateTime? updateDateField;

	private string updateUserField;

	private decimal ratioField;

	private DateTime? maintainDateField;

	private DateTime? checkDateField;

	private DateTime? scrapDateField;

	private string toolGroupNoField;

	[XmlElement(Order = 0)]
	public int ID
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
			RaisePropertyChanged("ID");
		}
	}

	[XmlElement(Order = 1)]
	public string ToolNo
	{
		get
		{
			return toolNoField;
		}
		set
		{
			toolNoField = value;
			RaisePropertyChanged("ToolNo");
		}
	}

	[XmlElement(Order = 2)]
	public string ToolTypeCode
	{
		get
		{
			return toolTypeCodeField;
		}
		set
		{
			toolTypeCodeField = value;
			RaisePropertyChanged("ToolTypeCode");
		}
	}

	[XmlElement(Order = 3)]
	public string ToolName
	{
		get
		{
			return toolNameField;
		}
		set
		{
			toolNameField = value;
			RaisePropertyChanged("ToolName");
		}
	}

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
	public string Version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
			RaisePropertyChanged("Version");
		}
	}

	[XmlElement(Order = 7)]
	public string LifeType
	{
		get
		{
			return lifeTypeField;
		}
		set
		{
			lifeTypeField = value;
			RaisePropertyChanged("LifeType");
		}
	}

	[XmlElement(IsNullable = true, Order = 8)]
	public decimal? LifeQuota
	{
		get
		{
			return lifeQuotaField;
		}
		set
		{
			lifeQuotaField = value;
			RaisePropertyChanged("LifeQuota");
		}
	}

	[XmlElement(Order = 9)]
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

	[XmlElement(Order = 10)]
	public DateTime ValidStartDate
	{
		get
		{
			return validStartDateField;
		}
		set
		{
			validStartDateField = value;
			RaisePropertyChanged("ValidStartDate");
		}
	}

	[XmlElement(Order = 11)]
	public DateTime ValidEndDate
	{
		get
		{
			return validEndDateField;
		}
		set
		{
			validEndDateField = value;
			RaisePropertyChanged("ValidEndDate");
		}
	}

	[XmlElement(Order = 12)]
	public decimal MaintainQuota
	{
		get
		{
			return maintainQuotaField;
		}
		set
		{
			maintainQuotaField = value;
			RaisePropertyChanged("MaintainQuota");
		}
	}

	[XmlElement(Order = 13)]
	public decimal CheckQuota
	{
		get
		{
			return checkQuotaField;
		}
		set
		{
			checkQuotaField = value;
			RaisePropertyChanged("CheckQuota");
		}
	}

	[XmlElement(Order = 14)]
	public decimal DefectQuota
	{
		get
		{
			return defectQuotaField;
		}
		set
		{
			defectQuotaField = value;
			RaisePropertyChanged("DefectQuota");
		}
	}

	[XmlElement(Order = 15)]
	public string MachNo
	{
		get
		{
			return machNoField;
		}
		set
		{
			machNoField = value;
			RaisePropertyChanged("MachNo");
		}
	}

	[XmlElement(Order = 16)]
	public string StatusCode
	{
		get
		{
			return statusCodeField;
		}
		set
		{
			statusCodeField = value;
			RaisePropertyChanged("StatusCode");
		}
	}

	[XmlElement(Order = 17)]
	public string NeedMaintain
	{
		get
		{
			return needMaintainField;
		}
		set
		{
			needMaintainField = value;
			RaisePropertyChanged("NeedMaintain");
		}
	}

	[XmlElement(Order = 18)]
	public string NeedCheck
	{
		get
		{
			return needCheckField;
		}
		set
		{
			needCheckField = value;
			RaisePropertyChanged("NeedCheck");
		}
	}

	[XmlElement(Order = 19)]
	public string EOL
	{
		get
		{
			return eOLField;
		}
		set
		{
			eOLField = value;
			RaisePropertyChanged("EOL");
		}
	}

	[XmlElement(Order = 20)]
	public string Expired
	{
		get
		{
			return expiredField;
		}
		set
		{
			expiredField = value;
			RaisePropertyChanged("Expired");
		}
	}

	[XmlElement(Order = 21)]
	public string Overed
	{
		get
		{
			return overedField;
		}
		set
		{
			overedField = value;
			RaisePropertyChanged("Overed");
		}
	}

	[XmlElement(Order = 22)]
	public string PONo
	{
		get
		{
			return pONoField;
		}
		set
		{
			pONoField = value;
			RaisePropertyChanged("PONo");
		}
	}

	[XmlElement(Order = 23)]
	public string VendorCode
	{
		get
		{
			return vendorCodeField;
		}
		set
		{
			vendorCodeField = value;
			RaisePropertyChanged("VendorCode");
		}
	}

	[XmlElement(Order = 24)]
	public string Layer
	{
		get
		{
			return layerField;
		}
		set
		{
			layerField = value;
			RaisePropertyChanged("Layer");
		}
	}

	[XmlElement(IsNullable = true, Order = 25)]
	public DateTime? OnLineDate
	{
		get
		{
			return onLineDateField;
		}
		set
		{
			onLineDateField = value;
			RaisePropertyChanged("OnLineDate");
		}
	}

	[XmlElement(Order = 26)]
	public string Enable
	{
		get
		{
			return enableField;
		}
		set
		{
			enableField = value;
			RaisePropertyChanged("Enable");
		}
	}

	[XmlElement(Order = 27)]
	public string OperateNote
	{
		get
		{
			return operateNoteField;
		}
		set
		{
			operateNoteField = value;
			RaisePropertyChanged("OperateNote");
		}
	}

	[XmlElement(Order = 28)]
	public string Remark
	{
		get
		{
			return remarkField;
		}
		set
		{
			remarkField = value;
			RaisePropertyChanged("Remark");
		}
	}

	[XmlElement(Order = 29)]
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

	[XmlElement(Order = 30)]
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

	[XmlElement(IsNullable = true, Order = 31)]
	public DateTime? UpdateDate
	{
		get
		{
			return updateDateField;
		}
		set
		{
			updateDateField = value;
			RaisePropertyChanged("UpdateDate");
		}
	}

	[XmlElement(Order = 32)]
	public string UpdateUser
	{
		get
		{
			return updateUserField;
		}
		set
		{
			updateUserField = value;
			RaisePropertyChanged("UpdateUser");
		}
	}

	[XmlElement(Order = 33)]
	public decimal Ratio
	{
		get
		{
			return ratioField;
		}
		set
		{
			ratioField = value;
			RaisePropertyChanged("Ratio");
		}
	}

	[XmlElement(IsNullable = true, Order = 34)]
	public DateTime? MaintainDate
	{
		get
		{
			return maintainDateField;
		}
		set
		{
			maintainDateField = value;
			RaisePropertyChanged("MaintainDate");
		}
	}

	[XmlElement(IsNullable = true, Order = 35)]
	public DateTime? CheckDate
	{
		get
		{
			return checkDateField;
		}
		set
		{
			checkDateField = value;
			RaisePropertyChanged("CheckDate");
		}
	}

	[XmlElement(IsNullable = true, Order = 36)]
	public DateTime? ScrapDate
	{
		get
		{
			return scrapDateField;
		}
		set
		{
			scrapDateField = value;
			RaisePropertyChanged("ScrapDate");
		}
	}

	[XmlElement(Order = 37)]
	public string ToolGroupNo
	{
		get
		{
			return toolGroupNoField;
		}
		set
		{
			toolGroupNoField = value;
			RaisePropertyChanged("ToolGroupNo");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
