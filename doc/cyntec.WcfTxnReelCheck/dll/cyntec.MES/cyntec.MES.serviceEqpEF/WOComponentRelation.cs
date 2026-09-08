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
public class WOComponentRelation : INotifyPropertyChanged
{
	private long indexIDField;

	private string wONoField;

	private string serialNoField;

	private string refSerialNoField;

	private DateTime createDateField;

	private string createUserField;

	private DateTime changeDateField;

	private string changeUserField;

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
	public string WONo
	{
		get
		{
			return wONoField;
		}
		set
		{
			wONoField = value;
			RaisePropertyChanged("WONo");
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
	public string RefSerialNo
	{
		get
		{
			return refSerialNoField;
		}
		set
		{
			refSerialNoField = value;
			RaisePropertyChanged("RefSerialNo");
		}
	}

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 6)]
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

	[XmlElement(Order = 7)]
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
