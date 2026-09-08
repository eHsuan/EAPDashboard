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
public class WOComponentLabel : INotifyPropertyChanged
{
	private string _LabelIDField;

	private string _SerialNoField;

	private string _AreaField;

	private string _WONoField;

	private string _BatchNoField;

	private string _RePrtField;

	private string _StatusField;

	private string _RefLabelIDField;

	private string _RemarkField;

	private string _LPStatusField;

	private string _LPRemarkField;

	private DateTime? _LPStartTimeField;

	private DateTime? _LPEndTimeField;

	private string _TCStatusField;

	private string _TCRemarkField;

	private DateTime? _TCStartTimeField;

	private DateTime? _TCEndTimeField;

	private string _EQAStatusField;

	private string _EQARemarkField;

	private DateTime? _EQAStartTimeField;

	private DateTime? _EQAEndTimeField;

	private DateTime _CreateDateField;

	private string _CreateUserField;

	private DateTime? _UpdateDateField;

	private string _UpdateUserField;

	private string _SAPStatusField;

	private string _SAPRemarkField;

	private string _LabelContent_QRField;

	private string _LabelContent_L1Field;

	private string _LabelContent_L2Field;

	private string _LabelContent_L3Field;

	private string _LabelContent_L4Field;

	private string _LabelContent_L5Field;

	private string _LabelContent_L6Field;

	private string _IsBySerinalNoLastRecordField;

	[XmlElement(Order = 0)]
	public string _LabelID
	{
		get
		{
			return _LabelIDField;
		}
		set
		{
			_LabelIDField = value;
			RaisePropertyChanged("_LabelID");
		}
	}

	[XmlElement(Order = 1)]
	public string _SerialNo
	{
		get
		{
			return _SerialNoField;
		}
		set
		{
			_SerialNoField = value;
			RaisePropertyChanged("_SerialNo");
		}
	}

	[XmlElement(Order = 2)]
	public string _Area
	{
		get
		{
			return _AreaField;
		}
		set
		{
			_AreaField = value;
			RaisePropertyChanged("_Area");
		}
	}

	[XmlElement(Order = 3)]
	public string _WONo
	{
		get
		{
			return _WONoField;
		}
		set
		{
			_WONoField = value;
			RaisePropertyChanged("_WONo");
		}
	}

	[XmlElement(Order = 4)]
	public string _BatchNo
	{
		get
		{
			return _BatchNoField;
		}
		set
		{
			_BatchNoField = value;
			RaisePropertyChanged("_BatchNo");
		}
	}

	[XmlElement(Order = 5)]
	public string _RePrt
	{
		get
		{
			return _RePrtField;
		}
		set
		{
			_RePrtField = value;
			RaisePropertyChanged("_RePrt");
		}
	}

	[XmlElement(Order = 6)]
	public string _Status
	{
		get
		{
			return _StatusField;
		}
		set
		{
			_StatusField = value;
			RaisePropertyChanged("_Status");
		}
	}

	[XmlElement(Order = 7)]
	public string _RefLabelID
	{
		get
		{
			return _RefLabelIDField;
		}
		set
		{
			_RefLabelIDField = value;
			RaisePropertyChanged("_RefLabelID");
		}
	}

	[XmlElement(Order = 8)]
	public string _Remark
	{
		get
		{
			return _RemarkField;
		}
		set
		{
			_RemarkField = value;
			RaisePropertyChanged("_Remark");
		}
	}

	[XmlElement(Order = 9)]
	public string _LPStatus
	{
		get
		{
			return _LPStatusField;
		}
		set
		{
			_LPStatusField = value;
			RaisePropertyChanged("_LPStatus");
		}
	}

	[XmlElement(Order = 10)]
	public string _LPRemark
	{
		get
		{
			return _LPRemarkField;
		}
		set
		{
			_LPRemarkField = value;
			RaisePropertyChanged("_LPRemark");
		}
	}

	[XmlElement(IsNullable = true, Order = 11)]
	public DateTime? _LPStartTime
	{
		get
		{
			return _LPStartTimeField;
		}
		set
		{
			_LPStartTimeField = value;
			RaisePropertyChanged("_LPStartTime");
		}
	}

	[XmlElement(IsNullable = true, Order = 12)]
	public DateTime? _LPEndTime
	{
		get
		{
			return _LPEndTimeField;
		}
		set
		{
			_LPEndTimeField = value;
			RaisePropertyChanged("_LPEndTime");
		}
	}

	[XmlElement(Order = 13)]
	public string _TCStatus
	{
		get
		{
			return _TCStatusField;
		}
		set
		{
			_TCStatusField = value;
			RaisePropertyChanged("_TCStatus");
		}
	}

	[XmlElement(Order = 14)]
	public string _TCRemark
	{
		get
		{
			return _TCRemarkField;
		}
		set
		{
			_TCRemarkField = value;
			RaisePropertyChanged("_TCRemark");
		}
	}

	[XmlElement(IsNullable = true, Order = 15)]
	public DateTime? _TCStartTime
	{
		get
		{
			return _TCStartTimeField;
		}
		set
		{
			_TCStartTimeField = value;
			RaisePropertyChanged("_TCStartTime");
		}
	}

