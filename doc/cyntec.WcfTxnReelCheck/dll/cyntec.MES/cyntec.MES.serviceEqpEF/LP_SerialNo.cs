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
public class LP_SerialNo : INotifyPropertyChanged
{
	private string labelTypeField;

	private string periodField;

	private int? numField;

	private string prefixField;

	private string remarkField;

	private string createUserField;

	private DateTime? createDateField;

	private string updateUserField;

	private DateTime? updateDateField;

	[XmlElement(Order = 0)]
	public string LabelType
	{
		get
		{
			return labelTypeField;
		}
		set
		{
			labelTypeField = value;
			RaisePropertyChanged("LabelType");
		}
	}

	[XmlElement(Order = 1)]
	public string Period
	{
		get
		{
			return periodField;
		}
		set
		{
			periodField = value;
			RaisePropertyChanged("Period");
		}
	}

	[XmlElement(IsNullable = true, Order = 2)]
	public int? Num
	{
		get
		{
			return numField;
		}
		set
		{
			numField = value;
			RaisePropertyChanged("Num");
		}
	}

	[XmlElement(Order = 3)]
	public string Prefix
	{
		get
		{
			return prefixField;
		}
		set
		{
			prefixField = value;
			RaisePropertyChanged("Prefix");
		}
	}

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
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

	[XmlElement(IsNullable = true, Order = 6)]
	public DateTime? CreateDate
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

	[XmlElement(Order = 7)]
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

	[XmlElement(IsNullable = true, Order = 8)]
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
