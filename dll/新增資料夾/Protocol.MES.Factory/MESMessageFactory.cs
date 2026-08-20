using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.corp.delta.twcynmes01;
using Protocol.MES.Enums;
using Protocol.MES.Messages;
using Protocol.MES.Properties;
using Protocol.MES.Utils;

namespace Protocol.MES.Factory;

public static class MESMessageFactory
{
	private static string url = Settings.Default.Protocol_MES_corp_delta_twcynmes01_Eqp_Portal;

	private static string restBaseUrl = Settings.Default.RestBaseUrl;

	public static MESBaseMessage Create(string json)
	{
		dynamic val = JsonConvert.DeserializeObject(json);
		string text = val.TransactionName;
		if (!Enum.TryParse<MESTransactionName>(text.ToString(), out var result))
		{
			throw new Exception("Unknown MES Transaction: " + text);
		}
		return result switch
		{
			MESTransactionName.CONNECTION => MESJsonHelper.Deserialize<CONNECTIONReply>(json), 
			MESTransactionName.WOQRY => MESJsonHelper.Deserialize<WOQRYReply>(json), 
			MESTransactionName.WOCHECKIN => MESJsonHelper.Deserialize<WOCHECKINReply>(json), 
			MESTransactionName.WOCHECKOUT => MESJsonHelper.Deserialize<WOCHECKOUTReply>(json), 
			MESTransactionName.WOPARMQRY => MESJsonHelper.Deserialize<WOPARMQRYReply>(json), 
			MESTransactionName.EQPSTATUS => MESJsonHelper.Deserialize<EQPSTATUSReply>(json), 
			MESTransactionName.MATADD => MESJsonHelper.Deserialize<MATADDReply>(json), 
			MESTransactionName.UPLOADTESTFILE => MESJsonHelper.Deserialize<UPLOADTESTFILEReply>(json), 
			MESTransactionName.WOCHECKIN_MULTI => MESJsonHelper.Deserialize<WOCHECKIN_MULTIReply>(json), 
			MESTransactionName.WOCHECKOUT_MULTI => MESJsonHelper.Deserialize<WOCHECKOUT_MULTIReply>(json), 
			MESTransactionName.EQPPARTREFRESH => MESJsonHelper.Deserialize<EQPPARTREFRESHReply>(json), 
			MESTransactionName.EQPMOULDQRY => MESJsonHelper.Deserialize<EQPMOULDQRYReply>(json), 
			MESTransactionName.EQPMOULDONLINE => MESJsonHelper.Deserialize<EQPMOULDONLINEReply>(json), 
			MESTransactionName.EQPMOULDOFFLINE => MESJsonHelper.Deserialize<EQPMOULDOFFLINEReply>(json), 
			MESTransactionName.FAIMEASUREMENT => MESJsonHelper.Deserialize<FAIMEASUREMENTReply>(json), 
			MESTransactionName.EQPMAINTANCE => MESJsonHelper.Deserialize<EQPMAINTANCEReply>(json), 
			MESTransactionName.EQPMAINTANCEEND => MESJsonHelper.Deserialize<EQPMAINTANCEENDReply>(json), 
			MESTransactionName.COMPONENTQUERY => MESJsonHelper.Deserialize<COMPONENTQUERYReply>(json), 
			MESTransactionName.COMPONENTCONFIRMIN => MESJsonHelper.Deserialize<COMPONENTCONFIRMINReply>(json), 
			MESTransactionName.COMPONENTCONFIRMOUT => MESJsonHelper.Deserialize<COMPONENTCONFIRMOUTReply>(json), 
			MESTransactionName.EDCDATAADD => MESJsonHelper.Deserialize<EDCDATAADDReply>(json), 
			MESTransactionName.EDCDATAQUERY => MESJsonHelper.Deserialize<EDCDATAQUERYReply>(json), 
			MESTransactionName.CreateTrackingCode => MESJsonHelper.Deserialize<CreateTrackingCodeReply>(json), 
			MESTransactionName.TrackingCodeCheck => MESJsonHelper.Deserialize<TrackingCodeCheckReply>(json), 
			MESTransactionName.TrackingCodeQryComponentNo => MESJsonHelper.Deserialize<TrackingCodeQryComponentNoReply>(json), 
			MESTransactionName.ComponentNoQryTrackingCode => MESJsonHelper.Deserialize<ComponentNoQryTrackingCodeReply>(json), 
			MESTransactionName.TrackingCodeQryLabelInfo => MESJsonHelper.Deserialize<TrackingCodeQryLabelInfoReply>(json), 
			MESTransactionName.USERQRY => MESJsonHelper.Deserialize<USERQRYReply>(json), 
			MESTransactionName.WOBOMQRY => MESJsonHelper.Deserialize<WOBOMQRYReply>(json), 
			MESTransactionName.COMPONENTQRYWONO => MESJsonHelper.Deserialize<COMPONENTQRYWONOReply>(json), 
			MESTransactionName.COMPONENTRELATION => MESJsonHelper.Deserialize<COMPONENTRELATIONReply>(json), 
			MESTransactionName.GETMARKINGRULEIDLIST => MESJsonHelper.Deserialize<GETMARKINGRULEIDLISTReply>(json), 
			MESTransactionName.COMPONENTCREATEPOSTCHECK => MESJsonHelper.Deserialize<COMPONENTCREATEPOSTCHECKReply>(json), 
			MESTransactionName.GETWOCOMPONENTLIST => MESJsonHelper.Deserialize<GETWOCOMPONENTLISTReply>(json), 
			MESTransactionName.CompLabelCheckInfo => MESJsonHelper.Deserialize<CompLabelCheckInfoReply>(json), 
			MESTransactionName.CompLabelCheck => MESJsonHelper.Deserialize<CompLabelCheckReply>(json), 
			MESTransactionName.GetComponentLabelCheckList => MESJsonHelper.Deserialize<GetComponentLabelCheckListReply>(json), 
			MESTransactionName.ComponentLabelCheckComplete => MESJsonHelper.Deserialize<ComponentLabelCheckCompleteReply>(json), 
			MESTransactionName.SMTWOBOMQRY => MESJsonHelper.Deserialize<SMTWOBOMQRYReply>(json), 
			MESTransactionName.UserVerify => MESJsonHelper.Deserialize<UserVerifyReply>(json), 
			MESTransactionName.ComponentNG => MESJsonHelper.Deserialize<ComponentNGReply>(json), 
			MESTransactionName.GetCheckTask => MESJsonHelper.Deserialize<GetCheckTaskReply>(json), 
			MESTransactionName.EqpCheck => MESJsonHelper.Deserialize<EqpCheckReply>(json), 
			MESTransactionName.ReelGetDecodeBarcode => MESJsonHelper.Deserialize<ReelGetDecodeBarcodeReply>(json), 
			MESTransactionName.ReelGetReelByID => MESJsonHelper.Deserialize<ReelGetReelByIDReply>(json), 
			MESTransactionName.ReelUpdateTPReelID => MESJsonHelper.Deserialize<ReelUpdateTPReelIDReply>(json), 
			MESTransactionName.ReelAddLog => MESJsonHelper.Deserialize<ReelAddLogReply>(json), 
			MESTransactionName.ReelWOReelCheckSave => MESJsonHelper.Deserialize<ReelWOReelCheckSaveReply>(json), 
			MESTransactionName.StdPartsQuery => MESJsonHelper.Deserialize<StdPartsQueryReply>(json), 
			MESTransactionName.StdPartsResultUpload => MESJsonHelper.Deserialize<StdPartsResultUploadReply>(json), 
			MESTransactionName.StdCheckItemsQuery => MESJsonHelper.Deserialize<StdCheckItemsQueryReply>(json), 
			MESTransactionName.WOMachineIn => MESJsonHelper.Deserialize<WOMachineInReply>(json), 
			MESTransactionName.WOMachineOut => MESJsonHelper.Deserialize<WOMachineOutReply>(json), 
			MESTransactionName.ComponentAddChilds => MESJsonHelper.Deserialize<ComponentAddChildsReply>(json), 
			MESTransactionName.ComponentGetChilds => MESJsonHelper.Deserialize<ComponentGetChildsReply>(json), 
			MESTransactionName.EdcPlanGet => MESJsonHelper.Deserialize<EdcPlanGetReply>(json), 
			MESTransactionName.EqpMouldOfflineReasonQry => MESJsonHelper.Deserialize<EqpMouldOfflineReasonQryReply>(json), 
			MESTransactionName.SMTComponentConfirm => MESJsonHelper.Deserialize<SMTComponentConfirmReply>(json), 
			MESTransactionName.BatchNo531Qry => MESJsonHelper.Deserialize<BatchNo531QryReply>(json), 
			MESTransactionName.MiniOhmToolingQry => MESJsonHelper.Deserialize<MiniOhmToolingQryReply>(json), 
			MESTransactionName.GetWOReelIDList => MESJsonHelper.Deserialize<GetWOReelIDListReply>(json), 
			MESTransactionName.WOComponentQrySimple => MESJsonHelper.Deserialize<WOComponentQrySimpleReply>(json), 
			MESTransactionName.EqpLiquidDataReport => MESJsonHelper.Deserialize<EqpLiquidDataReportReply>(json), 
			MESTransactionName.ComponentCheckOut_Less => MESJsonHelper.Deserialize<ComponentCheckOut_LessReply>(json), 
			MESTransactionName.Component_MakeUp => MESJsonHelper.Deserialize<Component_MakeUpReply>(json), 
			MESTransactionName.CompLabelBatchCheck => MESJsonHelper.Deserialize<CompLabelBatchCheckReply>(json), 
			MESTransactionName.ComponentCheckOut_Less_ChangeBack => MESJsonHelper.Deserialize<ComponentCheckOut_Less_ChangeBackReply>(json), 
			MESTransactionName.Component_MakeUp_ChangeBack => MESJsonHelper.Deserialize<Component_MakeUp_ChangeBackReply>(json), 
			MESTransactionName.UploadReeIDComponentRelation => MESJsonHelper.Deserialize<UploadReeIDComponentRelationReply>(json), 
			MESTransactionName.ReportComponentMarkingData => MESJsonHelper.Deserialize<ReportComponentMarkingDataReply>(json), 
			MESTransactionName.GetWOComponentMarkingData => MESJsonHelper.Deserialize<GetWOComponentMarkingDataReply>(json), 
			MESTransactionName.WorkOrderComponentBound => MESJsonHelper.Deserialize<WorkOrderComponentBoundReply>(json), 
			MESTransactionName.GetWOMachineRecord => MESJsonHelper.Deserialize<GetWOMachineRecordReply>(json), 
			MESTransactionName.TOS_WOQRY => MESJsonHelper.Deserialize<TOS_WOQRYReply>(json), 
			MESTransactionName.CARRIERIN => MESJsonHelper.Deserialize<CARRIERINReply>(json), 
			MESTransactionName.ComponentBatchIn => MESJsonHelper.Deserialize<ComponentBatchInReply>(json), 
			MESTransactionName.ComponentBatchOut => MESJsonHelper.Deserialize<ComponentBatchOutReply>(json), 
			MESTransactionName.ReportComponentLabelData => MESJsonHelper.Deserialize<ReportComponentLabelDataReply>(json), 
			MESTransactionName.GetWOComponentLabelData => MESJsonHelper.Deserialize<GetWOComponentLabelDataReply>(json), 
			MESTransactionName.ComponentCheckOut_NG => MESJsonHelper.Deserialize<ComponentCheckOut_NGReply>(json), 
			MESTransactionName.WOParmQryNotByEqpNo => MESJsonHelper.Deserialize<WOParmQryNotByEqpNoReply>(json), 
			MESTransactionName.WO_Component_Add => MESJsonHelper.Deserialize<WO_Component_AddReply>(json), 
			MESTransactionName.WOInfoQry => MESJsonHelper.Deserialize<WOInfoQryReply>(json), 
			MESTransactionName.GetWOMachineList => MESJsonHelper.Deserialize<GetWOMachineListReply>(json), 
			MESTransactionName.GetWORouting => MESJsonHelper.Deserialize<GetWORoutingReply>(json), 
			MESTransactionName.GetCompConfirmStatus => MESJsonHelper.Deserialize<GetCompConfirmStatusReply>(json), 
			MESTransactionName.TraceMatRecipeQuery => MESJsonHelper.Deserialize<TraceMatRecipeQueryReply>(json), 
			MESTransactionName.TraceMatCreate => MESJsonHelper.Deserialize<TraceMatCreateReply>(json), 
			MESTransactionName.TraceMatQuery => MESJsonHelper.Deserialize<TraceMatQueryReply>(json), 
			MESTransactionName.WOMaterialCheck => MESJsonHelper.Deserialize<WOMaterialCheckReply>(json), 
			MESTransactionName.SMTRawMatSetNG => MESJsonHelper.Deserialize<SMTRawMatSetNGReply>(json), 
			MESTransactionName.UpdateReelStatus => MESJsonHelper.Deserialize<UpdateReelStatusReply>(json), 
			MESTransactionName.UploadCMK => MESJsonHelper.Deserialize<UploadCMKReply>(json), 
			MESTransactionName.EqpWOInputAdd => MESJsonHelper.Deserialize<EqpWOInputAddReply>(json), 
			MESTransactionName.EqpWONGAdd => MESJsonHelper.Deserialize<EqpWONGAddReply>(json), 
			MESTransactionName.GetLeaveWOList => MESJsonHelper.Deserialize<GetLeaveWOListReply>(json), 
			MESTransactionName.TOS_GetToolListByWo => MESJsonHelper.Deserialize<TOS_GetToolListByWoReply>(json), 
			MESTransactionName.GetLeftoverWONo => MESJsonHelper.Deserialize<GetLeftoverWONoReply>(json), 
			MESTransactionName.AddProductLeftover => MESJsonHelper.Deserialize<AddProductLeftoverReply>(json), 
			MESTransactionName.UseProductLeftover => MESJsonHelper.Deserialize<UseProductLeftoverReply>(json), 
			MESTransactionName.QueryMatMaster => MESJsonHelper.Deserialize<QueryMatMasterReply>(json), 
			MESTransactionName.GetTestProgram => MESJsonHelper.Deserialize<GetTestProgramReply>(json), 
			_ => throw new NotImplementedException("Transaction " + text + " not implemented"), 
		};
	}

