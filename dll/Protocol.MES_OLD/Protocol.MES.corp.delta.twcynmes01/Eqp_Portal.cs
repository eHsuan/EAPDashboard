using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using Protocol.MES.Properties;

namespace Protocol.MES.corp.delta.twcynmes01;

[GeneratedCode("System.Web.Services", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[WebServiceBinding(Name = "Eqp_PortalSoap", Namespace = "CyntecMES")]
public class Eqp_Portal : SoapHttpClientProtocol
{
	private SendOrPostCallback EqpTransactionOperationCompleted;

	private SendOrPostCallback InsertEqpErrorLogOperationCompleted;

	private SendOrPostCallback InsertEqpTransactionLogOperationCompleted;

	private SendOrPostCallback EqpStatusRefreshOperationCompleted;

	private SendOrPostCallback InsertParameterOperationCompleted;

	private SendOrPostCallback CheckUserCertOperationCompleted;

	private SendOrPostCallback CheckUserOperationCompleted;

	private SendOrPostCallback CheckWOOperationCompleted;

	private SendOrPostCallback CheckTraceMatOperationCompleted;

	private SendOrPostCallback CheckWOAlreadyCheckInOperationCompleted;

	private SendOrPostCallback WOQryOperationCompleted;

	private SendOrPostCallback WOInCheckOperationCompleted;

	private SendOrPostCallback WO_CheckAlreadyInOperationCompleted;

	private SendOrPostCallback WO_InsertUseMachOperationCompleted;

	private SendOrPostCallback WOCheckInOperationCompleted;

	private SendOrPostCallback WOCheckIn_JustCheckOperationCompleted;

	private SendOrPostCallback WOParmCodeQryOperationCompleted;

	private SendOrPostCallback GetParaOperationCompleted;

	private SendOrPostCallback GetWorkCenterAllParaOperationCompleted;

	private SendOrPostCallback EqpMatAddOperationCompleted;

	private SendOrPostCallback EqpMatAdd_BatchOperationCompleted;

	private SendOrPostCallback EqpMatAdd_Batch_SkipRepeatOperationCompleted;

	private SendOrPostCallback EqpWOInputAddOperationCompleted;

	private SendOrPostCallback EqpWOInputAdd_BatchOperationCompleted;

	private SendOrPostCallback EqpCarrierAddOperationCompleted;

	private SendOrPostCallback EqpCarrierAdd_BatchOperationCompleted;

	private SendOrPostCallback EqpPartProcessOperationCompleted;

	private SendOrPostCallback EqpPartProcess_BatchOperationCompleted;

	private SendOrPostCallback EqpTestValueRecordOperationCompleted;

	private SendOrPostCallback MouldInfoOperationCompleted;

	private SendOrPostCallback MouldInfo_ByIDOperationCompleted;

	private SendOrPostCallback MouldOnlineOperationCompleted;

	private SendOrPostCallback MouldOnlineByIDOperationCompleted;

	private SendOrPostCallback GetMoverRsnOperationCompleted;

	private SendOrPostCallback MouldOffLineOperationCompleted;

	private SendOrPostCallback MouldOffLine_ByIDOperationCompleted;

	private SendOrPostCallback MouldOutputUpdateOperationCompleted;

	private SendOrPostCallback MouldOutputUpdate_ByIDOperationCompleted;

	private SendOrPostCallback MachOnlineMouldQueryOperationCompleted;

	private SendOrPostCallback UploadTestResultFileOperationCompleted;

	private SendOrPostCallback CarrierInOperationCompleted;

	private SendOrPostCallback WoCheckInCarrierCheckOperationCompleted;

	private SendOrPostCallback WoCheckOutCarrierCheckOperationCompleted;

	private SendOrPostCallback GetWONoByToolNoOperationCompleted;

	private SendOrPostCallback ConnectionTestOperationCompleted;

	private SendOrPostCallback MES_CarrierInOperationCompleted;

	private SendOrPostCallback MES_CarrierIn_JustCheckOperationCompleted;

	private SendOrPostCallback MES_CheckEdcRawDataOperationCompleted;

	private SendOrPostCallback WOBomQryOperationCompleted;

	private bool useDefaultCredentialsSetExplicitly;

	public new string Url
	{
		get
		{
			return base.Url;
		}
		set
		{
			if (IsLocalFileSystemWebService(base.Url) && !useDefaultCredentialsSetExplicitly && !IsLocalFileSystemWebService(value))
			{
				base.UseDefaultCredentials = false;
			}
			base.Url = value;
		}
	}

	public new bool UseDefaultCredentials
	{
		get
		{
			return base.UseDefaultCredentials;
		}
		set
		{
			base.UseDefaultCredentials = value;
			useDefaultCredentialsSetExplicitly = true;
		}
	}

	public event EqpTransactionCompletedEventHandler EqpTransactionCompleted;

	public event InsertEqpErrorLogCompletedEventHandler InsertEqpErrorLogCompleted;

	public event InsertEqpTransactionLogCompletedEventHandler InsertEqpTransactionLogCompleted;

	public event EqpStatusRefreshCompletedEventHandler EqpStatusRefreshCompleted;

	public event InsertParameterCompletedEventHandler InsertParameterCompleted;

	public event CheckUserCertCompletedEventHandler CheckUserCertCompleted;

	public event CheckUserCompletedEventHandler CheckUserCompleted;

	public event CheckWOCompletedEventHandler CheckWOCompleted;

	public event CheckTraceMatCompletedEventHandler CheckTraceMatCompleted;

	public event CheckWOAlreadyCheckInCompletedEventHandler CheckWOAlreadyCheckInCompleted;

	public event WOQryCompletedEventHandler WOQryCompleted;

	public event WOInCheckCompletedEventHandler WOInCheckCompleted;

	public event WO_CheckAlreadyInCompletedEventHandler WO_CheckAlreadyInCompleted;

	public event WO_InsertUseMachCompletedEventHandler WO_InsertUseMachCompleted;

	public event WOCheckInCompletedEventHandler WOCheckInCompleted;

	public event WOCheckIn_JustCheckCompletedEventHandler WOCheckIn_JustCheckCompleted;

	public event WOParmCodeQryCompletedEventHandler WOParmCodeQryCompleted;

	public event GetParaCompletedEventHandler GetParaCompleted;

	public event GetWorkCenterAllParaCompletedEventHandler GetWorkCenterAllParaCompleted;

	public event EqpMatAddCompletedEventHandler EqpMatAddCompleted;

	public event EqpMatAdd_BatchCompletedEventHandler EqpMatAdd_BatchCompleted;

	public event EqpMatAdd_Batch_SkipRepeatCompletedEventHandler EqpMatAdd_Batch_SkipRepeatCompleted;

	public event EqpWOInputAddCompletedEventHandler EqpWOInputAddCompleted;

	public event EqpWOInputAdd_BatchCompletedEventHandler EqpWOInputAdd_BatchCompleted;

	public event EqpCarrierAddCompletedEventHandler EqpCarrierAddCompleted;

	public event EqpCarrierAdd_BatchCompletedEventHandler EqpCarrierAdd_BatchCompleted;

	public event EqpPartProcessCompletedEventHandler EqpPartProcessCompleted;

	public event EqpPartProcess_BatchCompletedEventHandler EqpPartProcess_BatchCompleted;

	public event EqpTestValueRecordCompletedEventHandler EqpTestValueRecordCompleted;

	public event MouldInfoCompletedEventHandler MouldInfoCompleted;

	public event MouldInfo_ByIDCompletedEventHandler MouldInfo_ByIDCompleted;

	public event MouldOnlineCompletedEventHandler MouldOnlineCompleted;

	public event MouldOnlineByIDCompletedEventHandler MouldOnlineByIDCompleted;

	public event GetMoverRsnCompletedEventHandler GetMoverRsnCompleted;

	public event MouldOffLineCompletedEventHandler MouldOffLineCompleted;

	public event MouldOffLine_ByIDCompletedEventHandler MouldOffLine_ByIDCompleted;

	public event MouldOutputUpdateCompletedEventHandler MouldOutputUpdateCompleted;

	public event MouldOutputUpdate_ByIDCompletedEventHandler MouldOutputUpdate_ByIDCompleted;

	public event MachOnlineMouldQueryCompletedEventHandler MachOnlineMouldQueryCompleted;

	public event UploadTestResultFileCompletedEventHandler UploadTestResultFileCompleted;

	public event CarrierInCompletedEventHandler CarrierInCompleted;

	public event WoCheckInCarrierCheckCompletedEventHandler WoCheckInCarrierCheckCompleted;

	public event WoCheckOutCarrierCheckCompletedEventHandler WoCheckOutCarrierCheckCompleted;

	public event GetWONoByToolNoCompletedEventHandler GetWONoByToolNoCompleted;

	public event ConnectionTestCompletedEventHandler ConnectionTestCompleted;

	public event MES_CarrierInCompletedEventHandler MES_CarrierInCompleted;

	public event MES_CarrierIn_JustCheckCompletedEventHandler MES_CarrierIn_JustCheckCompleted;

	public event MES_CheckEdcRawDataCompletedEventHandler MES_CheckEdcRawDataCompleted;

	public event WOBomQryCompletedEventHandler WOBomQryCompleted;

	public Eqp_Portal()
	{
		Url = Settings.Default.Protocol_MES_corp_delta_twcynmes01_Eqp_Portal;
		if (IsLocalFileSystemWebService(Url))
		{
			UseDefaultCredentials = true;
			useDefaultCredentialsSetExplicitly = false;
		}
		else
		{
			useDefaultCredentialsSetExplicitly = true;
		}
	}

	[SoapDocumentMethod("CyntecMES/EqpTransaction", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string EqpTransaction(string pParameter)
	{
		return (string)Invoke("EqpTransaction", new object[1] { pParameter })[0];
	}

	public void EqpTransactionAsync(string pParameter)
	{
		EqpTransactionAsync(pParameter, null);
	}

	public void EqpTransactionAsync(string pParameter, object userState)
	{
		if (EqpTransactionOperationCompleted == null)
		{
			EqpTransactionOperationCompleted = OnEqpTransactionOperationCompleted;
		}
		InvokeAsync("EqpTransaction", new object[1] { pParameter }, EqpTransactionOperationCompleted, userState);
	}

	private void OnEqpTransactionOperationCompleted(object arg)
	{
		if (this.EqpTransactionCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.EqpTransactionCompleted(this, new EqpTransactionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/InsertEqpErrorLog", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string InsertEqpErrorLog(string pParameter)
	{
		return (string)Invoke("InsertEqpErrorLog", new object[1] { pParameter })[0];
	}

	public void InsertEqpErrorLogAsync(string pParameter)
	{
		InsertEqpErrorLogAsync(pParameter, null);
	}

	public void InsertEqpErrorLogAsync(string pParameter, object userState)
	{
		if (InsertEqpErrorLogOperationCompleted == null)
		{
			InsertEqpErrorLogOperationCompleted = OnInsertEqpErrorLogOperationCompleted;
		}
		InvokeAsync("InsertEqpErrorLog", new object[1] { pParameter }, InsertEqpErrorLogOperationCompleted, userState);
	}

	private void OnInsertEqpErrorLogOperationCompleted(object arg)
	{
		if (this.InsertEqpErrorLogCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.InsertEqpErrorLogCompleted(this, new InsertEqpErrorLogCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/InsertEqpTransactionLog", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string InsertEqpTransactionLog(string pParameter)
	{
		return (string)Invoke("InsertEqpTransactionLog", new object[1] { pParameter })[0];
	}

	public void InsertEqpTransactionLogAsync(string pParameter)
	{
		InsertEqpTransactionLogAsync(pParameter, null);
	}

	public void InsertEqpTransactionLogAsync(string pParameter, object userState)
	{
		if (InsertEqpTransactionLogOperationCompleted == null)
		{
			InsertEqpTransactionLogOperationCompleted = OnInsertEqpTransactionLogOperationCompleted;
		}
		InvokeAsync("InsertEqpTransactionLog", new object[1] { pParameter }, InsertEqpTransactionLogOperationCompleted, userState);
	}

	private void OnInsertEqpTransactionLogOperationCompleted(object arg)
	{
		if (this.InsertEqpTransactionLogCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.InsertEqpTransactionLogCompleted(this, new InsertEqpTransactionLogCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/EqpStatusRefresh", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string EqpStatusRefresh(string pParameter)
	{
		return (string)Invoke("EqpStatusRefresh", new object[1] { pParameter })[0];
	}

	public void EqpStatusRefreshAsync(string pParameter)
	{
		EqpStatusRefreshAsync(pParameter, null);
	}

	public void EqpStatusRefreshAsync(string pParameter, object userState)
	{
		if (EqpStatusRefreshOperationCompleted == null)
		{
			EqpStatusRefreshOperationCompleted = OnEqpStatusRefreshOperationCompleted;
		}
		InvokeAsync("EqpStatusRefresh", new object[1] { pParameter }, EqpStatusRefreshOperationCompleted, userState);
	}

	private void OnEqpStatusRefreshOperationCompleted(object arg)
	{
		if (this.EqpStatusRefreshCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.EqpStatusRefreshCompleted(this, new EqpStatusRefreshCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/InsertParameter", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string InsertParameter(string pParameter)
	{
		return (string)Invoke("InsertParameter", new object[1] { pParameter })[0];
	}

	public void InsertParameterAsync(string pParameter)
	{
		InsertParameterAsync(pParameter, null);
	}

	public void InsertParameterAsync(string pParameter, object userState)
	{
		if (InsertParameterOperationCompleted == null)
		{
			InsertParameterOperationCompleted = OnInsertParameterOperationCompleted;
		}
		InvokeAsync("InsertParameter", new object[1] { pParameter }, InsertParameterOperationCompleted, userState);
	}

	private void OnInsertParameterOperationCompleted(object arg)
	{
		if (this.InsertParameterCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.InsertParameterCompleted(this, new InsertParameterCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/CheckUserCert", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string CheckUserCert(string pParameter)
	{
		return (string)Invoke("CheckUserCert", new object[1] { pParameter })[0];
	}

	public void CheckUserCertAsync(string pParameter)
	{
		CheckUserCertAsync(pParameter, null);
	}

	public void CheckUserCertAsync(string pParameter, object userState)
	{
		if (CheckUserCertOperationCompleted == null)
		{
			CheckUserCertOperationCompleted = OnCheckUserCertOperationCompleted;
		}
		InvokeAsync("CheckUserCert", new object[1] { pParameter }, CheckUserCertOperationCompleted, userState);
	}

	private void OnCheckUserCertOperationCompleted(object arg)
	{
		if (this.CheckUserCertCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.CheckUserCertCompleted(this, new CheckUserCertCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/CheckUser", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string CheckUser(string pParameter)
	{
		return (string)Invoke("CheckUser", new object[1] { pParameter })[0];
	}

	public void CheckUserAsync(string pParameter)
	{
		CheckUserAsync(pParameter, null);
	}

	public void CheckUserAsync(string pParameter, object userState)
	{
		if (CheckUserOperationCompleted == null)
		{
			CheckUserOperationCompleted = OnCheckUserOperationCompleted;
		}
		InvokeAsync("CheckUser", new object[1] { pParameter }, CheckUserOperationCompleted, userState);
	}

	private void OnCheckUserOperationCompleted(object arg)
	{
		if (this.CheckUserCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.CheckUserCompleted(this, new CheckUserCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/CheckWO", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string CheckWO(string pParameter)
	{
		return (string)Invoke("CheckWO", new object[1] { pParameter })[0];
	}

	public void CheckWOAsync(string pParameter)
	{
		CheckWOAsync(pParameter, null);
	}

	public void CheckWOAsync(string pParameter, object userState)
	{
		if (CheckWOOperationCompleted == null)
		{
			CheckWOOperationCompleted = OnCheckWOOperationCompleted;
		}
		InvokeAsync("CheckWO", new object[1] { pParameter }, CheckWOOperationCompleted, userState);
	}

	private void OnCheckWOOperationCompleted(object arg)
	{
		if (this.CheckWOCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.CheckWOCompleted(this, new CheckWOCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/CheckTraceMat", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string CheckTraceMat(string pParameter)
	{
		return (string)Invoke("CheckTraceMat", new object[1] { pParameter })[0];
	}

	public void CheckTraceMatAsync(string pParameter)
	{
		CheckTraceMatAsync(pParameter, null);
	}

	public void CheckTraceMatAsync(string pParameter, object userState)
	{
		if (CheckTraceMatOperationCompleted == null)
		{
			CheckTraceMatOperationCompleted = OnCheckTraceMatOperationCompleted;
		}
		InvokeAsync("CheckTraceMat", new object[1] { pParameter }, CheckTraceMatOperationCompleted, userState);
	}

	private void OnCheckTraceMatOperationCompleted(object arg)
	{
		if (this.CheckTraceMatCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.CheckTraceMatCompleted(this, new CheckTraceMatCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/CheckWOAlreadyCheckIn", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string CheckWOAlreadyCheckIn(string pParameter)
	{
		return (string)Invoke("CheckWOAlreadyCheckIn", new object[1] { pParameter })[0];
	}

	public void CheckWOAlreadyCheckInAsync(string pParameter)
	{
		CheckWOAlreadyCheckInAsync(pParameter, null);
	}

	public void CheckWOAlreadyCheckInAsync(string pParameter, object userState)
	{
		if (CheckWOAlreadyCheckInOperationCompleted == null)
		{
			CheckWOAlreadyCheckInOperationCompleted = OnCheckWOAlreadyCheckInOperationCompleted;
		}
		InvokeAsync("CheckWOAlreadyCheckIn", new object[1] { pParameter }, CheckWOAlreadyCheckInOperationCompleted, userState);
	}

	private void OnCheckWOAlreadyCheckInOperationCompleted(object arg)
	{
		if (this.CheckWOAlreadyCheckInCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.CheckWOAlreadyCheckInCompleted(this, new CheckWOAlreadyCheckInCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/WOQry", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string WOQry(string pParameter)
	{
		return (string)Invoke("WOQry", new object[1] { pParameter })[0];
	}

	public void WOQryAsync(string pParameter)
	{
		WOQryAsync(pParameter, null);
	}

	public void WOQryAsync(string pParameter, object userState)
	{
		if (WOQryOperationCompleted == null)
		{
			WOQryOperationCompleted = OnWOQryOperationCompleted;
		}
		InvokeAsync("WOQry", new object[1] { pParameter }, WOQryOperationCompleted, userState);
	}

	private void OnWOQryOperationCompleted(object arg)
	{
		if (this.WOQryCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.WOQryCompleted(this, new WOQryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/WOInCheck", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string WOInCheck(string pParameter)
	{
		return (string)Invoke("WOInCheck", new object[1] { pParameter })[0];
	}

	public void WOInCheckAsync(string pParameter)
	{
		WOInCheckAsync(pParameter, null);
	}

	public void WOInCheckAsync(string pParameter, object userState)
	{
		if (WOInCheckOperationCompleted == null)
		{
			WOInCheckOperationCompleted = OnWOInCheckOperationCompleted;
		}
		InvokeAsync("WOInCheck", new object[1] { pParameter }, WOInCheckOperationCompleted, userState);
	}

	private void OnWOInCheckOperationCompleted(object arg)
	{
		if (this.WOInCheckCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.WOInCheckCompleted(this, new WOInCheckCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/WO_CheckAlreadyIn", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string WO_CheckAlreadyIn(string pParameter)
	{
		return (string)Invoke("WO_CheckAlreadyIn", new object[1] { pParameter })[0];
	}

	public void WO_CheckAlreadyInAsync(string pParameter)
	{
		WO_CheckAlreadyInAsync(pParameter, null);
	}

	public void WO_CheckAlreadyInAsync(string pParameter, object userState)
	{
		if (WO_CheckAlreadyInOperationCompleted == null)
		{
			WO_CheckAlreadyInOperationCompleted = OnWO_CheckAlreadyInOperationCompleted;
		}
		InvokeAsync("WO_CheckAlreadyIn", new object[1] { pParameter }, WO_CheckAlreadyInOperationCompleted, userState);
	}

	private void OnWO_CheckAlreadyInOperationCompleted(object arg)
	{
		if (this.WO_CheckAlreadyInCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.WO_CheckAlreadyInCompleted(this, new WO_CheckAlreadyInCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/WO_InsertUseMach", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string WO_InsertUseMach(string pParameter)
	{
		return (string)Invoke("WO_InsertUseMach", new object[1] { pParameter })[0];
	}

	public void WO_InsertUseMachAsync(string pParameter)
	{
		WO_InsertUseMachAsync(pParameter, null);
	}

	public void WO_InsertUseMachAsync(string pParameter, object userState)
	{
		if (WO_InsertUseMachOperationCompleted == null)
		{
			WO_InsertUseMachOperationCompleted = OnWO_InsertUseMachOperationCompleted;
		}
		InvokeAsync("WO_InsertUseMach", new object[1] { pParameter }, WO_InsertUseMachOperationCompleted, userState);
	}

	private void OnWO_InsertUseMachOperationCompleted(object arg)
	{
		if (this.WO_InsertUseMachCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.WO_InsertUseMachCompleted(this, new WO_InsertUseMachCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/WOCheckIn", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string WOCheckIn(string pParameter)
	{
		return (string)Invoke("WOCheckIn", new object[1] { pParameter })[0];
	}

	public void WOCheckInAsync(string pParameter)
	{
		WOCheckInAsync(pParameter, null);
	}

	public void WOCheckInAsync(string pParameter, object userState)
	{
		if (WOCheckInOperationCompleted == null)
		{
			WOCheckInOperationCompleted = OnWOCheckInOperationCompleted;
		}
		InvokeAsync("WOCheckIn", new object[1] { pParameter }, WOCheckInOperationCompleted, userState);
	}

	private void OnWOCheckInOperationCompleted(object arg)
	{
		if (this.WOCheckInCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.WOCheckInCompleted(this, new WOCheckInCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/WOCheckIn_JustCheck", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string WOCheckIn_JustCheck(string pParameter)
	{
		return (string)Invoke("WOCheckIn_JustCheck", new object[1] { pParameter })[0];
	}

	public void WOCheckIn_JustCheckAsync(string pParameter)
	{
		WOCheckIn_JustCheckAsync(pParameter, null);
	}

	public void WOCheckIn_JustCheckAsync(string pParameter, object userState)
	{
		if (WOCheckIn_JustCheckOperationCompleted == null)
		{
			WOCheckIn_JustCheckOperationCompleted = OnWOCheckIn_JustCheckOperationCompleted;
		}
		InvokeAsync("WOCheckIn_JustCheck", new object[1] { pParameter }, WOCheckIn_JustCheckOperationCompleted, userState);
	}

	private void OnWOCheckIn_JustCheckOperationCompleted(object arg)
	{
		if (this.WOCheckIn_JustCheckCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.WOCheckIn_JustCheckCompleted(this, new WOCheckIn_JustCheckCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/WOParmCodeQry", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string WOParmCodeQry(string pParameter)
	{
		return (string)Invoke("WOParmCodeQry", new object[1] { pParameter })[0];
	}

	public void WOParmCodeQryAsync(string pParameter)
	{
		WOParmCodeQryAsync(pParameter, null);
	}

	public void WOParmCodeQryAsync(string pParameter, object userState)
	{
		if (WOParmCodeQryOperationCompleted == null)
		{
			WOParmCodeQryOperationCompleted = OnWOParmCodeQryOperationCompleted;
		}
		InvokeAsync("WOParmCodeQry", new object[1] { pParameter }, WOParmCodeQryOperationCompleted, userState);
	}

	private void OnWOParmCodeQryOperationCompleted(object arg)
	{
		if (this.WOParmCodeQryCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.WOParmCodeQryCompleted(this, new WOParmCodeQryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/GetPara", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetPara(string pParameter)
	{
		return (string)Invoke("GetPara", new object[1] { pParameter })[0];
	}

	public void GetParaAsync(string pParameter)
	{
		GetParaAsync(pParameter, null);
	}

	public void GetParaAsync(string pParameter, object userState)
	{
		if (GetParaOperationCompleted == null)
		{
			GetParaOperationCompleted = OnGetParaOperationCompleted;
		}
		InvokeAsync("GetPara", new object[1] { pParameter }, GetParaOperationCompleted, userState);
	}

	private void OnGetParaOperationCompleted(object arg)
	{
		if (this.GetParaCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.GetParaCompleted(this, new GetParaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/GetWorkCenterAllPara", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetWorkCenterAllPara(string pParameter)
	{
		return (string)Invoke("GetWorkCenterAllPara", new object[1] { pParameter })[0];
	}

	public void GetWorkCenterAllParaAsync(string pParameter)
	{
		GetWorkCenterAllParaAsync(pParameter, null);
	}

	public void GetWorkCenterAllParaAsync(string pParameter, object userState)
	{
		if (GetWorkCenterAllParaOperationCompleted == null)
		{
			GetWorkCenterAllParaOperationCompleted = OnGetWorkCenterAllParaOperationCompleted;
		}
		InvokeAsync("GetWorkCenterAllPara", new object[1] { pParameter }, GetWorkCenterAllParaOperationCompleted, userState);
	}

	private void OnGetWorkCenterAllParaOperationCompleted(object arg)
	{
		if (this.GetWorkCenterAllParaCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.GetWorkCenterAllParaCompleted(this, new GetWorkCenterAllParaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/EqpMatAdd", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string EqpMatAdd(string pParameter)
	{
		return (string)Invoke("EqpMatAdd", new object[1] { pParameter })[0];
	}

	public void EqpMatAddAsync(string pParameter)
	{
		EqpMatAddAsync(pParameter, null);
	}

	public void EqpMatAddAsync(string pParameter, object userState)
	{
		if (EqpMatAddOperationCompleted == null)
		{
			EqpMatAddOperationCompleted = OnEqpMatAddOperationCompleted;
		}
		InvokeAsync("EqpMatAdd", new object[1] { pParameter }, EqpMatAddOperationCompleted, userState);
	}

	private void OnEqpMatAddOperationCompleted(object arg)
	{
		if (this.EqpMatAddCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.EqpMatAddCompleted(this, new EqpMatAddCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/EqpMatAdd_Batch", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string EqpMatAdd_Batch(string pParameter)
	{
		return (string)Invoke("EqpMatAdd_Batch", new object[1] { pParameter })[0];
	}

	public void EqpMatAdd_BatchAsync(string pParameter)
	{
		EqpMatAdd_BatchAsync(pParameter, null);
	}

	public void EqpMatAdd_BatchAsync(string pParameter, object userState)
	{
		if (EqpMatAdd_BatchOperationCompleted == null)
		{
			EqpMatAdd_BatchOperationCompleted = OnEqpMatAdd_BatchOperationCompleted;
		}
		InvokeAsync("EqpMatAdd_Batch", new object[1] { pParameter }, EqpMatAdd_BatchOperationCompleted, userState);
	}

	private void OnEqpMatAdd_BatchOperationCompleted(object arg)
	{
		if (this.EqpMatAdd_BatchCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.EqpMatAdd_BatchCompleted(this, new EqpMatAdd_BatchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/EqpMatAdd_Batch_SkipRepeat", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string EqpMatAdd_Batch_SkipRepeat(string pParameter)
	{
		return (string)Invoke("EqpMatAdd_Batch_SkipRepeat", new object[1] { pParameter })[0];
	}

	public void EqpMatAdd_Batch_SkipRepeatAsync(string pParameter)
	{
		EqpMatAdd_Batch_SkipRepeatAsync(pParameter, null);
	}

	public void EqpMatAdd_Batch_SkipRepeatAsync(string pParameter, object userState)
	{
		if (EqpMatAdd_Batch_SkipRepeatOperationCompleted == null)
		{
			EqpMatAdd_Batch_SkipRepeatOperationCompleted = OnEqpMatAdd_Batch_SkipRepeatOperationCompleted;
		}
		InvokeAsync("EqpMatAdd_Batch_SkipRepeat", new object[1] { pParameter }, EqpMatAdd_Batch_SkipRepeatOperationCompleted, userState);
	}

	private void OnEqpMatAdd_Batch_SkipRepeatOperationCompleted(object arg)
	{
		if (this.EqpMatAdd_Batch_SkipRepeatCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.EqpMatAdd_Batch_SkipRepeatCompleted(this, new EqpMatAdd_Batch_SkipRepeatCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/EqpWOInputAdd", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string EqpWOInputAdd(string pParameter)
	{
		return (string)Invoke("EqpWOInputAdd", new object[1] { pParameter })[0];
	}

	public void EqpWOInputAddAsync(string pParameter)
	{
		EqpWOInputAddAsync(pParameter, null);
	}

	public void EqpWOInputAddAsync(string pParameter, object userState)
	{
		if (EqpWOInputAddOperationCompleted == null)
		{
			EqpWOInputAddOperationCompleted = OnEqpWOInputAddOperationCompleted;
		}
		InvokeAsync("EqpWOInputAdd", new object[1] { pParameter }, EqpWOInputAddOperationCompleted, userState);
	}

	private void OnEqpWOInputAddOperationCompleted(object arg)
	{
		if (this.EqpWOInputAddCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.EqpWOInputAddCompleted(this, new EqpWOInputAddCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/EqpWOInputAdd_Batch", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string EqpWOInputAdd_Batch(string pParameter)
	{
		return (string)Invoke("EqpWOInputAdd_Batch", new object[1] { pParameter })[0];
	}

	public void EqpWOInputAdd_BatchAsync(string pParameter)
	{
		EqpWOInputAdd_BatchAsync(pParameter, null);
	}

	public void EqpWOInputAdd_BatchAsync(string pParameter, object userState)
	{
		if (EqpWOInputAdd_BatchOperationCompleted == null)
		{
			EqpWOInputAdd_BatchOperationCompleted = OnEqpWOInputAdd_BatchOperationCompleted;
		}
		InvokeAsync("EqpWOInputAdd_Batch", new object[1] { pParameter }, EqpWOInputAdd_BatchOperationCompleted, userState);
	}

	private void OnEqpWOInputAdd_BatchOperationCompleted(object arg)
	{
		if (this.EqpWOInputAdd_BatchCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.EqpWOInputAdd_BatchCompleted(this, new EqpWOInputAdd_BatchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/EqpCarrierAdd", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string EqpCarrierAdd(string pParameter)
	{
		return (string)Invoke("EqpCarrierAdd", new object[1] { pParameter })[0];
	}

	public void EqpCarrierAddAsync(string pParameter)
	{
		EqpCarrierAddAsync(pParameter, null);
	}

	public void EqpCarrierAddAsync(string pParameter, object userState)
	{
		if (EqpCarrierAddOperationCompleted == null)
		{
			EqpCarrierAddOperationCompleted = OnEqpCarrierAddOperationCompleted;
		}
		InvokeAsync("EqpCarrierAdd", new object[1] { pParameter }, EqpCarrierAddOperationCompleted, userState);
	}

	private void OnEqpCarrierAddOperationCompleted(object arg)
	{
		if (this.EqpCarrierAddCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.EqpCarrierAddCompleted(this, new EqpCarrierAddCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/EqpCarrierAdd_Batch", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string EqpCarrierAdd_Batch(string pParameter)
	{
		return (string)Invoke("EqpCarrierAdd_Batch", new object[1] { pParameter })[0];
	}

	public void EqpCarrierAdd_BatchAsync(string pParameter)
	{
		EqpCarrierAdd_BatchAsync(pParameter, null);
	}

	public void EqpCarrierAdd_BatchAsync(string pParameter, object userState)
	{
		if (EqpCarrierAdd_BatchOperationCompleted == null)
		{
			EqpCarrierAdd_BatchOperationCompleted = OnEqpCarrierAdd_BatchOperationCompleted;
		}
		InvokeAsync("EqpCarrierAdd_Batch", new object[1] { pParameter }, EqpCarrierAdd_BatchOperationCompleted, userState);
	}

	private void OnEqpCarrierAdd_BatchOperationCompleted(object arg)
	{
		if (this.EqpCarrierAdd_BatchCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.EqpCarrierAdd_BatchCompleted(this, new EqpCarrierAdd_BatchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/EqpPartProcess", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string EqpPartProcess(string pParameter)
	{
		return (string)Invoke("EqpPartProcess", new object[1] { pParameter })[0];
	}

	public void EqpPartProcessAsync(string pParameter)
	{
		EqpPartProcessAsync(pParameter, null);
	}

	public void EqpPartProcessAsync(string pParameter, object userState)
	{
		if (EqpPartProcessOperationCompleted == null)
		{
			EqpPartProcessOperationCompleted = OnEqpPartProcessOperationCompleted;
		}
		InvokeAsync("EqpPartProcess", new object[1] { pParameter }, EqpPartProcessOperationCompleted, userState);
	}

	private void OnEqpPartProcessOperationCompleted(object arg)
	{
		if (this.EqpPartProcessCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.EqpPartProcessCompleted(this, new EqpPartProcessCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/EqpPartProcess_Batch", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string EqpPartProcess_Batch(string pParameter)
	{
		return (string)Invoke("EqpPartProcess_Batch", new object[1] { pParameter })[0];
	}

	public void EqpPartProcess_BatchAsync(string pParameter)
	{
		EqpPartProcess_BatchAsync(pParameter, null);
	}

	public void EqpPartProcess_BatchAsync(string pParameter, object userState)
	{
		if (EqpPartProcess_BatchOperationCompleted == null)
		{
			EqpPartProcess_BatchOperationCompleted = OnEqpPartProcess_BatchOperationCompleted;
		}
		InvokeAsync("EqpPartProcess_Batch", new object[1] { pParameter }, EqpPartProcess_BatchOperationCompleted, userState);
	}

	private void OnEqpPartProcess_BatchOperationCompleted(object arg)
	{
		if (this.EqpPartProcess_BatchCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.EqpPartProcess_BatchCompleted(this, new EqpPartProcess_BatchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/EqpTestValueRecord", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string EqpTestValueRecord(string pParameter)
	{
		return (string)Invoke("EqpTestValueRecord", new object[1] { pParameter })[0];
	}

	public void EqpTestValueRecordAsync(string pParameter)
	{
		EqpTestValueRecordAsync(pParameter, null);
	}

	public void EqpTestValueRecordAsync(string pParameter, object userState)
	{
		if (EqpTestValueRecordOperationCompleted == null)
		{
			EqpTestValueRecordOperationCompleted = OnEqpTestValueRecordOperationCompleted;
		}
		InvokeAsync("EqpTestValueRecord", new object[1] { pParameter }, EqpTestValueRecordOperationCompleted, userState);
	}

	private void OnEqpTestValueRecordOperationCompleted(object arg)
	{
		if (this.EqpTestValueRecordCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.EqpTestValueRecordCompleted(this, new EqpTestValueRecordCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/MouldInfo", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string MouldInfo(string pParameter)
	{
		return (string)Invoke("MouldInfo", new object[1] { pParameter })[0];
	}

	public void MouldInfoAsync(string pParameter)
	{
		MouldInfoAsync(pParameter, null);
	}

	public void MouldInfoAsync(string pParameter, object userState)
	{
		if (MouldInfoOperationCompleted == null)
		{
			MouldInfoOperationCompleted = OnMouldInfoOperationCompleted;
		}
		InvokeAsync("MouldInfo", new object[1] { pParameter }, MouldInfoOperationCompleted, userState);
	}

	private void OnMouldInfoOperationCompleted(object arg)
	{
		if (this.MouldInfoCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.MouldInfoCompleted(this, new MouldInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/MouldInfo_ByID", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string MouldInfo_ByID(string pParameter)
	{
		return (string)Invoke("MouldInfo_ByID", new object[1] { pParameter })[0];
	}

	public void MouldInfo_ByIDAsync(string pParameter)
	{
		MouldInfo_ByIDAsync(pParameter, null);
	}

	public void MouldInfo_ByIDAsync(string pParameter, object userState)
	{
		if (MouldInfo_ByIDOperationCompleted == null)
		{
			MouldInfo_ByIDOperationCompleted = OnMouldInfo_ByIDOperationCompleted;
		}
		InvokeAsync("MouldInfo_ByID", new object[1] { pParameter }, MouldInfo_ByIDOperationCompleted, userState);
	}

	private void OnMouldInfo_ByIDOperationCompleted(object arg)
	{
		if (this.MouldInfo_ByIDCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.MouldInfo_ByIDCompleted(this, new MouldInfo_ByIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/MouldOnline", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string MouldOnline(string pParameter)
	{
		return (string)Invoke("MouldOnline", new object[1] { pParameter })[0];
	}

	public void MouldOnlineAsync(string pParameter)
	{
		MouldOnlineAsync(pParameter, null);
	}

	public void MouldOnlineAsync(string pParameter, object userState)
	{
		if (MouldOnlineOperationCompleted == null)
		{
			MouldOnlineOperationCompleted = OnMouldOnlineOperationCompleted;
		}
		InvokeAsync("MouldOnline", new object[1] { pParameter }, MouldOnlineOperationCompleted, userState);
	}

	private void OnMouldOnlineOperationCompleted(object arg)
	{
		if (this.MouldOnlineCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.MouldOnlineCompleted(this, new MouldOnlineCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/MouldOnlineByID", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string MouldOnlineByID(string pParameter)
	{
		return (string)Invoke("MouldOnlineByID", new object[1] { pParameter })[0];
	}

	public void MouldOnlineByIDAsync(string pParameter)
	{
		MouldOnlineByIDAsync(pParameter, null);
	}

	public void MouldOnlineByIDAsync(string pParameter, object userState)
	{
		if (MouldOnlineByIDOperationCompleted == null)
		{
			MouldOnlineByIDOperationCompleted = OnMouldOnlineByIDOperationCompleted;
		}
		InvokeAsync("MouldOnlineByID", new object[1] { pParameter }, MouldOnlineByIDOperationCompleted, userState);
	}

	private void OnMouldOnlineByIDOperationCompleted(object arg)
	{
		if (this.MouldOnlineByIDCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.MouldOnlineByIDCompleted(this, new MouldOnlineByIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/GetMoverRsn", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetMoverRsn(string pParameter)
	{
		return (string)Invoke("GetMoverRsn", new object[1] { pParameter })[0];
	}

	public void GetMoverRsnAsync(string pParameter)
	{
		GetMoverRsnAsync(pParameter, null);
	}

	public void GetMoverRsnAsync(string pParameter, object userState)
	{
		if (GetMoverRsnOperationCompleted == null)
		{
			GetMoverRsnOperationCompleted = OnGetMoverRsnOperationCompleted;
		}
		InvokeAsync("GetMoverRsn", new object[1] { pParameter }, GetMoverRsnOperationCompleted, userState);
	}

	private void OnGetMoverRsnOperationCompleted(object arg)
	{
		if (this.GetMoverRsnCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.GetMoverRsnCompleted(this, new GetMoverRsnCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/MouldOffLine", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string MouldOffLine(string pParameter)
	{
		return (string)Invoke("MouldOffLine", new object[1] { pParameter })[0];
	}

	public void MouldOffLineAsync(string pParameter)
	{
		MouldOffLineAsync(pParameter, null);
	}

	public void MouldOffLineAsync(string pParameter, object userState)
	{
		if (MouldOffLineOperationCompleted == null)
		{
			MouldOffLineOperationCompleted = OnMouldOffLineOperationCompleted;
		}
		InvokeAsync("MouldOffLine", new object[1] { pParameter }, MouldOffLineOperationCompleted, userState);
	}

	private void OnMouldOffLineOperationCompleted(object arg)
	{
		if (this.MouldOffLineCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.MouldOffLineCompleted(this, new MouldOffLineCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/MouldOffLine_ByID", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string MouldOffLine_ByID(string pParameter)
	{
		return (string)Invoke("MouldOffLine_ByID", new object[1] { pParameter })[0];
	}

	public void MouldOffLine_ByIDAsync(string pParameter)
	{
		MouldOffLine_ByIDAsync(pParameter, null);
	}

	public void MouldOffLine_ByIDAsync(string pParameter, object userState)
	{
		if (MouldOffLine_ByIDOperationCompleted == null)
		{
			MouldOffLine_ByIDOperationCompleted = OnMouldOffLine_ByIDOperationCompleted;
		}
		InvokeAsync("MouldOffLine_ByID", new object[1] { pParameter }, MouldOffLine_ByIDOperationCompleted, userState);
	}

	private void OnMouldOffLine_ByIDOperationCompleted(object arg)
	{
		if (this.MouldOffLine_ByIDCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.MouldOffLine_ByIDCompleted(this, new MouldOffLine_ByIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/MouldOutputUpdate", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string MouldOutputUpdate(string pParameter)
	{
		return (string)Invoke("MouldOutputUpdate", new object[1] { pParameter })[0];
	}

	public void MouldOutputUpdateAsync(string pParameter)
	{
		MouldOutputUpdateAsync(pParameter, null);
	}

	public void MouldOutputUpdateAsync(string pParameter, object userState)
	{
		if (MouldOutputUpdateOperationCompleted == null)
		{
			MouldOutputUpdateOperationCompleted = OnMouldOutputUpdateOperationCompleted;
		}
		InvokeAsync("MouldOutputUpdate", new object[1] { pParameter }, MouldOutputUpdateOperationCompleted, userState);
	}

	private void OnMouldOutputUpdateOperationCompleted(object arg)
	{
		if (this.MouldOutputUpdateCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.MouldOutputUpdateCompleted(this, new MouldOutputUpdateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/MouldOutputUpdate_ByID", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string MouldOutputUpdate_ByID(string pParameter)
	{
		return (string)Invoke("MouldOutputUpdate_ByID", new object[1] { pParameter })[0];
	}

	public void MouldOutputUpdate_ByIDAsync(string pParameter)
	{
		MouldOutputUpdate_ByIDAsync(pParameter, null);
	}

	public void MouldOutputUpdate_ByIDAsync(string pParameter, object userState)
	{
		if (MouldOutputUpdate_ByIDOperationCompleted == null)
		{
			MouldOutputUpdate_ByIDOperationCompleted = OnMouldOutputUpdate_ByIDOperationCompleted;
		}
		InvokeAsync("MouldOutputUpdate_ByID", new object[1] { pParameter }, MouldOutputUpdate_ByIDOperationCompleted, userState);
	}

	private void OnMouldOutputUpdate_ByIDOperationCompleted(object arg)
	{
		if (this.MouldOutputUpdate_ByIDCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.MouldOutputUpdate_ByIDCompleted(this, new MouldOutputUpdate_ByIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/MachOnlineMouldQuery", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string MachOnlineMouldQuery(string pParameter)
	{
		return (string)Invoke("MachOnlineMouldQuery", new object[1] { pParameter })[0];
	}

	public void MachOnlineMouldQueryAsync(string pParameter)
	{
		MachOnlineMouldQueryAsync(pParameter, null);
	}

	public void MachOnlineMouldQueryAsync(string pParameter, object userState)
	{
		if (MachOnlineMouldQueryOperationCompleted == null)
		{
			MachOnlineMouldQueryOperationCompleted = OnMachOnlineMouldQueryOperationCompleted;
		}
		InvokeAsync("MachOnlineMouldQuery", new object[1] { pParameter }, MachOnlineMouldQueryOperationCompleted, userState);
	}

	private void OnMachOnlineMouldQueryOperationCompleted(object arg)
	{
		if (this.MachOnlineMouldQueryCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.MachOnlineMouldQueryCompleted(this, new MachOnlineMouldQueryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/UploadTestResultFile", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string UploadTestResultFile(string pParameter)
	{
		return (string)Invoke("UploadTestResultFile", new object[1] { pParameter })[0];
	}

	public void UploadTestResultFileAsync(string pParameter)
	{
		UploadTestResultFileAsync(pParameter, null);
	}

	public void UploadTestResultFileAsync(string pParameter, object userState)
	{
		if (UploadTestResultFileOperationCompleted == null)
		{
			UploadTestResultFileOperationCompleted = OnUploadTestResultFileOperationCompleted;
		}
		InvokeAsync("UploadTestResultFile", new object[1] { pParameter }, UploadTestResultFileOperationCompleted, userState);
	}

	private void OnUploadTestResultFileOperationCompleted(object arg)
	{
		if (this.UploadTestResultFileCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.UploadTestResultFileCompleted(this, new UploadTestResultFileCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/CarrierIn", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string CarrierIn(string pParameter)
	{
		return (string)Invoke("CarrierIn", new object[1] { pParameter })[0];
	}

	public void CarrierInAsync(string pParameter)
	{
		CarrierInAsync(pParameter, null);
	}

	public void CarrierInAsync(string pParameter, object userState)
	{
		if (CarrierInOperationCompleted == null)
		{
			CarrierInOperationCompleted = OnCarrierInOperationCompleted;
		}
		InvokeAsync("CarrierIn", new object[1] { pParameter }, CarrierInOperationCompleted, userState);
	}

	private void OnCarrierInOperationCompleted(object arg)
	{
		if (this.CarrierInCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.CarrierInCompleted(this, new CarrierInCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/WoCheckInCarrierCheck", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string WoCheckInCarrierCheck(string pParameter)
	{
		return (string)Invoke("WoCheckInCarrierCheck", new object[1] { pParameter })[0];
	}

	public void WoCheckInCarrierCheckAsync(string pParameter)
	{
		WoCheckInCarrierCheckAsync(pParameter, null);
	}

	public void WoCheckInCarrierCheckAsync(string pParameter, object userState)
	{
		if (WoCheckInCarrierCheckOperationCompleted == null)
		{
			WoCheckInCarrierCheckOperationCompleted = OnWoCheckInCarrierCheckOperationCompleted;
		}
		InvokeAsync("WoCheckInCarrierCheck", new object[1] { pParameter }, WoCheckInCarrierCheckOperationCompleted, userState);
	}

	private void OnWoCheckInCarrierCheckOperationCompleted(object arg)
	{
		if (this.WoCheckInCarrierCheckCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.WoCheckInCarrierCheckCompleted(this, new WoCheckInCarrierCheckCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/WoCheckOutCarrierCheck", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string WoCheckOutCarrierCheck(string pParameter)
	{
		return (string)Invoke("WoCheckOutCarrierCheck", new object[1] { pParameter })[0];
	}

	public void WoCheckOutCarrierCheckAsync(string pParameter)
	{
		WoCheckOutCarrierCheckAsync(pParameter, null);
	}

	public void WoCheckOutCarrierCheckAsync(string pParameter, object userState)
	{
		if (WoCheckOutCarrierCheckOperationCompleted == null)
		{
			WoCheckOutCarrierCheckOperationCompleted = OnWoCheckOutCarrierCheckOperationCompleted;
		}
		InvokeAsync("WoCheckOutCarrierCheck", new object[1] { pParameter }, WoCheckOutCarrierCheckOperationCompleted, userState);
	}

	private void OnWoCheckOutCarrierCheckOperationCompleted(object arg)
	{
		if (this.WoCheckOutCarrierCheckCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.WoCheckOutCarrierCheckCompleted(this, new WoCheckOutCarrierCheckCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/GetWONoByToolNo", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetWONoByToolNo(string pParameter)
	{
		return (string)Invoke("GetWONoByToolNo", new object[1] { pParameter })[0];
	}

	public void GetWONoByToolNoAsync(string pParameter)
	{
		GetWONoByToolNoAsync(pParameter, null);
	}

	public void GetWONoByToolNoAsync(string pParameter, object userState)
	{
		if (GetWONoByToolNoOperationCompleted == null)
		{
			GetWONoByToolNoOperationCompleted = OnGetWONoByToolNoOperationCompleted;
		}
		InvokeAsync("GetWONoByToolNo", new object[1] { pParameter }, GetWONoByToolNoOperationCompleted, userState);
	}

	private void OnGetWONoByToolNoOperationCompleted(object arg)
	{
		if (this.GetWONoByToolNoCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.GetWONoByToolNoCompleted(this, new GetWONoByToolNoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/ConnectionTest", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool ConnectionTest()
	{
		return (bool)Invoke("ConnectionTest", new object[0])[0];
	}

	public void ConnectionTestAsync()
	{
		ConnectionTestAsync(null);
	}

	public void ConnectionTestAsync(object userState)
	{
		if (ConnectionTestOperationCompleted == null)
		{
			ConnectionTestOperationCompleted = OnConnectionTestOperationCompleted;
		}
		InvokeAsync("ConnectionTest", new object[0], ConnectionTestOperationCompleted, userState);
	}

	private void OnConnectionTestOperationCompleted(object arg)
	{
		if (this.ConnectionTestCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.ConnectionTestCompleted(this, new ConnectionTestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/MES_CarrierIn", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string MES_CarrierIn(string pParameter)
	{
		return (string)Invoke("MES_CarrierIn", new object[1] { pParameter })[0];
	}

	public void MES_CarrierInAsync(string pParameter)
	{
		MES_CarrierInAsync(pParameter, null);
	}

	public void MES_CarrierInAsync(string pParameter, object userState)
	{
		if (MES_CarrierInOperationCompleted == null)
		{
			MES_CarrierInOperationCompleted = OnMES_CarrierInOperationCompleted;
		}
		InvokeAsync("MES_CarrierIn", new object[1] { pParameter }, MES_CarrierInOperationCompleted, userState);
	}

	private void OnMES_CarrierInOperationCompleted(object arg)
	{
		if (this.MES_CarrierInCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.MES_CarrierInCompleted(this, new MES_CarrierInCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/MES_CarrierIn_JustCheck", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string MES_CarrierIn_JustCheck(string pParameter)
	{
		return (string)Invoke("MES_CarrierIn_JustCheck", new object[1] { pParameter })[0];
	}

	public void MES_CarrierIn_JustCheckAsync(string pParameter)
	{
		MES_CarrierIn_JustCheckAsync(pParameter, null);
	}

	public void MES_CarrierIn_JustCheckAsync(string pParameter, object userState)
	{
		if (MES_CarrierIn_JustCheckOperationCompleted == null)
		{
			MES_CarrierIn_JustCheckOperationCompleted = OnMES_CarrierIn_JustCheckOperationCompleted;
		}
		InvokeAsync("MES_CarrierIn_JustCheck", new object[1] { pParameter }, MES_CarrierIn_JustCheckOperationCompleted, userState);
	}

	private void OnMES_CarrierIn_JustCheckOperationCompleted(object arg)
	{
		if (this.MES_CarrierIn_JustCheckCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.MES_CarrierIn_JustCheckCompleted(this, new MES_CarrierIn_JustCheckCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/MES_CheckEdcRawData", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string MES_CheckEdcRawData(string pParameter)
	{
		return (string)Invoke("MES_CheckEdcRawData", new object[1] { pParameter })[0];
	}

	public void MES_CheckEdcRawDataAsync(string pParameter)
	{
		MES_CheckEdcRawDataAsync(pParameter, null);
	}

	public void MES_CheckEdcRawDataAsync(string pParameter, object userState)
	{
		if (MES_CheckEdcRawDataOperationCompleted == null)
		{
			MES_CheckEdcRawDataOperationCompleted = OnMES_CheckEdcRawDataOperationCompleted;
		}
		InvokeAsync("MES_CheckEdcRawData", new object[1] { pParameter }, MES_CheckEdcRawDataOperationCompleted, userState);
	}

	private void OnMES_CheckEdcRawDataOperationCompleted(object arg)
	{
		if (this.MES_CheckEdcRawDataCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.MES_CheckEdcRawDataCompleted(this, new MES_CheckEdcRawDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("CyntecMES/WOBomQry", RequestNamespace = "CyntecMES", ResponseNamespace = "CyntecMES", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string WOBomQry(string pParameter)
	{
		return (string)Invoke("WOBomQry", new object[1] { pParameter })[0];
	}

	public void WOBomQryAsync(string pParameter)
	{
		WOBomQryAsync(pParameter, null);
	}

	public void WOBomQryAsync(string pParameter, object userState)
	{
		if (WOBomQryOperationCompleted == null)
		{
			WOBomQryOperationCompleted = OnWOBomQryOperationCompleted;
		}
		InvokeAsync("WOBomQry", new object[1] { pParameter }, WOBomQryOperationCompleted, userState);
	}

	private void OnWOBomQryOperationCompleted(object arg)
	{
		if (this.WOBomQryCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.WOBomQryCompleted(this, new WOBomQryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	public new void CancelAsync(object userState)
	{
		base.CancelAsync(userState);
	}

	private bool IsLocalFileSystemWebService(string url)
	{
		if (url == null || url == string.Empty)
		{
			return false;
		}
		Uri wsUri = new Uri(url);
		if (wsUri.Port >= 1024 && string.Compare(wsUri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0)
		{
			return true;
		}
		return false;
	}
}
