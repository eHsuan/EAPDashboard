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
public class Result_EdcRawdataCell : INotifyPropertyChanged
{
	private string _WoNoField;

	private string componentNoField;

	private string createDateField;

	private string changeDateField;

	private string refFieldCodeField;

	private string itemNameField;

	private string dataStringField;

	private string dataTypeField;

	[XmlElement(Order = 0)]
	public string _WoNo
	{
		get
		{
			return _WoNoField;
		}
		set
		{
			_WoNoField = value;
			RaisePropertyChanged("_WoNo");
		}
	}

	[XmlElement(Order = 1)]
	public string ComponentNo
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

	[XmlElement(Order = 2)]
	public string CreateDate
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

	[XmlElement(Order = 3)]
	public string ChangeDate
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
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

	[XmlElement(Order = 7)]
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
