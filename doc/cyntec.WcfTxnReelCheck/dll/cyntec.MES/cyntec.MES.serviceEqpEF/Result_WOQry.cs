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
public class Result_WOQry : Basic_Result
{
	private string _matGroupNoField;

	private string _lineCodeField;

	private string _matNoField;

	private decimal _wIPQtyField;

	private string _workCenterNoField;

	private string _workCenterNameField;

	private string _currentWorkCenterNoField;

	private string _currentWorkCenterNameField;

	private int? _currentFlowIDField;

	private DateTime? _createDateField;

	private int? _flowIDField;

	private string _matClassCodeField;

	private int? _wToPCSField;

	private int? _sToPCSField;

	private bool _alreadyInFlagField;

	private int? _confirmIDField;

	private string _plantNoField;

	private string _routeNoField;

	private decimal _refInputQtyField;

	private string _batchNoField;

	private int? _machIDField;

	private string _machAliasField;

	private string _woUnitField;

	private string _workCenterNoWOUnitField;

	private WO_MachRecord[] _listWOMachRecordField;

	private NGSet[] _listNGSetField;

	private InputCodeSet[] _listInputCodeSetField;

	private ParamSet[] _listParamSetField;

	private WOComponent[] _listWOComponentField;

	[XmlElement(Order = 0)]
	public string _matGroupNo
	{
		get
		{
			return _matGroupNoField;
		}
		set
		{
			_matGroupNoField = value;
			RaisePropertyChanged("_matGroupNo");
		}
	}

	[XmlElement(Order = 1)]
	public string _lineCode
	{
		get
		{
			return _lineCodeField;
		}
		set
		{
			_lineCodeField = value;
			RaisePropertyChanged("_lineCode");
		}
	}

	[XmlElement(Order = 2)]
	public string _matNo
	{
		get
		{
			return _matNoField;
		}
		set
		{
			_matNoField = value;
			RaisePropertyChanged("_matNo");
		}
	}

	[XmlElement(Order = 3)]
	public decimal _wIPQty
	{
		get
		{
			return _wIPQtyField;
		}
		set
		{
			_wIPQtyField = value;
			RaisePropertyChanged("_wIPQty");
		}
	}

	[XmlElement(Order = 4)]
	public string _workCenterNo
	{
		get
		{
			return _workCenterNoField;
		}
		set
		{
			_workCenterNoField = value;
			RaisePropertyChanged("_workCenterNo");
		}
	}

	[XmlElement(Order = 5)]
	public string _workCenterName
	{
		get
		{
			return _workCenterNameField;
		}
		set
		{
			_workCenterNameField = value;
			RaisePropertyChanged("_workCenterName");
		}
	}

	[XmlElement(Order = 6)]
	public string _currentWorkCenterNo
	{
		get
		{
			return _currentWorkCenterNoField;
		}
		set
		{
			_currentWorkCenterNoField = value;
			RaisePropertyChanged("_currentWorkCenterNo");
		}
	}

	[XmlElement(Order = 7)]
	public string _currentWorkCenterName
	{
		get
		{
			return _currentWorkCenterNameField;
		}
		set
		{
			_currentWorkCenterNameField = value;
			RaisePropertyChanged("_currentWorkCenterName");
		}
	}

	[XmlElement(IsNullable = true, Order = 8)]
	public int? _currentFlowID
	{
		get
		{
			return _currentFlowIDField;
		}
		set
		{
			_currentFlowIDField = value;
			RaisePropertyChanged("_currentFlowID");
		}
	}

	[XmlElement(IsNullable = true, Order = 9)]
	public DateTime? _createDate
	{
		get
		{
			return _createDateField;
		}
		set
		{
			_createDateField = value;
			RaisePropertyChanged("_createDate");
		}
	}

	[XmlElement(IsNullable = true, Order = 10)]
	public int? _flowID
	{
		get
		{
			return _flowIDField;
		}
		set
		{
			_flowIDField = value;
			RaisePropertyChanged("_flowID");
		}
	}

	[XmlElement(Order = 11)]
	public string _matClassCode
	{
		get
		{
			return _matClassCodeField;
		}
		set
		{
			_matClassCodeField = value;
			RaisePropertyChanged("_matClassCode");
		}
	}

