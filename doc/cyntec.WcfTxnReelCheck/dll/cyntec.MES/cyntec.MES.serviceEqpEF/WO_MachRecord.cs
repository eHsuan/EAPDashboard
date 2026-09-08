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
public class WO_MachRecord : INotifyPropertyChanged
{
	private int idField;

	private int machIDField;

	private int flowIDField;

	private string wONoField;

	private string machineNoField;

	private string lineCodeField;

	private string plantNoField;

	private string matGroupNoField;

	private string workCenterNoField;

	private string confirmWorkCenterField;

	private string outPutQtyField;

	private string operatorField;

	private string iPQCField;

	private string createUserField;

	private DateTime createDateField;

	[XmlElement(Order = 0)]
	public int ID
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
	public int MachID
	{
		get
		{
			return machIDField;
		}
		set
		{
			machIDField = value;
			RaisePropertyChanged("MachID");
		}
	}

	[XmlElement(Order = 2)]
	public int FlowID
	{
		get
		{
			return flowIDField;
		}
		set
		{
			flowIDField = value;
			RaisePropertyChanged("FlowID");
		}
	}

	[XmlElement(Order = 3)]
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

	[XmlElement(Order = 4)]
	public string MachineNo
	{
		get
		{
			return machineNoField;
		}
		set
		{
			machineNoField = value;
			RaisePropertyChanged("MachineNo");
		}
	}

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
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

	[XmlElement(Order = 7)]
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

	[XmlElement(Order = 8)]
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

	[XmlElement(Order = 9)]
	public string ConfirmWorkCenter
	{
		get
		{
			return confirmWorkCenterField;
		}
		set
		{
			confirmWorkCenterField = value;
			RaisePropertyChanged("ConfirmWorkCenter");
		}
	}

	[XmlElement(Order = 10)]
	public string OutPutQty
	{
		get
		{
			return outPutQtyField;
		}
		set
		{
			outPutQtyField = value;
			RaisePropertyChanged("OutPutQty");
		}
	}

	[XmlElement(Order = 11)]
	public string Operator
	{
		get
		{
			return operatorField;
		}
		set
		{
			operatorField = value;
			RaisePropertyChanged("Operator");
		}
	}

	[XmlElement(Order = 12)]
	public string IPQC
	{
		get
		{
			return iPQCField;
		}
		set
		{
			iPQCField = value;
			RaisePropertyChanged("IPQC");
		}
	}

	[XmlElement(Order = 13)]
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

	[XmlElement(Order = 14)]
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
