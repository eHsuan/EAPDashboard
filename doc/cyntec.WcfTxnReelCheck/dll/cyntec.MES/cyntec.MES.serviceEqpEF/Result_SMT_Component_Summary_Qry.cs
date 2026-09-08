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
public class Result_SMT_Component_Summary_Qry : Basic_Result
{
	private decimal wO_WIPQtyField;

	private decimal wO_Component_Status_OK_CountField;

	private decimal wO_Component_Status_NG_CountField;

	private decimal wO_Sum_Comp_Status_OK_QtyField;

	private decimal wO_Sum_Comp_Status_NG_QtyField;

	private decimal wO_Sum_Comp_ConfirmOut_QtyField;

	private string[] wOComponentMissingConfirmOutListField;

	private SMT_WO_NGItem[] wONgSummaryListField;

	private WOComponent[] wOComponentListField;

	private WOComponentConfirm[] wOComponentConfirmListField;

	private WOComponentConfirmNGRecord[] wOComponentConfirmNGRecordListField;

	[XmlElement(Order = 0)]
	public decimal WO_WIPQty
	{
		get
		{
			return wO_WIPQtyField;
		}
		set
		{
			wO_WIPQtyField = value;
			RaisePropertyChanged("WO_WIPQty");
		}
	}

	[XmlElement(Order = 1)]
	public decimal WO_Component_Status_OK_Count
	{
		get
		{
			return wO_Component_Status_OK_CountField;
		}
		set
		{
			wO_Component_Status_OK_CountField = value;
			RaisePropertyChanged("WO_Component_Status_OK_Count");
		}
	}

	[XmlElement(Order = 2)]
	public decimal WO_Component_Status_NG_Count
	{
		get
		{
			return wO_Component_Status_NG_CountField;
		}
		set
		{
			wO_Component_Status_NG_CountField = value;
			RaisePropertyChanged("WO_Component_Status_NG_Count");
		}
	}

	[XmlElement(Order = 3)]
	public decimal WO_Sum_Comp_Status_OK_Qty
	{
		get
		{
			return wO_Sum_Comp_Status_OK_QtyField;
		}
		set
		{
			wO_Sum_Comp_Status_OK_QtyField = value;
			RaisePropertyChanged("WO_Sum_Comp_Status_OK_Qty");
		}
	}

	[XmlElement(Order = 4)]
	public decimal WO_Sum_Comp_Status_NG_Qty
	{
		get
		{
			return wO_Sum_Comp_Status_NG_QtyField;
		}
		set
		{
			wO_Sum_Comp_Status_NG_QtyField = value;
			RaisePropertyChanged("WO_Sum_Comp_Status_NG_Qty");
		}
	}

	[XmlElement(Order = 5)]
	public decimal WO_Sum_Comp_ConfirmOut_Qty
	{
		get
		{
			return wO_Sum_Comp_ConfirmOut_QtyField;
		}
		set
		{
			wO_Sum_Comp_ConfirmOut_QtyField = value;
			RaisePropertyChanged("WO_Sum_Comp_ConfirmOut_Qty");
		}
	}

	[XmlArray(Order = 6)]
	public string[] WOComponentMissingConfirmOutList
	{
		get
		{
			return wOComponentMissingConfirmOutListField;
		}
		set
		{
			wOComponentMissingConfirmOutListField = value;
			RaisePropertyChanged("WOComponentMissingConfirmOutList");
		}
	}

	[XmlArray(Order = 7)]
	public SMT_WO_NGItem[] WONgSummaryList
	{
		get
		{
			return wONgSummaryListField;
		}
		set
		{
			wONgSummaryListField = value;
			RaisePropertyChanged("WONgSummaryList");
		}
	}

	[XmlArray(Order = 8)]
	public WOComponent[] WOComponentList
	{
		get
		{
			return wOComponentListField;
		}
		set
		{
			wOComponentListField = value;
			RaisePropertyChanged("WOComponentList");
		}
	}

	[XmlArray(Order = 9)]
	public WOComponentConfirm[] WOComponentConfirmList
	{
		get
		{
			return wOComponentConfirmListField;
		}
		set
		{
			wOComponentConfirmListField = value;
			RaisePropertyChanged("WOComponentConfirmList");
		}
	}

	[XmlArray(Order = 10)]
	public WOComponentConfirmNGRecord[] WOComponentConfirmNGRecordList
	{
		get
		{
			return wOComponentConfirmNGRecordListField;
		}
		set
		{
			wOComponentConfirmNGRecordListField = value;
			RaisePropertyChanged("WOComponentConfirmNGRecordList");
		}
	}
}
