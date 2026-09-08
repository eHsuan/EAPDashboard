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
public class WOComponentCust : INotifyPropertyChanged
{
	private long indexIDField;

	private string custTypeField;

	private string serialNoField;

	private string custSerialNoField;

	private string enableField;

	private DateTime createDateField;

	private string createUserField;

	private DateTime updateDateField;

	private string updateUserField;

	[XmlElement(Order = 0)]
	public long IndexID
	{
		get
		{
			return indexIDField;
		}
		set
		{
			indexIDField = value;
			RaisePropertyChanged("IndexID");
		}
	}

	[XmlElement(Order = 1)]
	public string CustType
	{
		get
		{
			return custTypeField;
		}
		set
		{
			custTypeField = value;
			RaisePropertyChanged("CustType");
		}
	}

	[XmlElement(Order = 2)]
	public string SerialNo
	{
		get
		{
			return serialNoField;
		}
		set
		{
			serialNoField = value;
			RaisePropertyChanged("SerialNo");
		}
	}

	[XmlElement(Order = 3)]
	public string CustSerialNo
	{
		get
		{
			return custSerialNoField;
		}
		set
		{
			custSerialNoField = value;
			RaisePropertyChanged("CustSerialNo");
		}
	}

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 7)]
	public DateTime UpdateDate
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

	[XmlElement(Order = 8)]
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