	[XmlElement(IsNullable = true, Order = 16)]
	public DateTime? _TCEndTime
	{
		get
		{
			return _TCEndTimeField;
		}
		set
		{
			_TCEndTimeField = value;
			RaisePropertyChanged("_TCEndTime");
		}
	}

	[XmlElement(Order = 17)]
	public string _EQAStatus
	{
		get
		{
			return _EQAStatusField;
		}
		set
		{
			_EQAStatusField = value;
			RaisePropertyChanged("_EQAStatus");
		}
	}

	[XmlElement(Order = 18)]
	public string _EQARemark
	{
		get
		{
			return _EQARemarkField;
		}
		set
		{
			_EQARemarkField = value;
			RaisePropertyChanged("_EQARemark");
		}
	}

	[XmlElement(IsNullable = true, Order = 19)]
	public DateTime? _EQAStartTime
	{
		get
		{
			return _EQAStartTimeField;
		}
		set
		{
			_EQAStartTimeField = value;
			RaisePropertyChanged("_EQAStartTime");
		}
	}

	[XmlElement(IsNullable = true, Order = 20)]
	public DateTime? _EQAEndTime
	{
		get
		{
			return _EQAEndTimeField;
		}
		set
		{
			_EQAEndTimeField = value;
			RaisePropertyChanged("_EQAEndTime");
		}
	}

	[XmlElement(Order = 21)]
	public DateTime _CreateDate
	{
		get
		{
			return _CreateDateField;
		}
		set
		{
			_CreateDateField = value;
			RaisePropertyChanged("_CreateDate");
		}
	}

	[XmlElement(Order = 22)]
	public string _CreateUser
	{
		get
		{
			return _CreateUserField;
		}
		set
		{
			_CreateUserField = value;
			RaisePropertyChanged("_CreateUser");
		}
	}

	[XmlElement(IsNullable = true, Order = 23)]
	public DateTime? _UpdateDate
	{
		get
		{
			return _UpdateDateField;
		}
		set
		{
			_UpdateDateField = value;
			RaisePropertyChanged("_UpdateDate");
		}
	}

	[XmlElement(Order = 24)]
	public string _UpdateUser
	{
		get
		{
			return _UpdateUserField;
		}
		set
		{
			_UpdateUserField = value;
			RaisePropertyChanged("_UpdateUser");
		}
	}

	[XmlElement(Order = 25)]
	public string _SAPStatus
	{
		get
		{
			return _SAPStatusField;
		}
		set
		{
			_SAPStatusField = value;
			RaisePropertyChanged("_SAPStatus");
		}
	}

	[XmlElement(Order = 26)]
	public string _SAPRemark
	{
		get
		{
			return _SAPRemarkField;
		}
		set
		{
			_SAPRemarkField = value;
			RaisePropertyChanged("_SAPRemark");
		}
	}

	[XmlElement(Order = 27)]
	public string _LabelContent_QR
	{
		get
		{
			return _LabelContent_QRField;
		}
		set
		{
			_LabelContent_QRField = value;
			RaisePropertyChanged("_LabelContent_QR");
		}
	}

	[XmlElement(Order = 28)]
	public string _LabelContent_L1
	{
		get
		{
			return _LabelContent_L1Field;
		}
		set
		{
			_LabelContent_L1Field = value;
			RaisePropertyChanged("_LabelContent_L1");
		}
	}

	[XmlElement(Order = 29)]
	public string _LabelContent_L2
	{
		get
		{
			return _LabelContent_L2Field;
		}
		set
		{
			_LabelContent_L2Field = value;
			RaisePropertyChanged("_LabelContent_L2");
		}
	}

	[XmlElement(Order = 30)]
	public string _LabelContent_L3
	{
		get
		{
			return _LabelContent_L3Field;
		}
		set
		{
			_LabelContent_L3Field = value;
			RaisePropertyChanged("_LabelContent_L3");
		}
	}

	[XmlElement(Order = 31)]
	public string _LabelContent_L4
	{
		get
		{
			return _LabelContent_L4Field;
		}
		set
		{
			_LabelContent_L4Field = value;
			RaisePropertyChanged("_LabelContent_L4");
		}
	}

	[XmlElement(Order = 32)]
	public string _LabelContent_L5
	{
		get
		{
			return _LabelContent_L5Field;
		}
		set
		{
			_LabelContent_L5Field = value;
			RaisePropertyChanged("_LabelContent_L5");
		}
	}

	[XmlElement(Order = 33)]
	public string _LabelContent_L6
	{
		get
		{
			return _LabelContent_L6Field;
		}
		set
		{
			_LabelContent_L6Field = value;
			RaisePropertyChanged("_LabelContent_L6");
		}
	}

	[XmlElement(Order = 34)]
	public string _IsBySerinalNoLastRecord
	{
		get
		{
			return _IsBySerinalNoLastRecordField;
		}
		set
		{
			_IsBySerinalNoLastRecordField = value;
			RaisePropertyChanged("_IsBySerinalNoLastRecord");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
