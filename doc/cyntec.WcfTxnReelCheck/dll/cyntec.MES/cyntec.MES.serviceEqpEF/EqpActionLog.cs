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
public class EqpActionLog : INotifyPropertyChanged
{
	private long indexIDField;

	private string eqpNoField;

	private string actionField;

	private string contentsField;

	private double excutionTimeField;

	private DateTime createDateField;

	private string createUserField;

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

	[XmlElement(Order = 2)]
	public string Action
	{
		get
		{
			return actionField;
		}
		set
		{
			actionField = value;
			RaisePropertyChanged("Action");
		}
	}

	[XmlElement(Order = 3)]
	public string Contents
	{
		get
		{
			return contentsField;
		}
		set
		{
			contentsField = value;
			RaisePropertyChanged("Contents");
		}
	}

	[XmlElement(Order = 4)]
	public double ExcutionTime
	{
		get
		{
			return excutionTimeField;
		}
		set
		{
			excutionTimeField = value;
			RaisePropertyChanged("ExcutionTime");
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
