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
public class Result_WOComponentQry : Basic_Result
{
	private string _woNoField;

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

	private int? _machIDField;

	private string _machAliasField;

	private bool _isFirstCheckinField;

	private bool _isLastCheckOutField;

	private string _woComponentNoConfirmIDField;

	private WO_MachRecord[] _listWOMachRecordField;

	private ParamSet[] _listParamSetField;

	private WOComponent _woComponentField;

	private WOComponentConfirm _woComponentConfirmField;

	[XmlElement(Order = 0)]
	public string _woNo
	{
		get
		{
			return _woNoField;
		}
		set
		{
			_woNoField = value;
			RaisePropertyChanged("_woNo");
		}
	}

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
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

	[XmlElement(Order = 3)]
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
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

	[XmlElement(Order = 7)]
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

	[XmlElement(Order = 8)]
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

	[XmlElement(IsNullable = true, Order = 9)]
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

	[XmlElement(IsNullable = true, Order = 10)]
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

	[XmlElement(IsNullable = true, Order = 11)]
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

	[XmlElement(Order = 12)]
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

	[XmlElement(IsNullable = true, Order = 13)]
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

	[XmlElement(IsNullable = true, Order = 14)]
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

	[XmlElement(Order = 15)]
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

	[XmlElement(IsNullable = true, Order = 16)]
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

	[XmlElement(Order = 17)]
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

	[XmlElement(Order = 18)]
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

	[XmlElement(IsNullable = true, Order = 19)]
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

	[XmlElement(Order = 20)]
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

	[XmlElement(Order = 21)]
	public bool _isFirstCheckin
	{
		get
		{
			return _isFirstCheckinField;
		}
		set
		{
			_isFirstCheckinField = value;
			RaisePropertyChanged("_isFirstCheckin");
		}
	}

	[XmlElement(Order = 22)]
	public bool _isLastCheckOut
	{
		get
		{
			return _isLastCheckOutField;
		}
		set
		{
			_isLastCheckOutField = value;
			RaisePropertyChanged("_isLastCheckOut");
		}
	}

	[XmlElement(Order = 23)]
	public string _woComponentNoConfirmID
	{
		get
		{
			return _woComponentNoConfirmIDField;
		}
		set
		{
			_woComponentNoConfirmIDField = value;
			RaisePropertyChanged("_woComponentNoConfirmID");
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

	[XmlElement(Order = 26)]
	public WOComponent _woComponent
	{
		get
		{
			return _woComponentField;
		}
		set
		{
			_woComponentField = value;
			RaisePropertyChanged("_woComponent");
		}
	}

	[XmlElement(Order = 27)]
	public WOComponentConfirm _woComponentConfirm
	{
		get
		{
			return _woComponentConfirmField;
		}
		set
		{
			_woComponentConfirmField = value;
			RaisePropertyChanged("_woComponentConfirm");
		}
	}
}