	[XmlElement(IsNullable = true, Order = 12)]
	public int? _wToPCS
	{
		get
		{
			return _wToPCSField;
		}
		set
		{
			_wToPCSField = value;
			RaisePropertyChanged("_wToPCS");
		}
	}

	[XmlElement(IsNullable = true, Order = 13)]
	public int? _sToPCS
	{
		get
		{
			return _sToPCSField;
		}
		set
		{
			_sToPCSField = value;
			RaisePropertyChanged("_sToPCS");
		}
	}

	[XmlElement(Order = 14)]
	public bool _alreadyInFlag
	{
		get
		{
			return _alreadyInFlagField;
		}
		set
		{
			_alreadyInFlagField = value;
			RaisePropertyChanged("_alreadyInFlag");
		}
	}

	[XmlElement(IsNullable = true, Order = 15)]
	public int? _confirmID
	{
		get
		{
			return _confirmIDField;
		}
		set
		{
			_confirmIDField = value;
			RaisePropertyChanged("_confirmID");
		}
	}

	[XmlElement(Order = 16)]
	public string _plantNo
	{
		get
		{
			return _plantNoField;
		}
		set
		{
			_plantNoField = value;
			RaisePropertyChanged("_plantNo");
		}
	}

	[XmlElement(Order = 17)]
	public string _routeNo
	{
		get
		{
			return _routeNoField;
		}
		set
		{
			_routeNoField = value;
			RaisePropertyChanged("_routeNo");
		}
	}

	[XmlElement(Order = 18)]
	public decimal _refInputQty
	{
		get
		{
			return _refInputQtyField;
		}
		set
		{
			_refInputQtyField = value;
			RaisePropertyChanged("_refInputQty");
		}
	}

	[XmlElement(Order = 19)]
	public string _batchNo
	{
		get
		{
			return _batchNoField;
		}
		set
		{
			_batchNoField = value;
			RaisePropertyChanged("_batchNo");
		}
	}

	[XmlElement(IsNullable = true, Order = 20)]
	public int? _machID
	{
		get
		{
			return _machIDField;
		}
		set
		{
			_machIDField = value;
			RaisePropertyChanged("_machID");
		}
	}

	[XmlElement(Order = 21)]
	public string _machAlias
	{
		get
		{
			return _machAliasField;
		}
		set
		{
			_machAliasField = value;
			RaisePropertyChanged("_machAlias");
		}
	}

	[XmlElement(Order = 22)]
	public string _woUnit
	{
		get
		{
			return _woUnitField;
		}
		set
		{
			_woUnitField = value;
			RaisePropertyChanged("_woUnit");
		}
	}

	[XmlElement(Order = 23)]
	public string _workCenterNoWOUnit
	{
		get
		{
			return _workCenterNoWOUnitField;
		}
		set
		{
			_workCenterNoWOUnitField = value;
			RaisePropertyChanged("_workCenterNoWOUnit");
		}
	}

	[XmlArray(Order = 24)]
	public WO_MachRecord[] _listWOMachRecord
	{
		get
		{
			return _listWOMachRecordField;
		}
		set
		{
			_listWOMachRecordField = value;
			RaisePropertyChanged("_listWOMachRecord");
		}
	}

	[XmlArray(Order = 25)]
	public NGSet[] _listNGSet
	{
		get
		{
			return _listNGSetField;
		}
		set
		{
			_listNGSetField = value;
			RaisePropertyChanged("_listNGSet");
		}
	}

	[XmlArray(Order = 26)]
	public InputCodeSet[] _listInputCodeSet
	{
		get
		{
			return _listInputCodeSetField;
		}
		set
		{
			_listInputCodeSetField = value;
			RaisePropertyChanged("_listInputCodeSet");
		}
	}

	[XmlArray(Order = 27)]
	public ParamSet[] _listParamSet
	{
		get
		{
			return _listParamSetField;
		}
		set
		{
			_listParamSetField = value;
			RaisePropertyChanged("_listParamSet");
		}
	}

	[XmlArray(Order = 28)]
	public WOComponent[] _listWOComponent
	{
		get
		{
			return _listWOComponentField;
		}
		set
		{
			_listWOComponentField = value;
			RaisePropertyChanged("_listWOComponent");
		}
	}
}