	public static TReply CallMes<TRequest, TReply>(TRequest request) where TRequest : MESBaseMessage where TReply : MESBaseMessage
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		if (IsRestTransaction(request.TransactionName))
		{
			return CallMesRest<TRequest, TReply>(request);
		}
		string pParameter = MESJsonHelper.Serialize(request);
		string result;
		try
		{
			Eqp_PortalSoapClient eqp_PortalSoapClient = new Eqp_PortalSoapClient(Eqp_PortalSoapClient.EndpointConfiguration.Eqp_PortalSoap, url);
			try
			{
				result = eqp_PortalSoapClient.EqpTransactionAsync(pParameter).GetAwaiter().GetResult();
			}
			finally
			{
				((IDisposable)eqp_PortalSoapClient)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Call MES SOAP failed: " + ex.Message, ex);
		}
		return MESJsonHelper.Deserialize<TReply>(result);
	}

	private static bool IsRestTransaction(MESTransactionName name)
	{
		return name == MESTransactionName.GetCheckTask || name == MESTransactionName.EqpCheck;
	}

	private static TReply CallMesRest<TRequest, TReply>(TRequest request) where TRequest : MESBaseMessage where TReply : MESBaseMessage
	{
		string restEndpoint = GetRestEndpoint(request.TransactionName);
		string content = MESJsonHelper.Serialize(request);
		using HttpClient httpClient = new HttpClient();
		string s = Settings.Default.RestUser + ":" + Settings.Default.RestPassword;
		string parameter = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", parameter);
		StringContent content2 = new StringContent(content, Encoding.UTF8, "application/json");
		HttpResponseMessage result = httpClient.PostAsync(restBaseUrl + restEndpoint, content2).Result;
		if (!result.IsSuccessStatusCode)
		{
			throw new Exception($"Call MES REST failed: {result.StatusCode}");
		}
		string result2 = result.Content.ReadAsStringAsync().Result;
		return MESJsonHelper.Deserialize<TReply>(result2);
	}

	private static string GetRestEndpoint(MESTransactionName name)
	{
		return name switch
		{
			MESTransactionName.GetCheckTask => "Eqp/GetCheckTask", 
			MESTransactionName.EqpCheck => "Eqp/EqpCheck", 
			_ => string.Empty, 
		};
	}
}
