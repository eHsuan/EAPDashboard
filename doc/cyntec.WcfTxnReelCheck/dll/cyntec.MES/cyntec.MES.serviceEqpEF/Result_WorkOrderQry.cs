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
public class Result_WorkOrderQry : Basic_Result
{
	private string _woNoField;

	private string _orderTypeField;

	private string _matGroupNoField;

	private string _lineCodeField;

	private string _matNoField;

	private decimal _wIPQtyField;

	private string _currentWorkCenterNoField;

	private string _currentWorkCenterNameField;

	private int? _currentFlowIDField;

	private int? _currentConfirmIDField;

	private DateTime? _createDateField;

	private string _matClassCodeField;

	private int? _wToPCSField;

	private int? _sToPCSField;

	private string _plantNoField;

	private string _routeNoField;

	private bool _alreadyInFlagField;

	private string _IsComponentField;

	private string _IsComponentBatchInField;

	private string _IsComponentBatchOutField;

	private string _IsPackingField;

	private string _IsTapingField;

	private string _IsLabelField;

	private int _UnitQtyField;

	private decimal _LessQtyField;

	private decimal _MakeUpQtyField;

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
	public string _orderType
	{
		get
		{
			return _orderTypeField;
		}
		set
		{
			_orderTypeField = value;
			RaisePropertyChanged("_orderType");
		}
	}

	[XmlElement(Order = 2)]
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

	[XmlElement(Order = 3)]
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
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
	public int? _currentConfirmID
	{
		get
		{
			return _currentConfirmIDField;
		}
		set
		{
			_currentConfirmIDField = value;
			RaisePropertyChanged("_currentConfirmID");
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

	[XmlElement(Order = 15)]
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

	[XmlElement(Order = 16)]
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

	[XmlElement(Order = 17)]
	public string _IsComponent
	{
		get
		{
			return _IsComponentField;
		}
		set
		{
			_IsComponentField = value;
			RaisePropertyChanged("_IsComponent");
		}
	}

	[XmlElement(Order = 18)]
	public string _IsComponentBatchIn
	{
		get
		{
			return _IsComponentBatchInField;
		}
		set
		{
			_IsComponentBatchInField = value;
			RaisePropertyChanged("_IsComponentBatchIn");
		}
	}

	[XmlElement(Order = 19)]
	public string _IsComponentBatchOut
	{
		get
		{
			return _IsComponentBatchOutField;
		}
		set
		{
			_IsComponentBatchOutField = value;
			RaisePropertyChanged("_IsComponentBatchOut");
		}
	}

	[XmlElement(Order = 20)]
	public string _IsPacking
	{
		get
		{
			return _IsPackingField;
		}
		set
		{
			_IsPackingField = value;
			RaisePropertyChanged("_IsPacking");
		}
	}

	[XmlElement(Order = 21)]
	public string _IsTaping
	{
		get
		{
			return _IsTapingField;
		}
		set
		{
			_IsTapingField = value;
			RaisePropertyChanged("_IsTaping");
		}
	}

	[XmlElement(Order = 22)]
	public string _IsLabel
	{
		get
		{
			return _IsLabelField;
		}
		set
		{
			_IsLabelField = value;
			RaisePropertyChanged("_IsLabel");
		}
	}

	[XmlElement(Order = 23)]
	public int _UnitQty
	{
		get
		{
			return _UnitQtyField;
		}
		set
		{
			_UnitQtyField = value;
			RaisePropertyChanged("_UnitQty");
		}
	}

	[XmlElement(Order = 24)]
	public decimal _LessQty
	{
		get
		{
			return _LessQtyField;
		}
		set
		{
			_LessQtyField = value;
			RaisePropertyChanged("_LessQty");
		}
	}

	[XmlElement(Order = 25)]
	public decimal _MakeUpQty
	{
		get
		{
			return _MakeUpQtyField;
		}
		set
		{
			_MakeUpQtyField = value;
			RaisePropertyChanged("_MakeUpQty");
		}
	}
}
