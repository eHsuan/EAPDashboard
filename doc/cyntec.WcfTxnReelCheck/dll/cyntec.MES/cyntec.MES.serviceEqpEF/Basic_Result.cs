using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpEF;

[Serializable]
[XmlInclude(typeof(Result_ComponentLabel_Rework_WO_Update))]
[XmlInclude(typeof(Result_WOComponentLabel_GetModelList))]
[XmlInclude(typeof(Result_WOComponentLabel_GetModel))]
[XmlInclude(typeof(Result_Component_SetNG))]
[XmlInclude(typeof(Result_Component_Return))]
[XmlInclude(typeof(Result_Component_MakeUp_ChangeBack))]
[XmlInclude(typeof(Result_Component_MakeUp))]
[XmlInclude(typeof(Result_ComponentCheckOut_Less_ChangeBack))]
[XmlInclude(typeof(Result_ComponentCheckOut_Less))]
[XmlInclude(typeof(Result_ComponentCheckOut_NG))]
[XmlInclude(typeof(Result_ComponentCheckIn_QReturn))]
[XmlInclude(typeof(Result_ComponentCheckIn_Cancel))]
[XmlInclude(typeof(Result_ComponentCheckOut_Batch))]
[XmlInclude(typeof(Result_ComponentCheckIn_Batch))]
[XmlInclude(typeof(Result_WO_Component_Add_Web))]
[XmlInclude(typeof(Result_WorkOrderQryWithComponent))]
[XmlInclude(typeof(Result_WorkOrderQry))]
[XmlInclude(typeof(Result_WOComponentQryWithComponent))]
[XmlInclude(typeof(Result_EDCPlanResultGet_FAI))]
[XmlInclude(typeof(Result_EqpDynamicNGRecord_GetModelList))]
[XmlInclude(typeof(Result_DownloadDll))]
[XmlInclude(typeof(Result_DownloadEqpDllFile))]
[XmlInclude(typeof(Result_EqpErrorCode_GetModelList))]
[XmlInclude(typeof(Result_TOS_MouldQry))]
[XmlInclude(typeof(Result_WOGroup))]
[XmlInclude(typeof(Result_WOCheckOut_Batch))]
[XmlInclude(typeof(WOCheckOutItem))]
[XmlInclude(typeof(Result_WOCheckIn_Batch))]
[XmlInclude(typeof(WOCheckItem))]
[XmlInclude(typeof(Result_WOParmQry))]
[XmlInclude(typeof(Result_WOParmQryByParamCode))]
[XmlInclude(typeof(Result_WOParmQryByRefFieldCode))]
[XmlInclude(typeof(Result_WOQry))]
[XmlInclude(typeof(Result_SMT_AOI_WOComponentConfirm))]
[XmlInclude(typeof(Result_SMT_Component_Summary_Qry))]
[XmlInclude(typeof(Result_SMT_WOComponentConfirm))]
[XmlInclude(typeof(Result_SMT_Component_SetNG))]
[XmlInclude(typeof(Result_UserVerify))]
[XmlInclude(typeof(Result_GetDecodeBarcode))]
[XmlInclude(typeof(Result_GetWOReelIDList))]
[XmlInclude(typeof(Result_CSM_TESLA_LableGet))]
[XmlInclude(typeof(Result_MarkingIDList))]
[XmlInclude(typeof(Result_CSM_SAIC_LableGet))]
[XmlInclude(typeof(Result_EdcRawdataGet))]
[XmlInclude(typeof(Result_EdcDataQuery))]
[XmlInclude(typeof(Result_TOS_WOLogGet))]
[XmlInclude(typeof(Result_EqpQry))]
[XmlInclude(typeof(Result_TOS_WOQry))]
[XmlInclude(typeof(Result_UserQry))]
[XmlInclude(typeof(Result_GetTrackingCodePPPP))]
[XmlInclude(typeof(Result_Get_Numbering_Rollback))]
[XmlInclude(typeof(Result_Get_Numbering))]
[XmlInclude(typeof(Result_WorkOrderComponentListQry))]
[XmlInclude(typeof(Result_GetEqpValue))]
[XmlInclude(typeof(Result_WOCheckOutCreateSPCData))]
[XmlInclude(typeof(Result_WOComponentCheckOut))]
[XmlInclude(typeof(Result_WOComponentCheckIn))]
[XmlInclude(typeof(Result_WOComponentQry))]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://CyntecMES")]
public class Basic_Result : INotifyPropertyChanged
{
	private bool _isSuccessField;

	private string _resultCodeField;

	private string _errorMessageField;

	private string _sQLErrorMessageField;

	private string _userIDField;

	private bool _isExistField;

	[XmlElement(Order = 0)]
	public bool _isSuccess
	{
		get
		{
			return _isSuccessField;
		}
		set
		{
			_isSuccessField = value;
			RaisePropertyChanged("_isSuccess");
		}
	}

	[XmlElement(Order = 1)]
	public string _resultCode
	{
		get
		{
			return _resultCodeField;
		}
		set
		{
			_resultCodeField = value;
			RaisePropertyChanged("_resultCode");
		}
	}

	[XmlElement(Order = 2)]
	public string _errorMessage
	{
		get
		{
			return _errorMessageField;
		}
		set
		{
			_errorMessageField = value;
			RaisePropertyChanged("_errorMessage");
		}
	}

	[XmlElement(Order = 3)]
	public string _sQLErrorMessage
	{
		get
		{
			return _sQLErrorMessageField;
		}
		set
		{
			_sQLErrorMessageField = value;
			RaisePropertyChanged("_sQLErrorMessage");
		}
	}

	[XmlElement(Order = 4)]
	public string _userID
	{
		get
		{
			return _userIDField;
		}
		set
		{
			_userIDField = value;
			RaisePropertyChanged("_userID");
		}
	}

	[XmlElement(Order = 5)]
	public bool _isExist
	{
		get
		{
			return _isExistField;
		}
		set
		{
			_isExistField = value;
			RaisePropertyChanged("_isExist");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
