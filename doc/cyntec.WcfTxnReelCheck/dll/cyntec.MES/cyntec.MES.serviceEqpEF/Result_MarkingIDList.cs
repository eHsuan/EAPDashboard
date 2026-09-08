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
public class Result_MarkingIDList : Basic_Result
{
	private Result_SubIDCell[] _subIDListField;

	private string _marking2DBarCodeKindField;

	private string _targetTypeField;

	private string _markingCodeField;

	private string _markingNameField;

	private string _markingDescField;

	private string _markingSideField;

	private string _markingSideUnitField;

	private string _markingSizeXField;

	private string _markingSizeYField;

	private long _ori_serial_numField;

	private long _new_serial_numField;

	[XmlArray(Order = 0)]
	public Result_SubIDCell[] _subIDList
	{
		get
		{
			return _subIDListField;
		}
		set
		{
			_subIDListField = value;
			RaisePropertyChanged("_subIDList");
		}
	}

	[XmlElement(Order = 1)]
	public string _marking2DBarCodeKind
	{
		get
		{
			return _marking2DBarCodeKindField;
		}
		set
		{
			_marking2DBarCodeKindField = value;
			RaisePropertyChanged("_marking2DBarCodeKind");
		}
	}

	[XmlElement(Order = 2)]
	public string _targetType
	{
		get
		{
			return _targetTypeField;
		}
		set
		{
			_targetTypeField = value;
			RaisePropertyChanged("_targetType");
		}
	}

	[XmlElement(Order = 3)]
	public string _markingCode
	{
		get
		{
			return _markingCodeField;
		}
		set
		{
			_markingCodeField = value;
			RaisePropertyChanged("_markingCode");
		}
	}

	[XmlElement(Order = 4)]
	public string _markingName
	{
		get
		{
			return _markingNameField;
		}
		set
		{
			_markingNameField = value;
			RaisePropertyChanged("_markingName");
		}
	}

	[XmlElement(Order = 5)]
	public string _markingDesc
	{
		get
		{
			return _markingDescField;
		}
		set
		{
			_markingDescField = value;
			RaisePropertyChanged("_markingDesc");
		}
	}

	[XmlElement(Order = 6)]
	public string _markingSide
	{
		get
		{
			return _markingSideField;
		}
		set
		{
			_markingSideField = value;
			RaisePropertyChanged("_markingSide");
		}
	}

	[XmlElement(Order = 7)]
	public string _markingSideUnit
	{
		get
		{
			return _markingSideUnitField;
		}
		set
		{
			_markingSideUnitField = value;
			RaisePropertyChanged("_markingSideUnit");
		}
	}

	[XmlElement(Order = 8)]
	public string _markingSizeX
	{
		get
		{
			return _markingSizeXField;
		}
		set
		{
			_markingSizeXField = value;
			RaisePropertyChanged("_markingSizeX");
		}
	}

	[XmlElement(Order = 9)]
	public string _markingSizeY
	{
		get
		{
			return _markingSizeYField;
		}
		set
		{
			_markingSizeYField = value;
			RaisePropertyChanged("_markingSizeY");
		}
	}

	[XmlElement(Order = 10)]
	public long _ori_serial_num
	{
		get
		{
			return _ori_serial_numField;
		}
		set
		{
			_ori_serial_numField = value;
			RaisePropertyChanged("_ori_serial_num");
		}
	}

	[XmlElement(Order = 11)]
	public long _new_serial_num
	{
		get
		{
			return _new_serial_numField;
		}
		set
		{
			_new_serial_numField = value;
			RaisePropertyChanged("_new_serial_num");
		}
	}
}
