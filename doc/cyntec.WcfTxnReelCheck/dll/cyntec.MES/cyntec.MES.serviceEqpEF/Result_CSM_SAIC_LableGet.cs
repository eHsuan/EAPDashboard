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
public class Result_CSM_SAIC_LableGet : Basic_Result
{
	private string _Line1Field;

	private string _Line2Field;

	private string _Line3Field;

	private string _Line4Field;

	private string _Line5Field;

	private string _Line6Field;

	private string _LineQRField;

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
	public string _Line5
	{
		get
		{
			return _Line5Field;
		}
		set
		{
			_Line5Field = value;
			RaisePropertyChanged("_Line5");
		}
	}

	[XmlElement(Order = 5)]
	public string _Line6
	{
		get
		{
			return _Line6Field;
		}
		set
		{
			_Line6Field = value;
			RaisePropertyChanged("_Line6");
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
}
