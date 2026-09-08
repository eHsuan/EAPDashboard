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
public class Result_CSM_TESLA_LableGet : Basic_Result
{
	private string _Line1Field;

	private string _Line2Field;

	private string _Line3Field;

	private string _Line4Field;

	private string _Line4bField;

	private string _Line4WithOhmField;

	private string _LineQRField;

	private string _JulianDateField;

	private double _PrintTimesDigitalField;

	private double _PrintTimesMaxValueField;

	private string _Default_TPNField;

	private string _Fetched_TPNField;

	private string _Is_Using_Default_TPNField;

	private string _Fetched_ShuntRField;

	private string _Fetched_PrintTimesField;

	private string _Fetched_Tail_Seq_NoField;

	[XmlElement(Order = 0)]
	public string _Line1
	{
		get
		{
			return _Line1Field;
		}
		set
		{
			_Line1Field = value;
			RaisePropertyChanged("_Line1");
		}
	}

	[XmlElement(Order = 1)]
	public string _Line2
	{
		get
		{
			return _Line2Field;
		}
		set
		{
			_Line2Field = value;
			RaisePropertyChanged("_Line2");
		}
	}

	[XmlElement(Order = 2)]
	public string _Line3
	{
		get
		{
			return _Line3Field;
		}
		set
		{
			_Line3Field = value;
			RaisePropertyChanged("_Line3");
		}
	}

	[XmlElement(Order = 3)]
	public string _Line4
	{
		get
		{
			return _Line4Field;
		}
		set
		{
			_Line4Field = value;
			RaisePropertyChanged("_Line4");
		}
	}

	[XmlElement(Order = 4)]
	public string _Line4b
	{
		get
		{
			return _Line4bField;
		}
		set
		{
			_Line4bField = value;
			RaisePropertyChanged("_Line4b");
		}
	}

	[XmlElement(Order = 5)]
	public string _Line4WithOhm
	{
		get
		{
			return _Line4WithOhmField;
		}
		set
		{
			_Line4WithOhmField = value;
			RaisePropertyChanged("_Line4WithOhm");
		}
	}

	[XmlElement(Order = 6)]
	public string _LineQR
	{
		get
		{
			return _LineQRField;
		}
		set
		{
			_LineQRField = value;
			RaisePropertyChanged("_LineQR");
		}
	}

	[XmlElement(Order = 7)]
	public string _JulianDate
	{
		get
		{
			return _JulianDateField;
		}
		set
		{
			_JulianDateField = value;
			RaisePropertyChanged("_JulianDate");
		}
	}

	[XmlElement(Order = 8)]
	public double _PrintTimesDigital
	{
		get
		{
			return _PrintTimesDigitalField;
		}
		set
		{
			_PrintTimesDigitalField = value;
			RaisePropertyChanged("_PrintTimesDigital");
		}
	}

	[XmlElement(Order = 9)]
	public double _PrintTimesMaxValue
	{
		get
		{
			return _PrintTimesMaxValueField;
		}
		set
		{
			_PrintTimesMaxValueField = value;
			RaisePropertyChanged("_PrintTimesMaxValue");
		}
	}

	[XmlElement(Order = 10)]
	public string _Default_TPN
	{
		get
		{
			return _Default_TPNField;
		}
		set
		{
			_Default_TPNField = value;
			RaisePropertyChanged("_Default_TPN");
		}
	}

	[XmlElement(Order = 11)]
	public string _Fetched_TPN
	{
		get
		{
			return _Fetched_TPNField;
		}
		set
		{
			_Fetched_TPNField = value;
			RaisePropertyChanged("_Fetched_TPN");
		}
	}

	[XmlElement(Order = 12)]
	public string _Is_Using_Default_TPN
	{
		get
		{
			return _Is_Using_Default_TPNField;
		}
		set
		{
			_Is_Using_Default_TPNField = value;
			RaisePropertyChanged("_Is_Using_Default_TPN");
		}
	}

	[XmlElement(Order = 13)]
	public string _Fetched_ShuntR
	{
		get
		{
			return _Fetched_ShuntRField;
		}
		set
		{
			_Fetched_ShuntRField = value;
			RaisePropertyChanged("_Fetched_ShuntR");
		}
	}

	[XmlElement(Order = 14)]
	public string _Fetched_PrintTimes
	{
		get
		{
			return _Fetched_PrintTimesField;
		}
		set
		{
			_Fetched_PrintTimesField = value;
			RaisePropertyChanged("_Fetched_PrintTimes");
		}
	}

	[XmlElement(Order = 15)]
	public string _Fetched_Tail_Seq_No
	{
		get
		{
			return _Fetched_Tail_Seq_NoField;
		}
		set
		{
			_Fetched_Tail_Seq_NoField = value;
			RaisePropertyChanged("_Fetched_Tail_Seq_No");
		}
	}
}
