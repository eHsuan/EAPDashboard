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
[XmlType(Namespace = "http://172.28.113.107:8085/websrv/")]
public class Param : INotifyPropertyChanged
{
	private int? idField;

	private string wONoField;

	private string workCenterNoField;

	private string sourceTypeField;

	private string sourceNoField;

	private string paramCodeField;

	private string paramNameField;

	private string refFieldCodeField;

	private string paramValueField;

	private string upperValueField;

	private string lowerValueField;

	private string unitField;

	private string referDocNoField;

	private string modifyByField;

	private string createUserField;

	private DateTime? createDateField;

	private string releaseUserField;

	private DateTime? releaseDateField;

	[XmlElement(IsNullable = true, Order = 0)]
	public int? ID
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

	[XmlElement(Order = 3)]
	public string SourceType
	{
		get
		{
			return sourceTypeField;
		}
		set
		{
			sourceTypeField = value;
			RaisePropertyChanged("SourceType");
		}
	}

	[XmlElement(Order = 4)]
	public string SourceNo
	{
		get
		{
			return sourceNoField;
		}
		set
		{
			sourceNoField = value;
			RaisePropertyChanged("SourceNo");
		}
	}

	[XmlElement(Order = 5)]
	public string ParamCode
	{
		get
		{
			return paramCodeField;
		}
		set
		{
			paramCodeField = value;
			RaisePropertyChanged("ParamCode");
		}
	}

	[XmlElement(Order = 6)]
	public string ParamName
	{
		get
		{
			return paramNameField;
		}
		set
		{
			paramNameField = value;
			RaisePropertyChanged("ParamName");
		}
	}

	[XmlElement(Order = 7)]
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

	[XmlElement(Order = 8)]
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

	[XmlElement(Order = 9)]
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

	[XmlElement(Order = 10)]
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

	[XmlElement(Order = 11)]
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

	[XmlElement(Order = 12)]
	public string ReferDocNo
	{
		get
		{
			return referDocNoField;
		}
		set
		{
			referDocNoField = value;
			RaisePropertyChanged("ReferDocNo");
		}
	}

	[XmlElement(Order = 13)]
	public string ModifyBy
	{
		get
		{
			return modifyByField;
		}
		set
		{
			modifyByField = value;
			RaisePropertyChanged("ModifyBy");
		}
	}

	[XmlElement(Order = 14)]
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

	[XmlElement(IsNullable = true, Order = 15)]
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

	[XmlElement(Order = 16)]
	public string ReleaseUser
	{
		get
		{
			return releaseUserField;
		}
		set
		{
			releaseUserField = value;
			RaisePropertyChanged("ReleaseUser");
		}
	}

	[XmlElement(IsNullable = true, Order = 17)]
	public DateTime? ReleaseDate
	{
		get
		{
			return releaseDateField;
		}
		set
		{
			releaseDateField = value;
			RaisePropertyChanged("ReleaseDate");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
