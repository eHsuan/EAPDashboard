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
public class StdCheckItemsQueryItem : INotifyPropertyChanged
{
	private string plantNoField;

	private string matGroupNoField;

	private string lineCodeField;

	private string matNoField;

	private string workCenterNoField;

	private string stdPartItemField;

	private string stdPartParamField;

	private string stdPartParamSeqField;

	private bool isKeepPartNoField;

	private string frequenceField;

	private string frequenceUnitField;

	private int matIDField;

	private string paramValueField;

	private string lowerValueField;

	private string upperValueField;

	private string stdPartIDField;

	private string stdPartTypeField;

	private string stdPartsAliasField;

	private string sysIDField;

	private string unitField;

	private decimal validHoursField;

	private DateTime operatedDateField;

	private string toolingIDField;

	private string[] dataListField;

	private bool isCompleteField;

	[XmlElement(Order = 0)]
	public string PlantNo
	{
		get
		{
			return plantNoField;
		}
		set
		{
			plantNoField = value;
			RaisePropertyChanged("PlantNo");
		}
	}

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
	public string LineCode
	{
		get
		{
			return lineCodeField;
		}
		set
		{
			lineCodeField = value;
			RaisePropertyChanged("LineCode");
		}
	}

	[XmlElement(Order = 3)]
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
	public string StdPartItem
	{
		get
		{
			return stdPartItemField;
		}
		set
		{
			stdPartItemField = value;
			RaisePropertyChanged("StdPartItem");
		}
	}

	[XmlElement(Order = 6)]
	public string StdPartParam
	{
		get
		{
			return stdPartParamField;
		}
		set
		{
			stdPartParamField = value;
			RaisePropertyChanged("StdPartParam");
		}
	}

	[XmlElement(Order = 7)]
	public string StdPartParamSeq
	{
		get
		{
			return stdPartParamSeqField;
		}
		set
		{
			stdPartParamSeqField = value;
			RaisePropertyChanged("StdPartParamSeq");
		}
	}

	[XmlElement(Order = 8)]
	public bool IsKeepPartNo
	{
		get
		{
			return isKeepPartNoField;
		}
		set
		{
			isKeepPartNoField = value;
			RaisePropertyChanged("IsKeepPartNo");
		}
	}

	[XmlElement(Order = 9)]
	public string Frequence
	{
		get
		{
			return frequenceField;
		}
		set
		{
			frequenceField = value;
			RaisePropertyChanged("Frequence");
		}
	}

	[XmlElement(Order = 10)]
	public string FrequenceUnit
	{
		get
		{
			return frequenceUnitField;
		}
		set
		{
			frequenceUnitField = value;
			RaisePropertyChanged("FrequenceUnit");
		}
	}

	[XmlElement(Order = 11)]
	public int MatID
	{
		get
		{
			return matIDField;
		}
		set
		{
			matIDField = value;
			RaisePropertyChanged("MatID");
		}
	}

	[XmlElement(Order = 12)]
	public string ParamValue
	{
		get
		{
			return paramValueField;
		}
		set
		{
			paramValueField = value;
			RaisePropertyChanged("ParamValue");
		}
	}

	[XmlElement(Order = 13)]
	public string LowerValue
	{
		get
		{
			return lowerValueField;
		}
		set
		{
			lowerValueField = value;
			RaisePropertyChanged("LowerValue");
		}
	}

	[XmlElement(Order = 14)]
	public string UpperValue
	{
		get
		{
			return upperValueField;
		}
		set
		{
			upperValueField = value;
			RaisePropertyChanged("UpperValue");
		}
	}

	[XmlElement(Order = 15)]
	public string StdPartID
	{
		get
		{
			return stdPartIDField;
		}
		set
		{
			stdPartIDField = value;
			RaisePropertyChanged("StdPartID");
		}
	}

	[XmlElement(Order = 16)]
	public string StdPartType
	{
		get
		{
			return stdPartTypeField;
		}
		set
		{
			stdPartTypeField = value;
			RaisePropertyChanged("StdPartType");
		}
	}

	[XmlElement(Order = 17)]
	public string StdPartsAlias
	{
		get
		{
			return stdPartsAliasField;
		}
		set
		{
			stdPartsAliasField = value;
			RaisePropertyChanged("StdPartsAlias");
		}
	}

	[XmlElement(Order = 18)]
	public string SysID
	{
		get
		{
			return sysIDField;
		}
		set
		{
			sysIDField = value;
			RaisePropertyChanged("SysID");
		}
	}

	[XmlElement(Order = 19)]
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

	[XmlElement(Order = 20)]
	public decimal ValidHours
	{
		get
		{
			return validHoursField;
		}
		set
		{
			validHoursField = value;
			RaisePropertyChanged("ValidHours");
		}
	}

	[XmlElement(Order = 21)]
	public DateTime OperatedDate
	{
		get
		{
			return operatedDateField;
		}
		set
		{
			operatedDateField = value;
			RaisePropertyChanged("OperatedDate");
		}
	}

	[XmlElement(Order = 22)]
	public string ToolingID
	{
		get
		{
			return toolingIDField;
		}
		set
		{
			toolingIDField = value;
			RaisePropertyChanged("ToolingID");
		}
	}

	[XmlArray(Order = 23)]
	public string[] DataList
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

	[XmlElement(Order = 24)]
	public bool IsComplete
	{
		get
		{
			return isCompleteField;
		}
		set
		{
			isCompleteField = value;
			RaisePropertyChanged("IsComplete");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
