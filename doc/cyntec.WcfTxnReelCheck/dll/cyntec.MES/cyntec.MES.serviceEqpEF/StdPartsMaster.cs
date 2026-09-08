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
public class StdPartsMaster : INotifyPropertyChanged
{
	private string stdPartsIDField;

	private string stdPartsTypeField;

	private string categoryCodeField;

	private string categoryNameField;

	private string plantNoField;

	private string lineCodeField;

	private string matGroupNoField;

	private string matNoField;

	private string workCenterNoField;

	private int noticeDayField;

	private string publishDateField;

	private string remarkField;

	private int safeQtyField;

	private string sendDateField;

	private string stdPartsAliasField;

	private decimal validDateField;

	private int validPeriodField;

	private StdPartsCheckItem[] stdPartsItemsField;

	[XmlElement(Order = 0)]
	public string StdPartsID
	{
		get
		{
			return stdPartsIDField;
		}
		set
		{
			stdPartsIDField = value;
			RaisePropertyChanged("StdPartsID");
		}
	}

	[XmlElement(Order = 1)]
	public string StdPartsType
	{
		get
		{
			return stdPartsTypeField;
		}
		set
		{
			stdPartsTypeField = value;
			RaisePropertyChanged("StdPartsType");
		}
	}

	[XmlElement(Order = 2)]
	public string CategoryCode
	{
		get
		{
			return categoryCodeField;
		}
		set
		{
			categoryCodeField = value;
			RaisePropertyChanged("CategoryCode");
		}
	}

	[XmlElement(Order = 3)]
	public string CategoryName
	{
		get
		{
			return categoryNameField;
		}
		set
		{
			categoryNameField = value;
			RaisePropertyChanged("CategoryName");
		}
	}

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
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

	[XmlElement(Order = 7)]
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

	[XmlElement(Order = 8)]
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

	[XmlElement(Order = 9)]
	public int NoticeDay
	{
		get
		{
			return noticeDayField;
		}
		set
		{
			noticeDayField = value;
			RaisePropertyChanged("NoticeDay");
		}
	}

	[XmlElement(Order = 10)]
	public string PublishDate
	{
		get
		{
			return publishDateField;
		}
		set
		{
			publishDateField = value;
			RaisePropertyChanged("PublishDate");
		}
	}

	[XmlElement(Order = 11)]
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

	[XmlElement(Order = 12)]
	public int SafeQty
	{
		get
		{
			return safeQtyField;
		}
		set
		{
			safeQtyField = value;
			RaisePropertyChanged("SafeQty");
		}
	}

	[XmlElement(Order = 13)]
	public string SendDate
	{
		get
		{
			return sendDateField;
		}
		set
		{
			sendDateField = value;
			RaisePropertyChanged("SendDate");
		}
	}

	[XmlElement(Order = 14)]
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

	[XmlElement(Order = 15)]
	public decimal ValidDate
	{
		get
		{
			return validDateField;
		}
		set
		{
			validDateField = value;
			RaisePropertyChanged("ValidDate");
		}
	}

	[XmlElement(Order = 16)]
	public int ValidPeriod
	{
		get
		{
			return validPeriodField;
		}
		set
		{
			validPeriodField = value;
			RaisePropertyChanged("ValidPeriod");
		}
	}

	[XmlArray(Order = 17)]
	public StdPartsCheckItem[] StdPartsItems
	{
		get
		{
			return stdPartsItemsField;
		}
		set
		{
			stdPartsItemsField = value;
			RaisePropertyChanged("StdPartsItems");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
