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
public class WorkOrder : INotifyPropertyChanged
{
	private string wONoField;

	private string plantNoField;

	private string matGroupNoField;

	private string matNoField;

	private string orderTypeField;

	private string productStageCodeField;

	private string matStageCodeField;

	private string priorityField;

	private string customerCodeField;

	private DateTime customerDeliveryTimeField;

	private string masterWONoField;

	private string routeNoField;

	private decimal totalQtyField;

	private decimal deliverQtyField;

	private decimal wIPQtyField;

	private string lineCodeField;

	private string batchNoField;

	private int currentFlowIDField;

	private string currentWorkCenterField;

	private DateTime startDateField;

	private DateTime endDateField;

	private DateTime deliverDateField;

	private DateTime processEndDateField;

	private string blockedField;

	private string deletedField;

	private string tECOField;

	private string releasedField;

	private string scrapedField;

	private string urgentField;

	private string splitCodeField;

	private string needMergeField;

	private string wOStatusCodeField;

	private DateTime closeDateField;

	private string closeCodeField;

	private string closeReasonField;

	private string reworkReasonField;

	private string remarkField;

	private decimal sFC1010Field;

	private string remark2Field;

	private string remark3Field;

	private string sortStringField;

	private DateTime lastConfirmDateField;

	private string traceCodeField;

	private string labelModeField;

	private string createUserField;

	private DateTime createDateField;

	private string changeUserField;

	private DateTime changeDateField;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
	public string PlantNo
	{
		get
		{
			return plantNoField;
		}
		set
		{
			plantNoField = value;
			RaisePropertyChanged("PlantNo");
		}
	}

	[XmlElement(Order = 2)]
	public string MatGroupNo
	{
		get
		{
			return matGroupNoField;
		}
		set
		{
			matGroupNoField = value;
			RaisePropertyChanged("MatGroupNo");
		}
	}

	[XmlElement(Order = 3)]
	public string MatNo
	{
		get
		{
			return matNoField;
		}
		set
		{
			matNoField = value;
			RaisePropertyChanged("MatNo");
		}
	}

	[XmlElement(Order = 4)]
	public string OrderType
	{
		get
		{
			return orderTypeField;
		}
		set
		{
			orderTypeField = value;
			RaisePropertyChanged("OrderType");
		}
	}

	[XmlElement(Order = 5)]
	public string ProductStageCode
	{
		get
		{
			return productStageCodeField;
		}
		set
		{
			productStageCodeField = value;
			RaisePropertyChanged("ProductStageCode");
		}
	}

	[XmlElement(Order = 6)]
	public string MatStageCode
	{
		get
		{
			return matStageCodeField;
		}
		set
		{
			matStageCodeField = value;
			RaisePropertyChanged("MatStageCode");
		}
	}

	[XmlElement(Order = 7)]
	public string Priority
	{
		get
		{
			return priorityField;
		}
		set
		{
			priorityField = value;
			RaisePropertyChanged("Priority");
		}
	}

	[XmlElement(Order = 8)]
	public string CustomerCode
	{
		get
		{
			return customerCodeField;
		}
		set
		{
			customerCodeField = value;
			RaisePropertyChanged("CustomerCode");
		}
	}

	[XmlElement(Order = 9)]
	public DateTime CustomerDeliveryTime
	{
		get
		{
			return customerDeliveryTimeField;
		}
		set
		{
			customerDeliveryTimeField = value;
			RaisePropertyChanged("CustomerDeliveryTime");
		}
	}

	[XmlElement(Order = 10)]
	public string MasterWONo
	{
		get
		{
			return masterWONoField;
		}
		set
		{
			masterWONoField = value;
			RaisePropertyChanged("MasterWONo");
		}
	}

	[XmlElement(Order = 11)]
	public string RouteNo
	{
		get
		{
			return routeNoField;
		}
		set
		{
			routeNoField = value;
			RaisePropertyChanged("RouteNo");
		}
	}

	[XmlElement(Order = 12)]
	public decimal TotalQty
	{
		get
		{
			return totalQtyField;
		}
		set
		{
			totalQtyField = value;
			RaisePropertyChanged("TotalQty");
		}
	}

	[XmlElement(Order = 13)]
	public decimal DeliverQty
	{
		get
		{
			return deliverQtyField;
		}
		set
		{
			deliverQtyField = value;
			RaisePropertyChanged("DeliverQty");
		}
	}

	[XmlElement(Order = 14)]
	public decimal WIPQty
	{
		get
		{
			return wIPQtyField;
		}
		set
		{
			wIPQtyField = value;
			RaisePropertyChanged("WIPQty");
		}
	}

	[XmlElement(Order = 15)]
	public string LineCode
	{
		get
		{
			return lineCodeField;
		}
		set
		{
			lineCodeField = value;
			RaisePropertyChanged("LineCode");
		}
	}

	[XmlElement(Order = 16)]
	public string BatchNo
	{
		get
		{
			return batchNoField;
		}
		set
		{
			batchNoField = value;
			RaisePropertyChanged("BatchNo");
		}
	}

	[XmlElement(Order = 17)]
	public int CurrentFlowID
	{
		get
		{
			return currentFlowIDField;
		}
		set
		{
			currentFlowIDField = value;
			RaisePropertyChanged("CurrentFlowID");
		}
	}

	[XmlElement(Order = 18)]
	public string CurrentWorkCenter
	{
		get
		{
			return currentWorkCenterField;
		}
		set
		{
			currentWorkCenterField = value;
			RaisePropertyChanged("CurrentWorkCenter");
		}
	}

