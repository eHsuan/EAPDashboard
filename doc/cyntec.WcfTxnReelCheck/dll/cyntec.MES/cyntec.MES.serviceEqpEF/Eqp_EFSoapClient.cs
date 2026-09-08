using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace cyntec.MES.serviceEqpEF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class Eqp_EFSoapClient : ClientBase<Eqp_EFSoap>, Eqp_EFSoap
{
	public Eqp_EFSoapClient()
	{
	}

	public Eqp_EFSoapClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public Eqp_EFSoapClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public Eqp_EFSoapClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public Eqp_EFSoapClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public BasicResultOfBoolean ComponentNoNLabelIDCheck(string woNo, string componentNo, string labelContents, bool isEqpAutoMode, string userID)
	{
		return base.Channel.ComponentNoNLabelIDCheck(woNo, componentNo, labelContents, isEqpAutoMode, userID);
	}

	public BasicResultOfCompLabelCheckInfo ComponentLabelCheckInfo(string woNo, string userID)
	{
		return base.Channel.ComponentLabelCheckInfo(woNo, userID);
	}

	public Basic_Result WO_Component_Add(string pEqpNo, string pWONO, string pComponentNo, string pUserID)
	{
		return base.Channel.WO_Component_Add(pEqpNo, pWONO, pComponentNo, pUserID);
	}

	public Basic_Result WO_Component_Add_WithWONO(string pWONO, string pComponentNo, string pUserID)
	{
		return base.Channel.WO_Component_Add_WithWONO(pWONO, pComponentNo, pUserID);
	}

	public Result_WOComponentQry WOComponentQry(string pEqpNo, string pUserID, string pComponentNo)
	{
		return base.Channel.WOComponentQry(pEqpNo, pUserID, pComponentNo);
	}

	public Result_WOComponentQry WOComponentQrySimple(string pEqpNo, string pUserID, string pComponentNo)
	{
		return base.Channel.WOComponentQrySimple(pEqpNo, pUserID, pComponentNo);
	}

	public Result_WOComponentCheckIn WOComponentCheckIn(string pEqpNo, string pWONO, string pUserID, string pComponentNo, string pRefWO, string pRefWOGroupID, bool pIsCheckEmpCert, MatItem[] pMatItemList, WOCarrierItem[] pWOCarrierList, WOInputItem[] pWOInputItemList)
	{
		return base.Channel.WOComponentCheckIn(pEqpNo, pWONO, pUserID, pComponentNo, pRefWO, pRefWOGroupID, pIsCheckEmpCert, pMatItemList, pWOCarrierList, pWOInputItemList);
	}

	public Basic_Result WOComponentCheckInAll(string pEqpNo, string pWONO, string pUserID, string pRefWO, string pRefWOGroupID, bool pisJustCheck, bool pIsCheckEmpCert, MatItem[] pMatItemList, WOCarrierItem[] pWOCarrierList, WOInputItem[] pWOInputItemList, string pComponentNo)
	{
		return base.Channel.WOComponentCheckInAll(pEqpNo, pWONO, pUserID, pRefWO, pRefWOGroupID, pisJustCheck, pIsCheckEmpCert, pMatItemList, pWOCarrierList, pWOInputItemList, pComponentNo);
	}

	public Result_WOComponentCheckOut WOComponentCheckOut(string pEqpNo, string pWONO, string pUserID, string pComponentNo, int pComponentNGQty, WOComponentConfirmNGRecord[] pComponentNGList)
	{
		return base.Channel.WOComponentCheckOut(pEqpNo, pWONO, pUserID, pComponentNo, pComponentNGQty, pComponentNGList);
	}

	public Result_WOComponentCheckOut WOComponentCheckOutAll(string pEqpNo, string pWONO, string pUserID, string pComponentNo, int pComponentNGQty, WOComponentConfirmNGRecord[] pComponentNGList, string pRemark, string pRefWO, string pRefWOGroupID, WOInputItem[] pWOInputItemList)
	{
		return base.Channel.WOComponentCheckOutAll(pEqpNo, pWONO, pUserID, pComponentNo, pComponentNGQty, pComponentNGList, pRemark, pRefWO, pRefWOGroupID, pWOInputItemList);
	}

	public Result_WOCheckOutCreateSPCData WOCheckOutCreateSPCData(string pEqpNo, string pWONO, string pUserID)
	{
		return base.Channel.WOCheckOutCreateSPCData(pEqpNo, pWONO, pUserID);
	}

	public Result_GetEqpValue GetEqpValue(string pEqpNo, string pVarName)
	{
		return base.Channel.GetEqpValue(pEqpNo, pVarName);
	}

	public Result_WorkOrderComponentListQry WorkOrderComponentListQry(string pWONO, string pUserID, bool IsCreate)
	{
		return base.Channel.WorkOrderComponentListQry(pWONO, pUserID, IsCreate);
	}

	public Basic_Result CheckUser(string pUserID)
	{
		return base.Channel.CheckUser(pUserID);
	}

	public Basic_Result CheckUserCert(string pUserID, string pWorkCenterNo)
	{
		return base.Channel.CheckUserCert(pUserID, pWorkCenterNo);
	}

	public Basic_Result CheckEdcRawData(string pWONo, string pWorkCenterNo, string pMachNo, string pCustomField1)
	{
		return base.Channel.CheckEdcRawData(pWONo, pWorkCenterNo, pMachNo, pCustomField1);
	}

	public BasicResultOfListOfWorkOrder GetAvailableWOList(string userID, string eqpNo)
	{
		return base.Channel.GetAvailableWOList(userID, eqpNo);
	}

	public BasicResultOfListOfTrackingCodeList CreateTrackingCode(string userID, string eqpNo, string woNo, string[] componentList)
	{
		return base.Channel.CreateTrackingCode(userID, eqpNo, woNo, componentList);
	}

	public BasicResultOfListOfTrackingCodeList CheckTrackingCode(string userID, string eqpNo, string woNo, TrackingCodeList[] trackCodeList)
	{
		return base.Channel.CheckTrackingCode(userID, eqpNo, woNo, trackCodeList);
	}

	public BasicResultOfListOfTrackingCodeList TrackingCodeQryComponentNo(string userID, TrackingCodeList[] trackCodeList, string WONo)
	{
		return base.Channel.TrackingCodeQryComponentNo(userID, trackCodeList, WONo);
	}

	public BasicResultOfListOfTrackingCodeList ComponentNoQryTrackingCode(string userID, TrackingCodeList[] componentNoList)
	{
		return base.Channel.ComponentNoQryTrackingCode(userID, componentNoList);
	}

	public Result_Get_Numbering GetNumbering(string pKeyType, string pKeyPeriod, string pUserID, string pPrefix, string pRemark)
	{
		return base.Channel.GetNumbering(pKeyType, pKeyPeriod, pUserID, pPrefix, pRemark);
	}

	public Result_Get_Numbering_Rollback GetNumberingRollbackDelete(string pKeyType, string pKeyPeriod)
	{
		return base.Channel.GetNumberingRollbackDelete(pKeyType, pKeyPeriod);
	}

	public Result_Get_Numbering_Rollback GetNumberingRollbackUpdate(LP_SerialNo model, ActionRecord actionRecord)
	{
		return base.Channel.GetNumberingRollbackUpdate(model, actionRecord);
	}

	public Result_GetTrackingCodePPPP GetTrackingCodePPPP()
	{
		return base.Channel.GetTrackingCodePPPP();
	}

	public Result_UserQry UserQry(string pUserID)
	{
		return base.Channel.UserQry(pUserID);
	}

	public Result_TOS_WOQry TOS_WorkOrderQry(string pToolNo)
	{
		return base.Channel.TOS_WorkOrderQry(pToolNo);
	}

	public Result_EqpQry EqpQry(string pEqpNo)
	{
		return base.Channel.EqpQry(pEqpNo);
	}

	public Result_TOS_WOLogGet TOS_WOLogQry(string pEqpNo, string pWONo, string pWorkCenterNo)
	{
		return base.Channel.TOS_WOLogQry(pEqpNo, pWONo, pWorkCenterNo);
	}

	public Basic_Result CarrierIn(string pWONo, int pFlowID, string pToolNo, string pUserID, string pEqpNo, bool pIsCycle)
	{
		return base.Channel.CarrierIn(pWONo, pFlowID, pToolNo, pUserID, pEqpNo, pIsCycle);
	}

	public Result_EdcDataQuery EdcDataQuery(string pWONO, string pIsAvailable, string pFlowID, string pWorkcenterNo, string pEdcPlanSysID, string pEdcRawdataSummarySysID, string pComponentID, string pEqpNo, string[] pRefFieldCodeList)
	{
		return base.Channel.EdcDataQuery(pWONO, pIsAvailable, pFlowID, pWorkcenterNo, pEdcPlanSysID, pEdcRawdataSummarySysID, pComponentID, pEqpNo, pRefFieldCodeList);
	}

	public Basic_Result AddWOMachineRecord(string pWONo, string pUserID, string pEqpNo)
	{
		return base.Channel.AddWOMachineRecord(pWONo, pUserID, pEqpNo);
	}

	public Result_EdcRawdataGet WOCompsEdcRawDataGet(string pEqpNo, string pWONO, string pUserID)
	{
		return base.Channel.WOCompsEdcRawDataGet(pEqpNo, pWONO, pUserID);
	}

	public Result_CSM_SAIC_LableGet WOCompsCSM_SAIC_LableGet(string pEqpNo, string pUserID, string pTrackingCode, string pWONo)
	{
		return base.Channel.WOCompsCSM_SAIC_LableGet(pEqpNo, pUserID, pTrackingCode, pWONo);
	}

	public Result_MarkingIDList GetWOMarkingData(string pEqpNo, string pWONO, string pComponentNo, string pUserID, string pTargetType, int pRequestQty, bool pUpdateSeqNum, bool pAutoGenCompID, bool pAutoCompCfmIN, bool pAutoCompCfmOut)
	{
		return base.Channel.GetWOMarkingData(pEqpNo, pWONO, pComponentNo, pUserID, pTargetType, pRequestQty, pUpdateSeqNum, pAutoGenCompID, pAutoCompCfmIN, pAutoCompCfmOut);
	}

	public BasicResultOfListOfWOComponentCust CreateWOComponentCustID(string userID, string eqpNo, string woNo, WOComponentCustIDList[] componentList)
	{
		return base.Channel.CreateWOComponentCustID(userID, eqpNo, woNo, componentList);
	}

	public BasicResultOfListOfWOComponentCustIDList CheckWOComponentCustID(string userID, string eqpNo, string woNo, string custType, WOComponentCustIDList[] pInputDataList)
	{
		return base.Channel.CheckWOComponentCustID(userID, eqpNo, woNo, custType, pInputDataList);
	}

	public BasicResultOfListOfWOComponentCustIDList WOComponentCustIDQryComponentNo(string userID, WOComponentCustIDList[] trackCodeList, string woNo, string custType)
	{
		return base.Channel.WOComponentCustIDQryComponentNo(userID, trackCodeList, woNo, custType);
	}

	public BasicResultOfListOfWOComponentCustIDList ComponentNoQryWOComponentCustID(string userID, WOComponentCustIDList[] componentNoList, string custType)
	{
		return base.Channel.ComponentNoQryWOComponentCustID(userID, componentNoList, custType);
	}

	public BasicResultOfBoolean InsertEQP_ActionLog(EqpActionLog model)
	{
		return base.Channel.InsertEQP_ActionLog(model);
	}

	public Result_CSM_TESLA_LableGet WOCompsCSM_TESLA_LableGet(string pEqpNo, string pUserID, string pSerialNo, string pWONo, string pLabelType)
	{
		return base.Channel.WOCompsCSM_TESLA_LableGet(pEqpNo, pUserID, pSerialNo, pWONo, pLabelType);
	}

	public BasicResultOfBoolean ComponentLabelCheckComplete(string woNo, string userID)
	{
		return base.Channel.ComponentLabelCheckComplete(woNo, userID);
	}

	public BasicResultOfBoolean ComponentRelaction(string eqpNo, string WONo, string userID, WOComponentRelation[] listComponentRelation)
	{
		return base.Channel.ComponentRelaction(eqpNo, WONo, userID, listComponentRelation);
	}

	public BasicResultOfComponentLabelCheckList GetComponentLabelCheckList(string woNo, string userID)
	{
		return base.Channel.GetComponentLabelCheckList(woNo, userID);
	}

	public BasicResultOfBoolean InsertEqpTransactionLog(string logID, string logType, string transactionSide, string macID, string wono, string eqpNo, string transactionName, string parameterJSON, string resultJSON, string userID)
	{
		return base.Channel.InsertEqpTransactionLog(logID, logType, transactionSide, macID, wono, eqpNo, transactionName, parameterJSON, resultJSON, userID);
	}

	public Result_GetWOReelIDList GetWOReelIDList(string pWONO)
	{
		return base.Channel.GetWOReelIDList(pWONO);
	}

	public Result_GetDecodeBarcode GetDecodeBarcode(string QRCode)
	{
		return base.Channel.GetDecodeBarcode(QRCode);
	}

	public BasicResultOfListOfStdCheckItemsQueryItem StdCheckItemsQuery(string eqpNo, string WONo, string userID)
	{
		return base.Channel.StdCheckItemsQuery(eqpNo, WONo, userID);
	}

	public BasicResultOfStdPartsMaster StdPartsQuery(string eqpNo, string WONo, string userID, string stdPartsID)
	{
		return base.Channel.StdPartsQuery(eqpNo, WONo, userID, stdPartsID);
	}

	public BasicResultOfStdPartsMaster StdPartsResultUpload(string eqpNo, string WONo, string userID, string stdPartsID, StdPartsResultItem[] resultItems)
	{
		return base.Channel.StdPartsResultUpload(eqpNo, WONo, userID, stdPartsID, resultItems);
	}

	public Result_UserVerify UserVerify(string pUser2DBarcode, string pUserID, string pUserPassword)
	{
		return base.Channel.UserVerify(pUser2DBarcode, pUserID, pUserPassword);
	}

	public Basic_Result SMT_Component_Add(string pWONO, string pComponentNo, int pFlowID, string pUserID, int pQty, int pInitQty, int pInitNgQty)
	{
		return base.Channel.SMT_Component_Add(pWONO, pComponentNo, pFlowID, pUserID, pQty, pInitQty, pInitNgQty);
	}

	public Result_SMT_Component_SetNG SMT_Component_SetNG(string pWONO, string pComponentNo, int pFlowID, string pUserID, SMT_NGItem[] pNGList)
	{
		return base.Channel.SMT_Component_SetNG(pWONO, pComponentNo, pFlowID, pUserID, pNGList);
	}

	public Result_SMT_WOComponentConfirm SMT_Component_Confirm(string pWONO, string pComponentNo, int pFlowID, string pUserID, SMT_NGItem[] pNGList, string pEqpNo, MatItem[] pMatItemList)
	{
		return base.Channel.SMT_Component_Confirm(pWONO, pComponentNo, pFlowID, pUserID, pNGList, pEqpNo, pMatItemList);
	}

	public Result_SMT_Component_Summary_Qry SMT_Component_Summary_Qry(string pWONO, int pFlowID, string pUserID, bool pWithComponentList, bool pWithComponentConfirmList, bool pWithComponentNGList)
	{
		return base.Channel.SMT_Component_Summary_Qry(pWONO, pFlowID, pUserID, pWithComponentList, pWithComponentConfirmList, pWithComponentNGList);
	}

	public Result_SMT_AOI_WOComponentConfirm SMT_AOI_Component_Confirm(string pWONO, string pComponentNo, int pFlowID, string pUserID, string pEqpNo, MatItem[] pMatItemList)
	{
		return base.Channel.SMT_AOI_Component_Confirm(pWONO, pComponentNo, pFlowID, pUserID, pEqpNo, pMatItemList);
	}

	public Result_WOQry WOQry(string pEqpNo, string pWONO)
	{
		return base.Channel.WOQry(pEqpNo, pWONO);
	}

	public Result_WOQry WOQryByWeb(string pWONO)
	{
		return base.Channel.WOQryByWeb(pWONO);
	}

	public Result_WOQry WOQryByFlowID(string pEqpNo, int pFlowID, string pWONO)
	{
		return base.Channel.WOQryByFlowID(pEqpNo, pFlowID, pWONO);
	}

	public Result_WOParmQryByRefFieldCode WOParmQryByRefFieldCode(string pEqpNo, string pWONO, string pRefFieldCode)
	{
		return base.Channel.WOParmQryByRefFieldCode(pEqpNo, pWONO, pRefFieldCode);
	}

	public Result_WOParmQryByParamCode WOParmQryByParamCode(string pEqpNo, string pWONO, string pParamCode)
	{
		return base.Channel.WOParmQryByParamCode(pEqpNo, pWONO, pParamCode);
	}

	public Result_WOParmQry WOParmQry(string pEqpNo, string pWONO)
	{
		return base.Channel.WOParmQry(pEqpNo, pWONO);
	}

	public Basic_Result WOCheckIn(string pEqpNo, string pWONO, string pUserID, string pRefWO, string pRefWOGroupID, bool pIsJustCheck, bool pIsCheckEmpCert, MatItem[] pMatItemList, WOCarrierItem[] pWOCarrierList, WOInputItem[] pWOInputItemList, WOParmValue[] pWOMgmtParmList, CheckItem[] pCheckItemList, TOSItem[] pTOSItemList, EqpItem[] pEqpItemList)
	{
		return base.Channel.WOCheckIn(pEqpNo, pWONO, pUserID, pRefWO, pRefWOGroupID, pIsJustCheck, pIsCheckEmpCert, pMatItemList, pWOCarrierList, pWOInputItemList, pWOMgmtParmList, pCheckItemList, pTOSItemList, pEqpItemList);
	}

	public Result_WOCheckIn_Batch WOCheckIn_Batch(WOCheckItem[] pWOCheckItemList)
	{
		return base.Channel.WOCheckIn_Batch(pWOCheckItemList);
	}

	public BasicResultOfBoolean WOCheckIn_Multi(string[] woNoList, WOInputItem[] inputList, MatItem[] traceBatchList, WOParmValue[] paramList, string[] machList, string userID, string refWONO, string refWOGroup)
	{
		return base.Channel.WOCheckIn_Multi(woNoList, inputList, traceBatchList, paramList, machList, userID, refWONO, refWOGroup);
	}

	public Basic_Result WO_InsertMachTOSItem(string pEqpNo, string pWONO, string pUserID, TOSItem[] pTOSItems)
	{
		return base.Channel.WO_InsertMachTOSItem(pEqpNo, pWONO, pUserID, pTOSItems);
	}

	public Basic_Result WO_InsertMachCheckItem(string pEqpNo, string pWONO, string pUserID, string pInOut, CheckItem[] pCheckItemList)
	{
		return base.Channel.WO_InsertMachCheckItem(pEqpNo, pWONO, pUserID, pInOut, pCheckItemList);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	WO_InsertUseMachResponse Eqp_EFSoap.WO_InsertUseMach(WO_InsertUseMachRequest request)
	{
		return base.Channel.WO_InsertUseMach(request);
	}

	public Basic_Result WO_InsertUseMach(string pEqpNo, string pWONO, string pUserID, DateTime? ModifyDate, bool IsSkipRepeat)
	{
		WO_InsertUseMachRequest wO_InsertUseMachRequest = new WO_InsertUseMachRequest();
		wO_InsertUseMachRequest.pEqpNo = pEqpNo;
		wO_InsertUseMachRequest.pWONO = pWONO;
		wO_InsertUseMachRequest.pUserID = pUserID;
		wO_InsertUseMachRequest.ModifyDate = ModifyDate;
		wO_InsertUseMachRequest.IsSkipRepeat = IsSkipRepeat;
		WO_InsertUseMachResponse wO_InsertUseMachResponse = ((Eqp_EFSoap)this).WO_InsertUseMach(wO_InsertUseMachRequest);
		return wO_InsertUseMachResponse.WO_InsertUseMachResult;
	}

	public Basic_Result EqpMatAdd(string pEqpNo, string pWONO, MatItem[] pMatItemList, bool IsSkipRepeat)
	{
		return base.Channel.EqpMatAdd(pEqpNo, pWONO, pMatItemList, IsSkipRepeat);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	EqpWOInputAddResponse Eqp_EFSoap.EqpWOInputAdd(EqpWOInputAddRequest request)
	{
		return base.Channel.EqpWOInputAdd(request);
	}

	public Basic_Result EqpWOInputAdd(string pEqpNo, string pWONO, string pUserID, WOInputItem[] pWOInputItemList, DateTime? pModifyDate, bool IsSkipRepeat)
	{
		EqpWOInputAddRequest eqpWOInputAddRequest = new EqpWOInputAddRequest();
		eqpWOInputAddRequest.pEqpNo = pEqpNo;
		eqpWOInputAddRequest.pWONO = pWONO;
		eqpWOInputAddRequest.pUserID = pUserID;
		eqpWOInputAddRequest.pWOInputItemList = pWOInputItemList;
		eqpWOInputAddRequest.pModifyDate = pModifyDate;
		eqpWOInputAddRequest.IsSkipRepeat = IsSkipRepeat;
		EqpWOInputAddResponse eqpWOInputAddResponse = ((Eqp_EFSoap)this).EqpWOInputAdd(eqpWOInputAddRequest);
		return eqpWOInputAddResponse.EqpWOInputAddResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	EqpCarrierAddResponse Eqp_EFSoap.EqpCarrierAdd(EqpCarrierAddRequest request)
	{
		return base.Channel.EqpCarrierAdd(request);
	}

	public Basic_Result EqpCarrierAdd(string pEqpNo, string pWONO, WOCarrierItem[] pCarrierNoList, string pInOut, string pUserID, DateTime? pModifyDate, bool IsSkipRepeat)
	{
		EqpCarrierAddRequest eqpCarrierAddRequest = new EqpCarrierAddRequest();
		eqpCarrierAddRequest.pEqpNo = pEqpNo;
		eqpCarrierAddRequest.pWONO = pWONO;
		eqpCarrierAddRequest.pCarrierNoList = pCarrierNoList;
		eqpCarrierAddRequest.pInOut = pInOut;
		eqpCarrierAddRequest.pUserID = pUserID;
		eqpCarrierAddRequest.pModifyDate = pModifyDate;
		eqpCarrierAddRequest.IsSkipRepeat = IsSkipRepeat;
		EqpCarrierAddResponse eqpCarrierAddResponse = ((Eqp_EFSoap)this).EqpCarrierAdd(eqpCarrierAddRequest);
		return eqpCarrierAddResponse.EqpCarrierAddResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	EqpWOParmValueAddResponse Eqp_EFSoap.EqpWOParmValueAdd(EqpWOParmValueAddRequest request)
	{
		return base.Channel.EqpWOParmValueAdd(request);
	}

	public Basic_Result EqpWOParmValueAdd(string pEqpNo, string pWONO, string pUserID, string pInOut, WOParmValue[] pWOParmValueList, DateTime? pModifyDate, bool IsSkipRepeat)
	{
		EqpWOParmValueAddRequest eqpWOParmValueAddRequest = new EqpWOParmValueAddRequest();
		eqpWOParmValueAddRequest.pEqpNo = pEqpNo;
		eqpWOParmValueAddRequest.pWONO = pWONO;
		eqpWOParmValueAddRequest.pUserID = pUserID;
		eqpWOParmValueAddRequest.pInOut = pInOut;
		eqpWOParmValueAddRequest.pWOParmValueList = pWOParmValueList;
		eqpWOParmValueAddRequest.pModifyDate = pModifyDate;
		eqpWOParmValueAddRequest.IsSkipRepeat = IsSkipRepeat;
		EqpWOParmValueAddResponse eqpWOParmValueAddResponse = ((Eqp_EFSoap)this).EqpWOParmValueAdd(eqpWOParmValueAddRequest);
		return eqpWOParmValueAddResponse.EqpWOParmValueAddResult;
	}

	public Basic_Result WOCheckOut(string pEqpNo, string pWONO, string pUserID, double pOutputQty, double pNGQty, string pRemark, string pRefWO, string pRefWOGroupID, double pRefOutPutQty, NGItem[] pWONGList, WOInputItem[] pWOInputItemList, bool isJustCheck, WOParmValue[] pWOMgmtParmList, CheckItem[] pCheckItemList, TOSItem[] pTOSItemList)
	{
		return base.Channel.WOCheckOut(pEqpNo, pWONO, pUserID, pOutputQty, pNGQty, pRemark, pRefWO, pRefWOGroupID, pRefOutPutQty, pWONGList, pWOInputItemList, isJustCheck, pWOMgmtParmList, pCheckItemList, pTOSItemList);
	}

	public Result_WOCheckOut_Batch WOCheckOut_Batch(WOCheckOutItem[] pWOCheckOutItemList)
	{
		return base.Channel.WOCheckOut_Batch(pWOCheckOutItemList);
	}

	public BasicResultOfBoolean WOCheckOut_Multi(string[] woNoList, WOInputItem[] inputList, WOParmValue[] paramList, DataSet NGDataSet, string userID, string refWONO, string refWOGroup)
	{
		return base.Channel.WOCheckOut_Multi(woNoList, inputList, paramList, NGDataSet, userID, refWONO, refWOGroup);
	}

	public Result_WOGroup WOGroupGet(string[] pWoNoList)
	{
		return base.Channel.WOGroupGet(pWoNoList);
	}

	public Result_TOS_MouldQry TOS_MouldQry(string pToolNo)
	{
		return base.Channel.TOS_MouldQry(pToolNo);
	}

	public Basic_Result TOS_MouldOnline(string pEqpNo, string pToolNo, string pUserID)
	{
		return base.Channel.TOS_MouldOnline(pEqpNo, pToolNo, pUserID);
	}

	public Basic_Result TOS_MouldOffLine(string pEqpNo, string pToolNo, decimal pUsageQty, string pUserID)
	{
		return base.Channel.TOS_MouldOffLine(pEqpNo, pToolNo, pUsageQty, pUserID);
	}

	public Basic_Result EqpCheck(string pEqpNo, EqpCheckItem[] pEqpCheckItemList)
	{
		return base.Channel.EqpCheck(pEqpNo, pEqpCheckItemList);
	}

	public Basic_Result EqpStatusRefresh(EqpStatus pEqpStatus, EqpParameter[] pParameterList, EqpPart[] pPartList)
	{
		return base.Channel.EqpStatusRefresh(pEqpStatus, pParameterList, pPartList);
	}

	public Basic_Result InsertParameter(EqpParameter[] pParameterList)
	{
		return base.Channel.InsertParameter(pParameterList);
	}

	public Basic_Result EqpPartProcess(EqpPart[] pEqpPartList, bool IsUpdateStatus)
	{
		return base.Channel.EqpPartProcess(pEqpPartList, IsUpdateStatus);
	}

	public Result_EqpErrorCode_GetModelList EqpError_GetModelList(string pMachNo)
	{
		return base.Channel.EqpError_GetModelList(pMachNo);
	}

	public Basic_Result EqpMaintance(string pEqpNo, string pUserID)
	{
		return base.Channel.EqpMaintance(pEqpNo, pUserID);
	}

	public Basic_Result EqpMaintanceEnd(string pEqpNo, string pUserID)
	{
		return base.Channel.EqpMaintanceEnd(pEqpNo, pUserID);
	}

	public Result_DownloadEqpDllFile DownloadEqpDllFile(string pFileName)
	{
		return base.Channel.DownloadEqpDllFile(pFileName);
	}

	public Result_DownloadDll DownloadDll(string pFileName)
	{
		return base.Channel.DownloadDll(pFileName);
	}

	public Basic_Result Eqp_CheckOffline_daily()
	{
		return base.Channel.Eqp_CheckOffline_daily();
	}

	public Basic_Result DynamicNG_Exist(string pMachNo, int pWOConfirmID, string pNGCode)
	{
		return base.Channel.DynamicNG_Exist(pMachNo, pWOConfirmID, pNGCode);
	}

	public Basic_Result DynamicNG_Add(EqpDynamicNGRecord pModel)
	{
		return base.Channel.DynamicNG_Add(pModel);
	}

	public Basic_Result DynamicNG_Update(EqpDynamicNGRecord pModel)
	{
		return base.Channel.DynamicNG_Update(pModel);
	}

	public Basic_Result DynamicNG_Delete(int pID)
	{
		return base.Channel.DynamicNG_Delete(pID);
	}

	public Result_EqpDynamicNGRecord_GetModelList DynamicNG_GetModelList(string pMachNo, string pWONO)
	{
		return base.Channel.DynamicNG_GetModelList(pMachNo, pWONO);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	UploadTestResultFileResponse Eqp_EFSoap.UploadTestResultFile(UploadTestResultFileRequest request)
	{
		return base.Channel.UploadTestResultFile(request);
	}

	public Basic_Result UploadTestResultFile(string pEqpNo, string pWONO, string pFileName, byte[] pFile, string pUserID, string pTransferCode, string pComponentNo)
	{
		UploadTestResultFileRequest uploadTestResultFileRequest = new UploadTestResultFileRequest();
		uploadTestResultFileRequest.pEqpNo = pEqpNo;
		uploadTestResultFileRequest.pWONO = pWONO;
		uploadTestResultFileRequest.pFileName = pFileName;
		uploadTestResultFileRequest.pFile = pFile;
		uploadTestResultFileRequest.pUserID = pUserID;
		uploadTestResultFileRequest.pTransferCode = pTransferCode;
		uploadTestResultFileRequest.pComponentNo = pComponentNo;
		UploadTestResultFileResponse uploadTestResultFileResponse = ((Eqp_EFSoap)this).UploadTestResultFile(uploadTestResultFileRequest);
		return uploadTestResultFileResponse.UploadTestResultFileResult;
	}

	public Basic_Result EqpMeasurementRecord(EqpMeasurementValue[] pMeasurementList)
	{
		return base.Channel.EqpMeasurementRecord(pMeasurementList);
	}

	public Basic_Result EqpMeasurementRecordWithUserID(string pEqpNo, string pWONO, EqpMeasurementValue[] pMeasurementList, string pUserID)
	{
		return base.Channel.EqpMeasurementRecordWithUserID(pEqpNo, pWONO, pMeasurementList, pUserID);
	}

	public Basic_Result EDCDataAdd(string pEqpNo, string pWONO, string pUserID, Input_EDC_RawData[] pEDCRawData)
	{
		return base.Channel.EDCDataAdd(pEqpNo, pWONO, pUserID, pEDCRawData);
	}

	public Basic_Result EDCDataAdd_New(string pEqpNo, string pWONO, string pUserID, Input_EDC_RawData_New[] pEDCRawData, bool isLast, bool isComponentMode)
	{
		return base.Channel.EDCDataAdd_New(pEqpNo, pWONO, pUserID, pEDCRawData, isLast, isComponentMode);
	}

	public Basic_Result EDCDataIsLastCheck(string pEqpNo, string pWONO, string pUserID, Result_WOQry cls_WOQryResult, Input_EDC_RawData_New[] pEDCRawData)
	{
		return base.Channel.EDCDataIsLastCheck(pEqpNo, pWONO, pUserID, cls_WOQryResult, pEDCRawData);
	}

	public Basic_Result EDCDataAdd_FAI(string pEqpNo, string pWONO, string pUserID, Input_EDC_RawData_New[] pEDCRawData, bool isLast, bool isComponentMode)
	{
		return base.Channel.EDCDataAdd_FAI(pEqpNo, pWONO, pUserID, pEDCRawData, isLast, isComponentMode);
	}

	public Result_EDCPlanResultGet_FAI EDCPlanResultGet_FAI(string pEqpNo, string pWONO, string pUserID, string[] pPlanTypeArray)
	{
		return base.Channel.EDCPlanResultGet_FAI(pEqpNo, pWONO, pUserID, pPlanTypeArray);
	}

	public Result_WOComponentQryWithComponent WOComponentQryWithComponent(string pComponentNo, string pUserID)
	{
		return base.Channel.WOComponentQryWithComponent(pComponentNo, pUserID);
	}

	public Result_WorkOrderQry WorkOrderQry(string pWONO, string pUserID)
	{
		return base.Channel.WorkOrderQry(pWONO, pUserID);
	}

	public Result_WorkOrderQryWithComponent WorkOrderQryWithComponent(string pWONO, string pUserID, bool IsCreate)
	{
		return base.Channel.WorkOrderQryWithComponent(pWONO, pUserID, IsCreate);
	}

	public Result_WO_Component_Add_Web WO_Component_Add_Web(string pWONO, string pComponentNo, string pUserID)
	{
		return base.Channel.WO_Component_Add_Web(pWONO, pComponentNo, pUserID);
	}

	public BasicResultOfString WO_Component_Insert_Web(string woNo, string componentNo, string userID)
	{
		return base.Channel.WO_Component_Insert_Web(woNo, componentNo, userID);
	}

	public Result_WOComponentCheckIn WOComponentCheckIn_Web(string pWONO, string pComponentNo, string pUserID)
	{
		return base.Channel.WOComponentCheckIn_Web(pWONO, pComponentNo, pUserID);
	}

	public Result_ComponentCheckIn_Batch ComponentCheckIn_Batch(string pWONO, string pUserID)
	{
		return base.Channel.ComponentCheckIn_Batch(pWONO, pUserID);
	}

	public Basic_Result ComponentCheckIn_Batch_RollBack(Result_ComponentCheckIn_Batch pResult)
	{
		return base.Channel.ComponentCheckIn_Batch_RollBack(pResult);
	}

	public Result_ComponentCheckOut_Batch ComponentCheckOut_Batch(string pWONO, string pUserID, int pNGQty)
	{
		return base.Channel.ComponentCheckOut_Batch(pWONO, pUserID, pNGQty);
	}

	public Basic_Result ComponentCheckOut_Batch_RollBack(Result_ComponentCheckOut_Batch pResult)
	{
		return base.Channel.ComponentCheckOut_Batch_RollBack(pResult);
	}

	public Result_ComponentCheckIn_Cancel ComponentCheckIn_Cancel(string pWONO, string pUserID)
	{
		return base.Channel.ComponentCheckIn_Cancel(pWONO, pUserID);
	}

	public Basic_Result ComponentCheckIn_Cancel_RollBack(Result_ComponentCheckIn_Cancel pResult)
	{
		return base.Channel.ComponentCheckIn_Cancel_RollBack(pResult);
	}

	public Result_ComponentCheckIn_QReturn ComponentCheckIn_QReturn(string pWONO, string pUserID)
	{
		return base.Channel.ComponentCheckIn_QReturn(pWONO, pUserID);
	}

	public Result_ComponentCheckOut_NG ComponentCheckOut_NG(string pSerialNo, string pNGCode, string pUserID)
	{
		return base.Channel.ComponentCheckOut_NG(pSerialNo, pNGCode, pUserID);
	}

	public Basic_Result ComponentCheckOut_NG_RollBack(Result_ComponentCheckOut_NG pResult)
	{
		return base.Channel.ComponentCheckOut_NG_RollBack(pResult);
	}

	public Result_ComponentCheckOut_Less ComponentCheckOut_Less(string pSerialNo, string pUserID)
	{
		return base.Channel.ComponentCheckOut_Less(pSerialNo, pUserID);
	}

	public Basic_Result ComponentCheckOut_Less_RollBack(Result_ComponentCheckOut_Less pResult)
	{
		return base.Channel.ComponentCheckOut_Less_RollBack(pResult);
	}

	public Result_ComponentCheckOut_Less_ChangeBack ComponentCheckOut_Less_ChangeBack(string pSerialNo, string pUserID)
	{
		return base.Channel.ComponentCheckOut_Less_ChangeBack(pSerialNo, pUserID);
	}

	public Basic_Result ComponentCheckOut_Less_Restore_RollBack(Result_ComponentCheckOut_Less_ChangeBack pResult)
	{
		return base.Channel.ComponentCheckOut_Less_Restore_RollBack(pResult);
	}

	public Result_Component_MakeUp Component_MakeUp(string pSerialNo, string pNewWONO, string pUserID)
	{
		return base.Channel.Component_MakeUp(pSerialNo, pNewWONO, pUserID);
	}

	public Basic_Result Component_MakeUp_RollBack(Result_Component_MakeUp pResult)
	{
		return base.Channel.Component_MakeUp_RollBack(pResult);
	}

	public Result_Component_MakeUp_ChangeBack Component_MakeUp_ChangeBack(string pSerialNo, string pUserID)
	{
		return base.Channel.Component_MakeUp_ChangeBack(pSerialNo, pUserID);
	}

	public Basic_Result Component_MakeUp_ChangeBack_RollBack(Result_Component_MakeUp_ChangeBack pResult)
	{
		return base.Channel.Component_MakeUp_ChangeBack_RollBack(pResult);
	}

	public BasicResultOfString ComponentStatusChange(string pSerialNo, ComponentStatus statusCode, string userID)
	{
		return base.Channel.ComponentStatusChange(pSerialNo, statusCode, userID);
	}

	public Result_Component_Return Component_Return(string pSerialNo, string pUserID)
	{
		return base.Channel.Component_Return(pSerialNo, pUserID);
	}

	public Result_Component_SetNG Component_SetNG(string pSerialNo, int pWOConfirmID, string pNGCode, string pUserID)
	{
		return base.Channel.Component_SetNG(pSerialNo, pWOConfirmID, pNGCode, pUserID);
	}

	public Basic_Result Component_LabelCheck(string pSerialNo, string pQRCode, string pUserID)
	{
		return base.Channel.Component_LabelCheck(pSerialNo, pQRCode, pUserID);
	}

	public Basic_Result ComponentLabel_ExistByLabelID(string sLabelID)
	{
		return base.Channel.ComponentLabel_ExistByLabelID(sLabelID);
	}

	public Basic_Result ComponentLabel_ExistBySerialNo(string pSerialNo)
	{
		return base.Channel.ComponentLabel_ExistBySerialNo(pSerialNo);
	}

	public Basic_Result ComponentLabel_Add(WOComponentLabel pModel)
	{
		return base.Channel.ComponentLabel_Add(pModel);
	}

	public Basic_Result ComponentLabel_Update(WOComponentLabel pModel)
	{
		return base.Channel.ComponentLabel_Update(pModel);
	}

	public Result_WOComponentLabel_GetModel ComponentLabel_GetModel(string pLabelID)
	{
		return base.Channel.ComponentLabel_GetModel(pLabelID);
	}

	public Result_WOComponentLabel_GetModel ComponentLabel_GetModelByReadQR(string pLabelQRData)
	{
		return base.Channel.ComponentLabel_GetModelByReadQR(pLabelQRData);
	}

	public Result_WOComponentLabel_GetModelList ComponentLabel_GetModelListBySerialNo(string pComponentNo)
	{
		return base.Channel.ComponentLabel_GetModelListBySerialNo(pComponentNo);
	}

	public Result_WOComponentLabel_GetModelList ComponentLabel_GetActiveLableListBySerialNo(string pComponentNo)
	{
		return base.Channel.ComponentLabel_GetActiveLableListBySerialNo(pComponentNo);
	}

	public Basic_Result ComponentLabel_Lable_Scan_Update(string labelID, string userID, bool isEqpAutoMode, bool isTrayCheck, bool isEQA)
	{
		return base.Channel.ComponentLabel_Lable_Scan_Update(labelID, userID, isEqpAutoMode, isTrayCheck, isEQA);
	}

	public Basic_Result ComponentLabel_Lable_Scan_Cancel(string pLabelID, string pUserID, bool pIsEqpAutoMode, bool pIsTrayCheck, bool pIsEQA)
	{
		return base.Channel.ComponentLabel_Lable_Scan_Cancel(pLabelID, pUserID, pIsEqpAutoMode, pIsTrayCheck, pIsEQA);
	}

	public Result_ComponentLabel_Rework_WO_Update ComponentLabel_Rework_WO_Update(string pComponentNo, string pUserID, string pNewWoNo)
	{
		return base.Channel.ComponentLabel_Rework_WO_Update(pComponentNo, pUserID, pNewWoNo);
	}

	public Basic_Result ComponentLabel_Rework_WO_Update_Rollback(Result_ComponentLabel_Rework_WO_Update pResult)
	{
		return base.Channel.ComponentLabel_Rework_WO_Update_Rollback(pResult);
	}
}
