using System.CodeDom.Compiler;
using System.Data;
using System.ServiceModel;

namespace cyntec.MES.serviceEqpEF;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://CyntecMES", ConfigurationName = "serviceEqpEF.Eqp_EFSoap")]
public interface Eqp_EFSoap
{
	[OperationContract(Action = "http://CyntecMES/ComponentNoNLabelIDCheck", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfBoolean ComponentNoNLabelIDCheck(string woNo, string componentNo, string labelContents, bool isEqpAutoMode, string userID);

	[OperationContract(Action = "http://CyntecMES/ComponentLabelCheckInfo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfCompLabelCheckInfo ComponentLabelCheckInfo(string woNo, string userID);

	[OperationContract(Action = "http://CyntecMES/WO_Component_Add", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result WO_Component_Add(string pEqpNo, string pWONO, string pComponentNo, string pUserID);

	[OperationContract(Action = "http://CyntecMES/WO_Component_Add_WithWONO", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result WO_Component_Add_WithWONO(string pWONO, string pComponentNo, string pUserID);

	[OperationContract(Action = "http://CyntecMES/WOComponentQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOComponentQry WOComponentQry(string pEqpNo, string pUserID, string pComponentNo);

	[OperationContract(Action = "http://CyntecMES/WOComponentQrySimple", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOComponentQry WOComponentQrySimple(string pEqpNo, string pUserID, string pComponentNo);

	[OperationContract(Action = "http://CyntecMES/WOComponentCheckIn", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOComponentCheckIn WOComponentCheckIn(string pEqpNo, string pWONO, string pUserID, string pComponentNo, string pRefWO, string pRefWOGroupID, bool pIsCheckEmpCert, MatItem[] pMatItemList, WOCarrierItem[] pWOCarrierList, WOInputItem[] pWOInputItemList);

	[OperationContract(Action = "http://CyntecMES/WOComponentCheckInAll", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result WOComponentCheckInAll(string pEqpNo, string pWONO, string pUserID, string pRefWO, string pRefWOGroupID, bool pisJustCheck, bool pIsCheckEmpCert, MatItem[] pMatItemList, WOCarrierItem[] pWOCarrierList, WOInputItem[] pWOInputItemList, string pComponentNo);

	[OperationContract(Action = "http://CyntecMES/WOComponentCheckOut", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOComponentCheckOut WOComponentCheckOut(string pEqpNo, string pWONO, string pUserID, string pComponentNo, int pComponentNGQty, WOComponentConfirmNGRecord[] pComponentNGList);

	[OperationContract(Action = "http://CyntecMES/WOComponentCheckOutAll", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOComponentCheckOut WOComponentCheckOutAll(string pEqpNo, string pWONO, string pUserID, string pComponentNo, int pComponentNGQty, WOComponentConfirmNGRecord[] pComponentNGList, string pRemark, string pRefWO, string pRefWOGroupID, WOInputItem[] pWOInputItemList);

	[OperationContract(Action = "http://CyntecMES/WOCheckOutCreateSPCData", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOCheckOutCreateSPCData WOCheckOutCreateSPCData(string pEqpNo, string pWONO, string pUserID);

	[OperationContract(Action = "http://CyntecMES/GetEqpValue", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_GetEqpValue GetEqpValue(string pEqpNo, string pVarName);

	[OperationContract(Action = "http://CyntecMES/WorkOrderComponentListQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WorkOrderComponentListQry WorkOrderComponentListQry(string pWONO, string pUserID, bool IsCreate);

	[OperationContract(Action = "http://CyntecMES/CheckUser", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result CheckUser(string pUserID);

	[OperationContract(Action = "http://CyntecMES/CheckUserCert", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result CheckUserCert(string pUserID, string pWorkCenterNo);

	[OperationContract(Action = "http://CyntecMES/CheckEdcRawData", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result CheckEdcRawData(string pWONo, string pWorkCenterNo, string pMachNo, string pCustomField1);

	[OperationContract(Action = "http://CyntecMES/GetAvailableWOList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfListOfWorkOrder GetAvailableWOList(string userID, string eqpNo);

	[OperationContract(Action = "http://CyntecMES/CreateTrackingCode", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfListOfTrackingCodeList CreateTrackingCode(string userID, string eqpNo, string woNo, string[] componentList);

	[OperationContract(Action = "http://CyntecMES/CheckTrackingCode", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfListOfTrackingCodeList CheckTrackingCode(string userID, string eqpNo, string woNo, TrackingCodeList[] trackCodeList);

	[OperationContract(Action = "http://CyntecMES/TrackingCodeQryComponentNo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfListOfTrackingCodeList TrackingCodeQryComponentNo(string userID, TrackingCodeList[] trackCodeList, string WONo);

	[OperationContract(Action = "http://CyntecMES/ComponentNoQryTrackingCode", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfListOfTrackingCodeList ComponentNoQryTrackingCode(string userID, TrackingCodeList[] componentNoList);

	[OperationContract(Action = "http://CyntecMES/GetNumbering", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_Get_Numbering GetNumbering(string pKeyType, string pKeyPeriod, string pUserID, string pPrefix, string pRemark);

	[OperationContract(Action = "http://CyntecMES/GetNumberingRollbackDelete", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_Get_Numbering_Rollback GetNumberingRollbackDelete(string pKeyType, string pKeyPeriod);

	[OperationContract(Action = "http://CyntecMES/GetNumberingRollbackUpdate", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_Get_Numbering_Rollback GetNumberingRollbackUpdate(LP_SerialNo model, ActionRecord actionRecord);

	[OperationContract(Action = "http://CyntecMES/GetTrackingCodePPPP", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_GetTrackingCodePPPP GetTrackingCodePPPP();

	[OperationContract(Action = "http://CyntecMES/UserQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_UserQry UserQry(string pUserID);

	[OperationContract(Action = "http://CyntecMES/TOS_WorkOrderQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_TOS_WOQry TOS_WorkOrderQry(string pToolNo);

	[OperationContract(Action = "http://CyntecMES/EqpQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_EqpQry EqpQry(string pEqpNo);

	[OperationContract(Action = "http://CyntecMES/TOS_WOLogQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_TOS_WOLogGet TOS_WOLogQry(string pEqpNo, string pWONo, string pWorkCenterNo);

	[OperationContract(Action = "http://CyntecMES/CarrierIn", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result CarrierIn(string pWONo, int pFlowID, string pToolNo, string pUserID, string pEqpNo, bool pIsCycle);

	[OperationContract(Action = "http://CyntecMES/EdcDataQuery", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_EdcDataQuery EdcDataQuery(string pWONO, string pIsAvailable, string pFlowID, string pWorkcenterNo, string pEdcPlanSysID, string pEdcRawdataSummarySysID, string pComponentID, string pEqpNo, string[] pRefFieldCodeList);

	[OperationContract(Action = "http://CyntecMES/AddWOMachineRecord", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result AddWOMachineRecord(string pWONo, string pUserID, string pEqpNo);

	[OperationContract(Action = "http://CyntecMES/WOCompsEdcRawDataGet", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_EdcRawdataGet WOCompsEdcRawDataGet(string pEqpNo, string pWONO, string pUserID);

	[OperationContract(Action = "http://CyntecMES/WOCompsCSM_SAIC_LableGet", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_CSM_SAIC_LableGet WOCompsCSM_SAIC_LableGet(string pEqpNo, string pUserID, string pTrackingCode, string pWONo);

	[OperationContract(Action = "http://CyntecMES/GetWOMarkingData", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_MarkingIDList GetWOMarkingData(string pEqpNo, string pWONO, string pComponentNo, string pUserID, string pTargetType, int pRequestQty, bool pUpdateSeqNum, bool pAutoGenCompID, bool pAutoCompCfmIN, bool pAutoCompCfmOut);

	[OperationContract(Action = "http://CyntecMES/CreateWOComponentCustID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfListOfWOComponentCust CreateWOComponentCustID(string userID, string eqpNo, string woNo, WOComponentCustIDList[] componentList);

	[OperationContract(Action = "http://CyntecMES/CheckWOComponentCustID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfListOfWOComponentCustIDList CheckWOComponentCustID(string userID, string eqpNo, string woNo, string custType, WOComponentCustIDList[] pInputDataList);

	[OperationContract(Action = "http://CyntecMES/WOComponentCustIDQryComponentNo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfListOfWOComponentCustIDList WOComponentCustIDQryComponentNo(string userID, WOComponentCustIDList[] trackCodeList, string woNo, string custType);

	[OperationContract(Action = "http://CyntecMES/ComponentNoQryWOComponentCustID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfListOfWOComponentCustIDList ComponentNoQryWOComponentCustID(string userID, WOComponentCustIDList[] componentNoList, string custType);

	[OperationContract(Action = "http://CyntecMES/InsertEQP_ActionLog", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfBoolean InsertEQP_ActionLog(EqpActionLog model);

	[OperationContract(Action = "http://CyntecMES/WOCompsCSM_TESLA_LableGet", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_CSM_TESLA_LableGet WOCompsCSM_TESLA_LableGet(string pEqpNo, string pUserID, string pSerialNo, string pWONo, string pLabelType);

	[OperationContract(Action = "http://CyntecMES/ComponentLabelCheckComplete", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfBoolean ComponentLabelCheckComplete(string woNo, string userID);

	[OperationContract(Action = "http://CyntecMES/ComponentRelaction", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfBoolean ComponentRelaction(string eqpNo, string WONo, string userID, WOComponentRelation[] listComponentRelation);

	[OperationContract(Action = "http://CyntecMES/GetComponentLabelCheckList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfComponentLabelCheckList GetComponentLabelCheckList(string woNo, string userID);

	[OperationContract(Action = "http://CyntecMES/InsertEqpTransactionLog", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfBoolean InsertEqpTransactionLog(string logID, string logType, string transactionSide, string macID, string wono, string eqpNo, string transactionName, string parameterJSON, string resultJSON, string userID);

	[OperationContract(Action = "http://CyntecMES/GetWOReelIDList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_GetWOReelIDList GetWOReelIDList(string pWONO);

	[OperationContract(Action = "http://CyntecMES/GetDecodeBarcode", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_GetDecodeBarcode GetDecodeBarcode(string QRCode);

	[OperationContract(Action = "http://CyntecMES/StdCheckItemsQuery", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfListOfStdCheckItemsQueryItem StdCheckItemsQuery(string eqpNo, string WONo, string userID);

	[OperationContract(Action = "http://CyntecMES/StdPartsQuery", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfStdPartsMaster StdPartsQuery(string eqpNo, string WONo, string userID, string stdPartsID);

	[OperationContract(Action = "http://CyntecMES/StdPartsResultUpload", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfStdPartsMaster StdPartsResultUpload(string eqpNo, string WONo, string userID, string stdPartsID, StdPartsResultItem[] resultItems);

	[OperationContract(Action = "http://CyntecMES/UserVerify", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_UserVerify UserVerify(string pUser2DBarcode, string pUserID, string pUserPassword);

	[OperationContract(Action = "http://CyntecMES/SMT_Component_Add", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result SMT_Component_Add(string pWONO, string pComponentNo, int pFlowID, string pUserID, int pQty, int pInitQty, int pInitNgQty);

	[OperationContract(Action = "http://CyntecMES/SMT_Component_SetNG", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_SMT_Component_SetNG SMT_Component_SetNG(string pWONO, string pComponentNo, int pFlowID, string pUserID, SMT_NGItem[] pNGList);

	[OperationContract(Action = "http://CyntecMES/SMT_Component_Confirm", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_SMT_WOComponentConfirm SMT_Component_Confirm(string pWONO, string pComponentNo, int pFlowID, string pUserID, SMT_NGItem[] pNGList, string pEqpNo, MatItem[] pMatItemList);

	[OperationContract(Action = "http://CyntecMES/SMT_Component_Summary_Qry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_SMT_Component_Summary_Qry SMT_Component_Summary_Qry(string pWONO, int pFlowID, string pUserID, bool pWithComponentList, bool pWithComponentConfirmList, bool pWithComponentNGList);

	[OperationContract(Action = "http://CyntecMES/SMT_AOI_Component_Confirm", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_SMT_AOI_WOComponentConfirm SMT_AOI_Component_Confirm(string pWONO, string pComponentNo, int pFlowID, string pUserID, string pEqpNo, MatItem[] pMatItemList);

	[OperationContract(Action = "http://CyntecMES/WOQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOQry WOQry(string pEqpNo, string pWONO);

	[OperationContract(Action = "http://CyntecMES/WOQryByWeb", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOQry WOQryByWeb(string pWONO);

	[OperationContract(Action = "http://CyntecMES/WOQryByFlowID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOQry WOQryByFlowID(string pEqpNo, int pFlowID, string pWONO);

	[OperationContract(Action = "http://CyntecMES/WOParmQryByRefFieldCode", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOParmQryByRefFieldCode WOParmQryByRefFieldCode(string pEqpNo, string pWONO, string pRefFieldCode);

	[OperationContract(Action = "http://CyntecMES/WOParmQryByParamCode", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOParmQryByParamCode WOParmQryByParamCode(string pEqpNo, string pWONO, string pParamCode);

	[OperationContract(Action = "http://CyntecMES/WOParmQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOParmQry WOParmQry(string pEqpNo, string pWONO);

	[OperationContract(Action = "http://CyntecMES/WOCheckIn", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result WOCheckIn(string pEqpNo, string pWONO, string pUserID, string pRefWO, string pRefWOGroupID, bool pIsJustCheck, bool pIsCheckEmpCert, MatItem[] pMatItemList, WOCarrierItem[] pWOCarrierList, WOInputItem[] pWOInputItemList, WOParmValue[] pWOMgmtParmList, CheckItem[] pCheckItemList, TOSItem[] pTOSItemList, EqpItem[] pEqpItemList);

	[OperationContract(Action = "http://CyntecMES/WOCheckIn_Batch", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOCheckIn_Batch WOCheckIn_Batch(WOCheckItem[] pWOCheckItemList);

	[OperationContract(Action = "http://CyntecMES/WOCheckIn_Multi", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfBoolean WOCheckIn_Multi(string[] woNoList, WOInputItem[] inputList, MatItem[] traceBatchList, WOParmValue[] paramList, string[] machList, string userID, string refWONO, string refWOGroup);

	[OperationContract(Action = "http://CyntecMES/WO_InsertMachTOSItem", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result WO_InsertMachTOSItem(string pEqpNo, string pWONO, string pUserID, TOSItem[] pTOSItems);

	[OperationContract(Action = "http://CyntecMES/WO_InsertMachCheckItem", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result WO_InsertMachCheckItem(string pEqpNo, string pWONO, string pUserID, string pInOut, CheckItem[] pCheckItemList);

	[OperationContract(Action = "http://CyntecMES/WO_InsertUseMach", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	WO_InsertUseMachResponse WO_InsertUseMach(WO_InsertUseMachRequest request);

	[OperationContract(Action = "http://CyntecMES/EqpMatAdd", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result EqpMatAdd(string pEqpNo, string pWONO, MatItem[] pMatItemList, bool IsSkipRepeat);

	[OperationContract(Action = "http://CyntecMES/EqpWOInputAdd", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	EqpWOInputAddResponse EqpWOInputAdd(EqpWOInputAddRequest request);

	[OperationContract(Action = "http://CyntecMES/EqpCarrierAdd", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	EqpCarrierAddResponse EqpCarrierAdd(EqpCarrierAddRequest request);

	[OperationContract(Action = "http://CyntecMES/EqpWOParmValueAdd", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	EqpWOParmValueAddResponse EqpWOParmValueAdd(EqpWOParmValueAddRequest request);

	[OperationContract(Action = "http://CyntecMES/WOCheckOut", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result WOCheckOut(string pEqpNo, string pWONO, string pUserID, double pOutputQty, double pNGQty, string pRemark, string pRefWO, string pRefWOGroupID, double pRefOutPutQty, NGItem[] pWONGList, WOInputItem[] pWOInputItemList, bool isJustCheck, WOParmValue[] pWOMgmtParmList, CheckItem[] pCheckItemList, TOSItem[] pTOSItemList);

	[OperationContract(Action = "http://CyntecMES/WOCheckOut_Batch", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOCheckOut_Batch WOCheckOut_Batch(WOCheckOutItem[] pWOCheckOutItemList);

	[OperationContract(Action = "http://CyntecMES/WOCheckOut_Multi", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfBoolean WOCheckOut_Multi(string[] woNoList, WOInputItem[] inputList, WOParmValue[] paramList, DataSet NGDataSet, string userID, string refWONO, string refWOGroup);

	[OperationContract(Action = "http://CyntecMES/WOGroupGet", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOGroup WOGroupGet(string[] pWoNoList);

	[OperationContract(Action = "http://CyntecMES/TOS_MouldQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_TOS_MouldQry TOS_MouldQry(string pToolNo);

	[OperationContract(Action = "http://CyntecMES/TOS_MouldOnline", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result TOS_MouldOnline(string pEqpNo, string pToolNo, string pUserID);

	[OperationContract(Action = "http://CyntecMES/TOS_MouldOffLine", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result TOS_MouldOffLine(string pEqpNo, string pToolNo, decimal pUsageQty, string pUserID);

	[OperationContract(Action = "http://CyntecMES/EqpCheck", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result EqpCheck(string pEqpNo, EqpCheckItem[] pEqpCheckItemList);

	[OperationContract(Action = "http://CyntecMES/EqpStatusRefresh", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result EqpStatusRefresh(EqpStatus pEqpStatus, EqpParameter[] pParameterList, EqpPart[] pPartList);

	[OperationContract(Action = "http://CyntecMES/InsertParameter", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result InsertParameter(EqpParameter[] pParameterList);

	[OperationContract(Action = "http://CyntecMES/EqpPartProcess", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result EqpPartProcess(EqpPart[] pEqpPartList, bool IsUpdateStatus);

	[OperationContract(Action = "http://CyntecMES/EqpError_GetModelList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_EqpErrorCode_GetModelList EqpError_GetModelList(string pMachNo);

	[OperationContract(Action = "http://CyntecMES/EqpMaintance", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result EqpMaintance(string pEqpNo, string pUserID);

	[OperationContract(Action = "http://CyntecMES/EqpMaintanceEnd", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result EqpMaintanceEnd(string pEqpNo, string pUserID);

	[OperationContract(Action = "http://CyntecMES/DownloadEqpDllFile", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_DownloadEqpDllFile DownloadEqpDllFile(string pFileName);

	[OperationContract(Action = "http://CyntecMES/DownloadDll", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_DownloadDll DownloadDll(string pFileName);

	[OperationContract(Action = "http://CyntecMES/Eqp_CheckOffline_daily", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result Eqp_CheckOffline_daily();

	[OperationContract(Action = "http://CyntecMES/DynamicNG_Exist", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result DynamicNG_Exist(string pMachNo, int pWOConfirmID, string pNGCode);

	[OperationContract(Action = "http://CyntecMES/DynamicNG_Add", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result DynamicNG_Add(EqpDynamicNGRecord pModel);

	[OperationContract(Action = "http://CyntecMES/DynamicNG_Update", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result DynamicNG_Update(EqpDynamicNGRecord pModel);

	[OperationContract(Action = "http://CyntecMES/DynamicNG_Delete", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result DynamicNG_Delete(int pID);

	[OperationContract(Action = "http://CyntecMES/DynamicNG_GetModelList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_EqpDynamicNGRecord_GetModelList DynamicNG_GetModelList(string pMachNo, string pWONO);

	[OperationContract(Action = "http://CyntecMES/UploadTestResultFile", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	UploadTestResultFileResponse UploadTestResultFile(UploadTestResultFileRequest request);

	[OperationContract(Action = "http://CyntecMES/EqpMeasurementRecord", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result EqpMeasurementRecord(EqpMeasurementValue[] pMeasurementList);

	[OperationContract(Action = "http://CyntecMES/EqpMeasurementRecordWithUserID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result EqpMeasurementRecordWithUserID(string pEqpNo, string pWONO, EqpMeasurementValue[] pMeasurementList, string pUserID);

	[OperationContract(Action = "http://CyntecMES/EDCDataAdd", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result EDCDataAdd(string pEqpNo, string pWONO, string pUserID, Input_EDC_RawData[] pEDCRawData);

	[OperationContract(Action = "http://CyntecMES/EDCDataAdd_New", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result EDCDataAdd_New(string pEqpNo, string pWONO, string pUserID, Input_EDC_RawData_New[] pEDCRawData, bool isLast, bool isComponentMode);

	[OperationContract(Action = "http://CyntecMES/EDCDataIsLastCheck", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result EDCDataIsLastCheck(string pEqpNo, string pWONO, string pUserID, Result_WOQry cls_WOQryResult, Input_EDC_RawData_New[] pEDCRawData);

	[OperationContract(Action = "http://CyntecMES/EDCDataAdd_FAI", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result EDCDataAdd_FAI(string pEqpNo, string pWONO, string pUserID, Input_EDC_RawData_New[] pEDCRawData, bool isLast, bool isComponentMode);

	[OperationContract(Action = "http://CyntecMES/EDCPlanResultGet_FAI", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_EDCPlanResultGet_FAI EDCPlanResultGet_FAI(string pEqpNo, string pWONO, string pUserID, string[] pPlanTypeArray);

	[OperationContract(Action = "http://CyntecMES/WOComponentQryWithComponent", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOComponentQryWithComponent WOComponentQryWithComponent(string pComponentNo, string pUserID);

	[OperationContract(Action = "http://CyntecMES/WorkOrderQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WorkOrderQry WorkOrderQry(string pWONO, string pUserID);

	[OperationContract(Action = "http://CyntecMES/WorkOrderQryWithComponent", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WorkOrderQryWithComponent WorkOrderQryWithComponent(string pWONO, string pUserID, bool IsCreate);

	[OperationContract(Action = "http://CyntecMES/WO_Component_Add_Web", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WO_Component_Add_Web WO_Component_Add_Web(string pWONO, string pComponentNo, string pUserID);

	[OperationContract(Action = "http://CyntecMES/WO_Component_Insert_Web", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfString WO_Component_Insert_Web(string woNo, string componentNo, string userID);

	[OperationContract(Action = "http://CyntecMES/WOComponentCheckIn_Web", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOComponentCheckIn WOComponentCheckIn_Web(string pWONO, string pComponentNo, string pUserID);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckIn_Batch", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_ComponentCheckIn_Batch ComponentCheckIn_Batch(string pWONO, string pUserID);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckIn_Batch_RollBack", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentCheckIn_Batch_RollBack(Result_ComponentCheckIn_Batch pResult);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckOut_Batch", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_ComponentCheckOut_Batch ComponentCheckOut_Batch(string pWONO, string pUserID, int pNGQty);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckOut_Batch_RollBack", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentCheckOut_Batch_RollBack(Result_ComponentCheckOut_Batch pResult);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckIn_Cancel", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_ComponentCheckIn_Cancel ComponentCheckIn_Cancel(string pWONO, string pUserID);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckIn_Cancel_RollBack", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentCheckIn_Cancel_RollBack(Result_ComponentCheckIn_Cancel pResult);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckIn_QReturn", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_ComponentCheckIn_QReturn ComponentCheckIn_QReturn(string pWONO, string pUserID);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckOut_NG", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_ComponentCheckOut_NG ComponentCheckOut_NG(string pSerialNo, string pNGCode, string pUserID);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckOut_NG_RollBack", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentCheckOut_NG_RollBack(Result_ComponentCheckOut_NG pResult);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckOut_Less", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_ComponentCheckOut_Less ComponentCheckOut_Less(string pSerialNo, string pUserID);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckOut_Less_RollBack", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentCheckOut_Less_RollBack(Result_ComponentCheckOut_Less pResult);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckOut_Less_ChangeBack", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_ComponentCheckOut_Less_ChangeBack ComponentCheckOut_Less_ChangeBack(string pSerialNo, string pUserID);

	[OperationContract(Action = "http://CyntecMES/ComponentCheckOut_Less_Restore_RollBack", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentCheckOut_Less_Restore_RollBack(Result_ComponentCheckOut_Less_ChangeBack pResult);

	[OperationContract(Action = "http://CyntecMES/Component_MakeUp", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_Component_MakeUp Component_MakeUp(string pSerialNo, string pNewWONO, string pUserID);

	[OperationContract(Action = "http://CyntecMES/Component_MakeUp_RollBack", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result Component_MakeUp_RollBack(Result_Component_MakeUp pResult);

	[OperationContract(Action = "http://CyntecMES/Component_MakeUp_ChangeBack", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_Component_MakeUp_ChangeBack Component_MakeUp_ChangeBack(string pSerialNo, string pUserID);

	[OperationContract(Action = "http://CyntecMES/Component_MakeUp_ChangeBack_RollBack", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result Component_MakeUp_ChangeBack_RollBack(Result_Component_MakeUp_ChangeBack pResult);

	[OperationContract(Action = "http://CyntecMES/ComponentStatusChange", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	BasicResultOfString ComponentStatusChange(string pSerialNo, ComponentStatus statusCode, string userID);

	[OperationContract(Action = "http://CyntecMES/Component_Return", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_Component_Return Component_Return(string pSerialNo, string pUserID);

	[OperationContract(Action = "http://CyntecMES/Component_SetNG", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_Component_SetNG Component_SetNG(string pSerialNo, int pWOConfirmID, string pNGCode, string pUserID);

	[OperationContract(Action = "http://CyntecMES/Component_LabelCheck", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result Component_LabelCheck(string pSerialNo, string pQRCode, string pUserID);

	[OperationContract(Action = "http://CyntecMES/ComponentLabel_ExistByLabelID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentLabel_ExistByLabelID(string sLabelID);

	[OperationContract(Action = "http://CyntecMES/ComponentLabel_ExistBySerialNo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentLabel_ExistBySerialNo(string pSerialNo);

	[OperationContract(Action = "http://CyntecMES/ComponentLabel_Add", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentLabel_Add(WOComponentLabel pModel);

	[OperationContract(Action = "http://CyntecMES/ComponentLabel_Update", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentLabel_Update(WOComponentLabel pModel);

	[OperationContract(Action = "http://CyntecMES/ComponentLabel_GetModel", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOComponentLabel_GetModel ComponentLabel_GetModel(string pLabelID);

	[OperationContract(Action = "http://CyntecMES/ComponentLabel_GetModelByReadQR", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOComponentLabel_GetModel ComponentLabel_GetModelByReadQR(string pLabelQRData);

	[OperationContract(Action = "http://CyntecMES/ComponentLabel_GetModelListBySerialNo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOComponentLabel_GetModelList ComponentLabel_GetModelListBySerialNo(string pComponentNo);

	[OperationContract(Action = "http://CyntecMES/ComponentLabel_GetActiveLableListBySerialNo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_WOComponentLabel_GetModelList ComponentLabel_GetActiveLableListBySerialNo(string pComponentNo);

	[OperationContract(Action = "http://CyntecMES/ComponentLabel_Lable_Scan_Update", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentLabel_Lable_Scan_Update(string labelID, string userID, bool isEqpAutoMode, bool isTrayCheck, bool isEQA);

	[OperationContract(Action = "http://CyntecMES/ComponentLabel_Lable_Scan_Cancel", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentLabel_Lable_Scan_Cancel(string pLabelID, string pUserID, bool pIsEqpAutoMode, bool pIsTrayCheck, bool pIsEQA);

	[OperationContract(Action = "http://CyntecMES/ComponentLabel_Rework_WO_Update", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Result_ComponentLabel_Rework_WO_Update ComponentLabel_Rework_WO_Update(string pComponentNo, string pUserID, string pNewWoNo);

	[OperationContract(Action = "http://CyntecMES/ComponentLabel_Rework_WO_Update_Rollback", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(TOS_Master))]
	Basic_Result ComponentLabel_Rework_WO_Update_Rollback(Result_ComponentLabel_Rework_WO_Update pResult);
}