	[XmlElement(Order = 19)]
	public DateTime StartDate
	{
		get
		{
			return startDateField;
		}
		set
		{
			startDateField = value;
			RaisePropertyChanged("StartDate");
		}
	}

	[XmlElement(Order = 20)]
	public DateTime EndDate
	{
		get
		{
			return endDateField;
		}
		set
		{
			endDateField = value;
			RaisePropertyChanged("EndDate");
		}
	}

	[XmlElement(Order = 21)]
	public DateTime DeliverDate
	{
		get
		{
			return deliverDateField;
		}
		set
		{
			deliverDateField = value;
			RaisePropertyChanged("DeliverDate");
		}
	}

	[XmlElement(Order = 22)]
	public DateTime ProcessEndDate
	{
		get
		{
			return processEndDateField;
		}
		set
		{
			processEndDateField = value;
			RaisePropertyChanged("ProcessEndDate");
		}
	}

	[XmlElement(Order = 23)]
	public string Blocked
	{
		get
		{
			return blockedField;
		}
		set
		{
			blockedField = value;
			RaisePropertyChanged("Blocked");
		}
	}

	[XmlElement(Order = 24)]
	public string Deleted
	{
		get
		{
			return deletedField;
		}
		set
		{
			deletedField = value;
			RaisePropertyChanged("Deleted");
		}
	}

	[XmlElement(Order = 25)]
	public string TECO
	{
		get
		{
			return tECOField;
		}
		set
		{
			tECOField = value;
			RaisePropertyChanged("TECO");
		}
	}

	[XmlElement(Order = 26)]
	public string Released
	{
		get
		{
			return releasedField;
		}
		set
		{
			releasedField = value;
			RaisePropertyChanged("Released");
		}
	}

	[XmlElement(Order = 27)]
	public string Scraped
	{
		get
		{
			return scrapedField;
		}
		set
		{
			scrapedField = value;
			RaisePropertyChanged("Scraped");
		}
	}

	[XmlElement(Order = 28)]
	public string Urgent
	{
		get
		{
			return urgentField;
		}
		set
		{
			urgentField = value;
			RaisePropertyChanged("Urgent");
		}
	}

	[XmlElement(Order = 29)]
	public string SplitCode
	{
		get
		{
			return splitCodeField;
		}
		set
		{
			splitCodeField = value;
			RaisePropertyChanged("SplitCode");
		}
	}

	[XmlElement(Order = 30)]
	public string NeedMerge
	{
		get
		{
			return needMergeField;
		}
		set
		{
			needMergeField = value;
			RaisePropertyChanged("NeedMerge");
		}
	}

	[XmlElement(Order = 31)]
	public string WOStatusCode
	{
		get
		{
			return wOStatusCodeField;
		}
		set
		{
			wOStatusCodeField = value;
			RaisePropertyChanged("WOStatusCode");
		}
	}

	[XmlElement(Order = 32)]
	public DateTime CloseDate
	{
		get
		{
			return closeDateField;
		}
		set
		{
			closeDateField = value;
			RaisePropertyChanged("CloseDate");
		}
	}

	[XmlElement(Order = 33)]
	public string CloseCode
	{
		get
		{
			return closeCodeField;
		}
		set
		{
			closeCodeField = value;
			RaisePropertyChanged("CloseCode");
		}
	}

	[XmlElement(Order = 34)]
	public string CloseReason
	{
		get
		{
			return closeReasonField;
		}
		set
		{
			closeReasonField = value;
			RaisePropertyChanged("CloseReason");
		}
	}

	[XmlElement(Order = 35)]
	public string ReworkReason
	{
		get
		{
			return reworkReasonField;
		}
		set
		{
			reworkReasonField = value;
			RaisePropertyChanged("ReworkReason");
		}
	}

	[XmlElement(Order = 36)]
	public string Remark
	{
		get
		{
			return remarkField;
		}
		set
		{
			remarkField = value;
			RaisePropertyChanged("Remark");
		}
	}

	[XmlElement(Order = 37)]
	public decimal SFC1010
	{
		get
		{
			return sFC1010Field;
		}
		set
		{
			sFC1010Field = value;
			RaisePropertyChanged("SFC1010");
		}
	}

	[XmlElement(Order = 38)]
	public string Remark2
	{
		get
		{
			return remark2Field;
		}
		set
		{
			remark2Field = value;
			RaisePropertyChanged("Remark2");
		}
	}

	[XmlElement(Order = 39)]
	public string Remark3
	{
		get
		{
			return remark3Field;
		}
		set
		{
			remark3Field = value;
			RaisePropertyChanged("Remark3");
		}
	}

	[XmlElement(Order = 40)]
	public string SortString
	{
		get
		{
			return sortStringField;
		}
		set
		{
			sortStringField = value;
			RaisePropertyChanged("SortString");
		}
	}

	[XmlElement(Order = 41)]
	public DateTime LastConfirmDate
	{
		get
		{
			return lastConfirmDateField;
		}
		set
		{
			lastConfirmDateField = value;
			RaisePropertyChanged("LastConfirmDate");
		}
	}

	[XmlElement(Order = 42)]
	public string TraceCode
	{
		get
		{
			return traceCodeField;
		}
		set
		{
			traceCodeField = value;
			RaisePropertyChanged("TraceCode");
		}
	}

	[XmlElement(Order = 43)]
	public string LabelMode
	{
		get
		{
			return labelModeField;
		}
		set
		{
			labelModeField = value;
			RaisePropertyChanged("LabelMode");
		}
	}

	[XmlElement(Order = 44)]
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

	[XmlElement(Order = 45)]
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

	[XmlElement(Order = 46)]
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

	[XmlElement(Order = 47)]
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
