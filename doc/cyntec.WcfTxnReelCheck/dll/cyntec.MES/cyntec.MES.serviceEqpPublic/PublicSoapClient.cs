using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace cyntec.MES.serviceEqpPublic;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class PublicSoapClient : ClientBase<PublicSoap>, PublicSoap
{
	public PublicSoapClient()
	{
	}

	public PublicSoapClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public PublicSoapClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public PublicSoapClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public PublicSoapClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public string SendMail(string pParameter)
	{
		return base.Channel.SendMail(pParameter);
	}

	public Basic_Result SendMail_EF(string pSubject, string pBody, string[] pMailToList, string[] pCCToList, string[] pBCCToList, AttachmentFile[] pAttachmentFileList)
	{
		return base.Channel.SendMail_EF(pSubject, pBody, pMailToList, pCCToList, pBCCToList, pAttachmentFileList);
	}

	public string CalculateMWD(string pParameter)
	{
		return base.Channel.CalculateMWD(pParameter);
	}

	public string CalculateMWD_NEW(string pParameter)
	{
		return base.Channel.CalculateMWD_NEW(pParameter);
	}

	public string GetDataTable(string pParameter)
	{
		return base.Channel.GetDataTable(pParameter);
	}

	public Result_GetDataTable_EF GetDataTable_EF(string pSqlcommand)
	{
		return base.Channel.GetDataTable_EF(pSqlcommand);
	}

	public string GetCodeTableData(string pParameter)
	{
		return base.Channel.GetCodeTableData(pParameter);
	}

	public Result_RptMailUser_Notification NotificationMail(string pMatGroupNo, string pPrgName, string pTriggerReason)
	{
		return base.Channel.NotificationMail(pMatGroupNo, pPrgName, pTriggerReason);
	}

	public Result_WOParm WOParmGetList(string pWoNo)
	{
		return base.Channel.WOParmGetList(pWoNo);
	}

	public Result_WOParm WOParmGetListByMatNo(string pWoNo, string pMatNo)
	{
		return base.Channel.WOParmGetListByMatNo(pWoNo, pMatNo);
	}

	public DataSet SqlGetDataTable(string pSqlcommand)
	{
		return base.Channel.SqlGetDataTable(pSqlcommand);
	}

	public DataSet SqlGetDataTableWithSQLParameter(string pSqlcommand, SqlParameter[] pSQLParameterList)
	{
		return base.Channel.SqlGetDataTableWithSQLParameter(pSqlcommand, pSQLParameterList);
	}

	public int ExecuteDataNonQuery(string pSqlcommand)
	{
		return base.Channel.ExecuteDataNonQuery(pSqlcommand);
	}

	public int ExecuteDataNonQueryWithSQLParameter(string pSqlcommand, SqlParameter[] pSQLParameterList)
	{
		return base.Channel.ExecuteDataNonQueryWithSQLParameter(pSqlcommand, pSQLParameterList);
	}

	public string ExecuteDataNonQueryWithSQLParameter1(string pSqlcommand, SqlParameter[] pSQLParameterList)
	{
		return base.Channel.ExecuteDataNonQueryWithSQLParameter1(pSqlcommand, pSQLParameterList);
	}

	public object ExecuteDataScalar(string pSqlcommand)
	{
		return base.Channel.ExecuteDataScalar(pSqlcommand);
	}

	public object ExecuteDataScalarWithSQLParameter(string pSqlcommand, SqlParameter[] pSQLParameterList)
	{
		return base.Channel.ExecuteDataScalarWithSQLParameter(pSqlcommand, pSQLParameterList);
	}
}
