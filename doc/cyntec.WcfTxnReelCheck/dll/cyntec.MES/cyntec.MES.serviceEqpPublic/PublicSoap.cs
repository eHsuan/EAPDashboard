using System.CodeDom.Compiler;
using System.Data;
using System.ServiceModel;

namespace cyntec.MES.serviceEqpPublic;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "CyntecMES", ConfigurationName = "serviceEqpPublic.PublicSoap")]
public interface PublicSoap
{
	[OperationContract(Action = "CyntecMES/SendMail", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	string SendMail(string pParameter);

	[OperationContract(Action = "CyntecMES/SendMail_EF", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	Basic_Result SendMail_EF(string pSubject, string pBody, string[] pMailToList, string[] pCCToList, string[] pBCCToList, AttachmentFile[] pAttachmentFileList);

	[OperationContract(Action = "CyntecMES/CalculateMWD", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	string CalculateMWD(string pParameter);

	[OperationContract(Action = "CyntecMES/CalculateMWD_NEW", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	string CalculateMWD_NEW(string pParameter);

	[OperationContract(Action = "CyntecMES/GetDataTable", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	string GetDataTable(string pParameter);

	[OperationContract(Action = "CyntecMES/GetDataTable_EF", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	Result_GetDataTable_EF GetDataTable_EF(string pSqlcommand);

	[OperationContract(Action = "CyntecMES/GetCodeTableData", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	string GetCodeTableData(string pParameter);

	[OperationContract(Action = "CyntecMES/NotificationMail", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	Result_RptMailUser_Notification NotificationMail(string pMatGroupNo, string pPrgName, string pTriggerReason);

	[OperationContract(Action = "CyntecMES/WOParmGetList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	Result_WOParm WOParmGetList(string pWoNo);

	[OperationContract(Action = "CyntecMES/WOParmGetListByMatNo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	Result_WOParm WOParmGetListByMatNo(string pWoNo, string pMatNo);

	[OperationContract(Action = "CyntecMES/SqlGetDataTable", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	DataSet SqlGetDataTable(string pSqlcommand);

	[OperationContract(Action = "CyntecMES/SqlGetDataTableWithSQLParameter", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	DataSet SqlGetDataTableWithSQLParameter(string pSqlcommand, SqlParameter[] pSQLParameterList);

	[OperationContract(Action = "CyntecMES/ExecuteDataNonQuery", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	int ExecuteDataNonQuery(string pSqlcommand);

	[OperationContract(Action = "CyntecMES/ExecuteDataNonQueryWithSQLParameter", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	int ExecuteDataNonQueryWithSQLParameter(string pSqlcommand, SqlParameter[] pSQLParameterList);

	[OperationContract(Action = "CyntecMES/ExecuteDataNonQueryWithSQLParameter1", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	string ExecuteDataNonQueryWithSQLParameter1(string pSqlcommand, SqlParameter[] pSQLParameterList);

	[OperationContract(Action = "CyntecMES/ExecuteDataScalar", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	object ExecuteDataScalar(string pSqlcommand);

	[OperationContract(Action = "CyntecMES/ExecuteDataScalarWithSQLParameter", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(SqlParameter[]))]
	object ExecuteDataScalarWithSQLParameter(string pSqlcommand, SqlParameter[] pSQLParameterList);
}
