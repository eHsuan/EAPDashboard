using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace cyntec.MES.serviceModbus;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ModBusSoapClient : ClientBase<ModBusSoap>, ModBusSoap
{
	public ModBusSoapClient()
	{
	}

	public ModBusSoapClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ModBusSoapClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ModBusSoapClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ModBusSoapClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SelectEQP_IPPCMAPPINGHOSTResponse ModBusSoap.SelectEQP_IPPCMAPPINGHOST(SelectEQP_IPPCMAPPINGHOSTRequest request)
	{
		return base.Channel.SelectEQP_IPPCMAPPINGHOST(request);
	}

	public string SelectEQP_IPPCMAPPINGHOST(string pParameter)
	{
		SelectEQP_IPPCMAPPINGHOSTRequest selectEQP_IPPCMAPPINGHOSTRequest = new SelectEQP_IPPCMAPPINGHOSTRequest();
		selectEQP_IPPCMAPPINGHOSTRequest.Body = new SelectEQP_IPPCMAPPINGHOSTRequestBody();
		selectEQP_IPPCMAPPINGHOSTRequest.Body.pParameter = pParameter;
		SelectEQP_IPPCMAPPINGHOSTResponse selectEQP_IPPCMAPPINGHOSTResponse = ((ModBusSoap)this).SelectEQP_IPPCMAPPINGHOST(selectEQP_IPPCMAPPINGHOSTRequest);
		return selectEQP_IPPCMAPPINGHOSTResponse.Body.SelectEQP_IPPCMAPPINGHOSTResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	InsertEQP_EQP_IPPCMAPPINGHOSTResponse ModBusSoap.InsertEQP_EQP_IPPCMAPPINGHOST(InsertEQP_EQP_IPPCMAPPINGHOSTRequest request)
	{
		return base.Channel.InsertEQP_EQP_IPPCMAPPINGHOST(request);
	}

	public string InsertEQP_EQP_IPPCMAPPINGHOST(string pParameter)
	{
		InsertEQP_EQP_IPPCMAPPINGHOSTRequest insertEQP_EQP_IPPCMAPPINGHOSTRequest = new InsertEQP_EQP_IPPCMAPPINGHOSTRequest();
		insertEQP_EQP_IPPCMAPPINGHOSTRequest.Body = new InsertEQP_EQP_IPPCMAPPINGHOSTRequestBody();
		insertEQP_EQP_IPPCMAPPINGHOSTRequest.Body.pParameter = pParameter;
		InsertEQP_EQP_IPPCMAPPINGHOSTResponse insertEQP_EQP_IPPCMAPPINGHOSTResponse = ((ModBusSoap)this).InsertEQP_EQP_IPPCMAPPINGHOST(insertEQP_EQP_IPPCMAPPINGHOSTRequest);
		return insertEQP_EQP_IPPCMAPPINGHOSTResponse.Body.InsertEQP_EQP_IPPCMAPPINGHOSTResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	UpdateEQP_EQP_IPPCMAPPINGHOSTResponse ModBusSoap.UpdateEQP_EQP_IPPCMAPPINGHOST(UpdateEQP_EQP_IPPCMAPPINGHOSTRequest request)
	{
		return base.Channel.UpdateEQP_EQP_IPPCMAPPINGHOST(request);
	}

	public string UpdateEQP_EQP_IPPCMAPPINGHOST(string pParameter)
	{
		UpdateEQP_EQP_IPPCMAPPINGHOSTRequest updateEQP_EQP_IPPCMAPPINGHOSTRequest = new UpdateEQP_EQP_IPPCMAPPINGHOSTRequest();
		updateEQP_EQP_IPPCMAPPINGHOSTRequest.Body = new UpdateEQP_EQP_IPPCMAPPINGHOSTRequestBody();
		updateEQP_EQP_IPPCMAPPINGHOSTRequest.Body.pParameter = pParameter;
		UpdateEQP_EQP_IPPCMAPPINGHOSTResponse updateEQP_EQP_IPPCMAPPINGHOSTResponse = ((ModBusSoap)this).UpdateEQP_EQP_IPPCMAPPINGHOST(updateEQP_EQP_IPPCMAPPINGHOSTRequest);
		return updateEQP_EQP_IPPCMAPPINGHOSTResponse.Body.UpdateEQP_EQP_IPPCMAPPINGHOSTResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	DeleteEQP_EQP_IPPCMAPPINGHOSTResponse ModBusSoap.DeleteEQP_EQP_IPPCMAPPINGHOST(DeleteEQP_EQP_IPPCMAPPINGHOSTRequest request)
	{
		return base.Channel.DeleteEQP_EQP_IPPCMAPPINGHOST(request);
	}

	public string DeleteEQP_EQP_IPPCMAPPINGHOST(string pParameter)
	{
		DeleteEQP_EQP_IPPCMAPPINGHOSTRequest deleteEQP_EQP_IPPCMAPPINGHOSTRequest = new DeleteEQP_EQP_IPPCMAPPINGHOSTRequest();
		deleteEQP_EQP_IPPCMAPPINGHOSTRequest.Body = new DeleteEQP_EQP_IPPCMAPPINGHOSTRequestBody();
		deleteEQP_EQP_IPPCMAPPINGHOSTRequest.Body.pParameter = pParameter;
		DeleteEQP_EQP_IPPCMAPPINGHOSTResponse deleteEQP_EQP_IPPCMAPPINGHOSTResponse = ((ModBusSoap)this).DeleteEQP_EQP_IPPCMAPPINGHOST(deleteEQP_EQP_IPPCMAPPINGHOSTRequest);
		return deleteEQP_EQP_IPPCMAPPINGHOSTResponse.Body.DeleteEQP_EQP_IPPCMAPPINGHOSTResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SelectEQP_IPPCMAPPINGResponse ModBusSoap.SelectEQP_IPPCMAPPING(SelectEQP_IPPCMAPPINGRequest request)
	{
		return base.Channel.SelectEQP_IPPCMAPPING(request);
	}

	public string SelectEQP_IPPCMAPPING(string pParameter)
	{
		SelectEQP_IPPCMAPPINGRequest selectEQP_IPPCMAPPINGRequest = new SelectEQP_IPPCMAPPINGRequest();
		selectEQP_IPPCMAPPINGRequest.Body = new SelectEQP_IPPCMAPPINGRequestBody();
		selectEQP_IPPCMAPPINGRequest.Body.pParameter = pParameter;
		SelectEQP_IPPCMAPPINGResponse selectEQP_IPPCMAPPINGResponse = ((ModBusSoap)this).SelectEQP_IPPCMAPPING(selectEQP_IPPCMAPPINGRequest);
		return selectEQP_IPPCMAPPINGResponse.Body.SelectEQP_IPPCMAPPINGResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SelectEQP_IPPCMAPPING_JOIN_MACHResponse ModBusSoap.SelectEQP_IPPCMAPPING_JOIN_MACH(SelectEQP_IPPCMAPPING_JOIN_MACHRequest request)
	{
		return base.Channel.SelectEQP_IPPCMAPPING_JOIN_MACH(request);
	}

	public string SelectEQP_IPPCMAPPING_JOIN_MACH(string pParameter)
	{
		SelectEQP_IPPCMAPPING_JOIN_MACHRequest selectEQP_IPPCMAPPING_JOIN_MACHRequest = new SelectEQP_IPPCMAPPING_JOIN_MACHRequest();
		selectEQP_IPPCMAPPING_JOIN_MACHRequest.Body = new SelectEQP_IPPCMAPPING_JOIN_MACHRequestBody();
		selectEQP_IPPCMAPPING_JOIN_MACHRequest.Body.pParameter = pParameter;
		SelectEQP_IPPCMAPPING_JOIN_MACHResponse selectEQP_IPPCMAPPING_JOIN_MACHResponse = ((ModBusSoap)this).SelectEQP_IPPCMAPPING_JOIN_MACH(selectEQP_IPPCMAPPING_JOIN_MACHRequest);
		return selectEQP_IPPCMAPPING_JOIN_MACHResponse.Body.SelectEQP_IPPCMAPPING_JOIN_MACHResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	InsertEQP_EQP_IPPCMAPPINGResponse ModBusSoap.InsertEQP_EQP_IPPCMAPPING(InsertEQP_EQP_IPPCMAPPINGRequest request)
	{
		return base.Channel.InsertEQP_EQP_IPPCMAPPING(request);
	}

	public string InsertEQP_EQP_IPPCMAPPING(string pParameter)
	{
		InsertEQP_EQP_IPPCMAPPINGRequest insertEQP_EQP_IPPCMAPPINGRequest = new InsertEQP_EQP_IPPCMAPPINGRequest();
		insertEQP_EQP_IPPCMAPPINGRequest.Body = new InsertEQP_EQP_IPPCMAPPINGRequestBody();
		insertEQP_EQP_IPPCMAPPINGRequest.Body.pParameter = pParameter;
		InsertEQP_EQP_IPPCMAPPINGResponse insertEQP_EQP_IPPCMAPPINGResponse = ((ModBusSoap)this).InsertEQP_EQP_IPPCMAPPING(insertEQP_EQP_IPPCMAPPINGRequest);
		return insertEQP_EQP_IPPCMAPPINGResponse.Body.InsertEQP_EQP_IPPCMAPPINGResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	UpdateEQP_EQP_IPPCMAPPINGResponse ModBusSoap.UpdateEQP_EQP_IPPCMAPPING(UpdateEQP_EQP_IPPCMAPPINGRequest request)
	{
		return base.Channel.UpdateEQP_EQP_IPPCMAPPING(request);
	}

	public string UpdateEQP_EQP_IPPCMAPPING(string pParameter)
	{
		UpdateEQP_EQP_IPPCMAPPINGRequest updateEQP_EQP_IPPCMAPPINGRequest = new UpdateEQP_EQP_IPPCMAPPINGRequest();
		updateEQP_EQP_IPPCMAPPINGRequest.Body = new UpdateEQP_EQP_IPPCMAPPINGRequestBody();
		updateEQP_EQP_IPPCMAPPINGRequest.Body.pParameter = pParameter;
		UpdateEQP_EQP_IPPCMAPPINGResponse updateEQP_EQP_IPPCMAPPINGResponse = ((ModBusSoap)this).UpdateEQP_EQP_IPPCMAPPING(updateEQP_EQP_IPPCMAPPINGRequest);
		return updateEQP_EQP_IPPCMAPPINGResponse.Body.UpdateEQP_EQP_IPPCMAPPINGResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	DeleteEQP_EQP_IPPCMAPPINGResponse ModBusSoap.DeleteEQP_EQP_IPPCMAPPING(DeleteEQP_EQP_IPPCMAPPINGRequest request)
	{
		return base.Channel.DeleteEQP_EQP_IPPCMAPPING(request);
	}

	public string DeleteEQP_EQP_IPPCMAPPING(string pParameter)
	{
		DeleteEQP_EQP_IPPCMAPPINGRequest deleteEQP_EQP_IPPCMAPPINGRequest = new DeleteEQP_EQP_IPPCMAPPINGRequest();
		deleteEQP_EQP_IPPCMAPPINGRequest.Body = new DeleteEQP_EQP_IPPCMAPPINGRequestBody();
		deleteEQP_EQP_IPPCMAPPINGRequest.Body.pParameter = pParameter;
		DeleteEQP_EQP_IPPCMAPPINGResponse deleteEQP_EQP_IPPCMAPPINGResponse = ((ModBusSoap)this).DeleteEQP_EQP_IPPCMAPPING(deleteEQP_EQP_IPPCMAPPINGRequest);
		return deleteEQP_EQP_IPPCMAPPINGResponse.Body.DeleteEQP_EQP_IPPCMAPPINGResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SelectEQP_EQP_IPPCMAPPINGVarResponse ModBusSoap.SelectEQP_EQP_IPPCMAPPINGVar(SelectEQP_EQP_IPPCMAPPINGVarRequest request)
	{
		return base.Channel.SelectEQP_EQP_IPPCMAPPINGVar(request);
	}

	public string SelectEQP_EQP_IPPCMAPPINGVar(string pParameter)
	{
		SelectEQP_EQP_IPPCMAPPINGVarRequest selectEQP_EQP_IPPCMAPPINGVarRequest = new SelectEQP_EQP_IPPCMAPPINGVarRequest();
		selectEQP_EQP_IPPCMAPPINGVarRequest.Body = new SelectEQP_EQP_IPPCMAPPINGVarRequestBody();
		selectEQP_EQP_IPPCMAPPINGVarRequest.Body.pParameter = pParameter;
		SelectEQP_EQP_IPPCMAPPINGVarResponse selectEQP_EQP_IPPCMAPPINGVarResponse = ((ModBusSoap)this).SelectEQP_EQP_IPPCMAPPINGVar(selectEQP_EQP_IPPCMAPPINGVarRequest);
		return selectEQP_EQP_IPPCMAPPINGVarResponse.Body.SelectEQP_EQP_IPPCMAPPINGVarResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SelectEQP_EQP_IPPCMAPPINGVar_SingleResponse ModBusSoap.SelectEQP_EQP_IPPCMAPPINGVar_Single(SelectEQP_EQP_IPPCMAPPINGVar_SingleRequest request)
	{
		return base.Channel.SelectEQP_EQP_IPPCMAPPINGVar_Single(request);
	}

	public string SelectEQP_EQP_IPPCMAPPINGVar_Single(string pLineSysID, string pSysID, string pUserID)
	{
		SelectEQP_EQP_IPPCMAPPINGVar_SingleRequest selectEQP_EQP_IPPCMAPPINGVar_SingleRequest = new SelectEQP_EQP_IPPCMAPPINGVar_SingleRequest();
		selectEQP_EQP_IPPCMAPPINGVar_SingleRequest.Body = new SelectEQP_EQP_IPPCMAPPINGVar_SingleRequestBody();
		selectEQP_EQP_IPPCMAPPINGVar_SingleRequest.Body.pLineSysID = pLineSysID;
		selectEQP_EQP_IPPCMAPPINGVar_SingleRequest.Body.pSysID = pSysID;
		selectEQP_EQP_IPPCMAPPINGVar_SingleRequest.Body.pUserID = pUserID;
		SelectEQP_EQP_IPPCMAPPINGVar_SingleResponse selectEQP_EQP_IPPCMAPPINGVar_SingleResponse = ((ModBusSoap)this).SelectEQP_EQP_IPPCMAPPINGVar_Single(selectEQP_EQP_IPPCMAPPINGVar_SingleRequest);
		return selectEQP_EQP_IPPCMAPPINGVar_SingleResponse.Body.SelectEQP_EQP_IPPCMAPPINGVar_SingleResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	InsertEQP_EQP_IPPCMAPPINGVarResponse ModBusSoap.InsertEQP_EQP_IPPCMAPPINGVar(InsertEQP_EQP_IPPCMAPPINGVarRequest request)
	{
		return base.Channel.InsertEQP_EQP_IPPCMAPPINGVar(request);
	}

	public string InsertEQP_EQP_IPPCMAPPINGVar(string pParameter)
	{
		InsertEQP_EQP_IPPCMAPPINGVarRequest insertEQP_EQP_IPPCMAPPINGVarRequest = new InsertEQP_EQP_IPPCMAPPINGVarRequest();
		insertEQP_EQP_IPPCMAPPINGVarRequest.Body = new InsertEQP_EQP_IPPCMAPPINGVarRequestBody();
		insertEQP_EQP_IPPCMAPPINGVarRequest.Body.pParameter = pParameter;
		InsertEQP_EQP_IPPCMAPPINGVarResponse insertEQP_EQP_IPPCMAPPINGVarResponse = ((ModBusSoap)this).InsertEQP_EQP_IPPCMAPPINGVar(insertEQP_EQP_IPPCMAPPINGVarRequest);
		return insertEQP_EQP_IPPCMAPPINGVarResponse.Body.InsertEQP_EQP_IPPCMAPPINGVarResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	UpdateEQP_EQP_IPPCMAPPINGVarResponse ModBusSoap.UpdateEQP_EQP_IPPCMAPPINGVar(UpdateEQP_EQP_IPPCMAPPINGVarRequest request)
	{
		return base.Channel.UpdateEQP_EQP_IPPCMAPPINGVar(request);
	}

	public string UpdateEQP_EQP_IPPCMAPPINGVar(string pParameter)
	{
		UpdateEQP_EQP_IPPCMAPPINGVarRequest updateEQP_EQP_IPPCMAPPINGVarRequest = new UpdateEQP_EQP_IPPCMAPPINGVarRequest();
		updateEQP_EQP_IPPCMAPPINGVarRequest.Body = new UpdateEQP_EQP_IPPCMAPPINGVarRequestBody();
		updateEQP_EQP_IPPCMAPPINGVarRequest.Body.pParameter = pParameter;
		UpdateEQP_EQP_IPPCMAPPINGVarResponse updateEQP_EQP_IPPCMAPPINGVarResponse = ((ModBusSoap)this).UpdateEQP_EQP_IPPCMAPPINGVar(updateEQP_EQP_IPPCMAPPINGVarRequest);
		return updateEQP_EQP_IPPCMAPPINGVarResponse.Body.UpdateEQP_EQP_IPPCMAPPINGVarResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	DeleteEQP_EQP_IPPCMAPPINGVarResponse ModBusSoap.DeleteEQP_EQP_IPPCMAPPINGVar(DeleteEQP_EQP_IPPCMAPPINGVarRequest request)
	{
		return base.Channel.DeleteEQP_EQP_IPPCMAPPINGVar(request);
	}

	public string DeleteEQP_EQP_IPPCMAPPINGVar(string pParameter)
	{
		DeleteEQP_EQP_IPPCMAPPINGVarRequest deleteEQP_EQP_IPPCMAPPINGVarRequest = new DeleteEQP_EQP_IPPCMAPPINGVarRequest();
		deleteEQP_EQP_IPPCMAPPINGVarRequest.Body = new DeleteEQP_EQP_IPPCMAPPINGVarRequestBody();
		deleteEQP_EQP_IPPCMAPPINGVarRequest.Body.pParameter = pParameter;
		DeleteEQP_EQP_IPPCMAPPINGVarResponse deleteEQP_EQP_IPPCMAPPINGVarResponse = ((ModBusSoap)this).DeleteEQP_EQP_IPPCMAPPINGVar(deleteEQP_EQP_IPPCMAPPINGVarRequest);
		return deleteEQP_EQP_IPPCMAPPINGVarResponse.Body.DeleteEQP_EQP_IPPCMAPPINGVarResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	ChangeEQP_EQP_IPPCMAPPINGVarIndexResponse ModBusSoap.ChangeEQP_EQP_IPPCMAPPINGVarIndex(ChangeEQP_EQP_IPPCMAPPINGVarIndexRequest request)
	{
		return base.Channel.ChangeEQP_EQP_IPPCMAPPINGVarIndex(request);
	}

	public string ChangeEQP_EQP_IPPCMAPPINGVarIndex(string pParameter)
	{
		ChangeEQP_EQP_IPPCMAPPINGVarIndexRequest changeEQP_EQP_IPPCMAPPINGVarIndexRequest = new ChangeEQP_EQP_IPPCMAPPINGVarIndexRequest();
		changeEQP_EQP_IPPCMAPPINGVarIndexRequest.Body = new ChangeEQP_EQP_IPPCMAPPINGVarIndexRequestBody();
		changeEQP_EQP_IPPCMAPPINGVarIndexRequest.Body.pParameter = pParameter;
		ChangeEQP_EQP_IPPCMAPPINGVarIndexResponse changeEQP_EQP_IPPCMAPPINGVarIndexResponse = ((ModBusSoap)this).ChangeEQP_EQP_IPPCMAPPINGVarIndex(changeEQP_EQP_IPPCMAPPINGVarIndexRequest);
		return changeEQP_EQP_IPPCMAPPINGVarIndexResponse.Body.ChangeEQP_EQP_IPPCMAPPINGVarIndexResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SelectEQP_EQP_IPPCMAPPINGPcCmdResponse ModBusSoap.SelectEQP_EQP_IPPCMAPPINGPcCmd(SelectEQP_EQP_IPPCMAPPINGPcCmdRequest request)
	{
		return base.Channel.SelectEQP_EQP_IPPCMAPPINGPcCmd(request);
	}

	public string SelectEQP_EQP_IPPCMAPPINGPcCmd(string pParameter)
	{
		SelectEQP_EQP_IPPCMAPPINGPcCmdRequest selectEQP_EQP_IPPCMAPPINGPcCmdRequest = new SelectEQP_EQP_IPPCMAPPINGPcCmdRequest();
		selectEQP_EQP_IPPCMAPPINGPcCmdRequest.Body = new SelectEQP_EQP_IPPCMAPPINGPcCmdRequestBody();
		selectEQP_EQP_IPPCMAPPINGPcCmdRequest.Body.pParameter = pParameter;
		SelectEQP_EQP_IPPCMAPPINGPcCmdResponse selectEQP_EQP_IPPCMAPPINGPcCmdResponse = ((ModBusSoap)this).SelectEQP_EQP_IPPCMAPPINGPcCmd(selectEQP_EQP_IPPCMAPPINGPcCmdRequest);
		return selectEQP_EQP_IPPCMAPPINGPcCmdResponse.Body.SelectEQP_EQP_IPPCMAPPINGPcCmdResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	InsertEQP_EQP_IPPCMAPPINGPcCmdResponse ModBusSoap.InsertEQP_EQP_IPPCMAPPINGPcCmd(InsertEQP_EQP_IPPCMAPPINGPcCmdRequest request)
	{
		return base.Channel.InsertEQP_EQP_IPPCMAPPINGPcCmd(request);
	}

	public string InsertEQP_EQP_IPPCMAPPINGPcCmd(string pParameter)
	{
		InsertEQP_EQP_IPPCMAPPINGPcCmdRequest insertEQP_EQP_IPPCMAPPINGPcCmdRequest = new InsertEQP_EQP_IPPCMAPPINGPcCmdRequest();
		insertEQP_EQP_IPPCMAPPINGPcCmdRequest.Body = new InsertEQP_EQP_IPPCMAPPINGPcCmdRequestBody();
		insertEQP_EQP_IPPCMAPPINGPcCmdRequest.Body.pParameter = pParameter;
		InsertEQP_EQP_IPPCMAPPINGPcCmdResponse insertEQP_EQP_IPPCMAPPINGPcCmdResponse = ((ModBusSoap)this).InsertEQP_EQP_IPPCMAPPINGPcCmd(insertEQP_EQP_IPPCMAPPINGPcCmdRequest);
		return insertEQP_EQP_IPPCMAPPINGPcCmdResponse.Body.InsertEQP_EQP_IPPCMAPPINGPcCmdResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	UpdateEQP_EQP_IPPCMAPPINGPcCmdResponse ModBusSoap.UpdateEQP_EQP_IPPCMAPPINGPcCmd(UpdateEQP_EQP_IPPCMAPPINGPcCmdRequest request)
	{
		return base.Channel.UpdateEQP_EQP_IPPCMAPPINGPcCmd(request);
	}

	public string UpdateEQP_EQP_IPPCMAPPINGPcCmd(string pParameter)
	{
		UpdateEQP_EQP_IPPCMAPPINGPcCmdRequest updateEQP_EQP_IPPCMAPPINGPcCmdRequest = new UpdateEQP_EQP_IPPCMAPPINGPcCmdRequest();
		updateEQP_EQP_IPPCMAPPINGPcCmdRequest.Body = new UpdateEQP_EQP_IPPCMAPPINGPcCmdRequestBody();
		updateEQP_EQP_IPPCMAPPINGPcCmdRequest.Body.pParameter = pParameter;
		UpdateEQP_EQP_IPPCMAPPINGPcCmdResponse updateEQP_EQP_IPPCMAPPINGPcCmdResponse = ((ModBusSoap)this).UpdateEQP_EQP_IPPCMAPPINGPcCmd(updateEQP_EQP_IPPCMAPPINGPcCmdRequest);
		return updateEQP_EQP_IPPCMAPPINGPcCmdResponse.Body.UpdateEQP_EQP_IPPCMAPPINGPcCmdResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SelectEQP_EQP_IPPCMAPPINGAPIListResponse ModBusSoap.SelectEQP_EQP_IPPCMAPPINGAPIList(SelectEQP_EQP_IPPCMAPPINGAPIListRequest request)
	{
		return base.Channel.SelectEQP_EQP_IPPCMAPPINGAPIList(request);
	}

	public string SelectEQP_EQP_IPPCMAPPINGAPIList()
	{
		SelectEQP_EQP_IPPCMAPPINGAPIListRequest selectEQP_EQP_IPPCMAPPINGAPIListRequest = new SelectEQP_EQP_IPPCMAPPINGAPIListRequest();
		selectEQP_EQP_IPPCMAPPINGAPIListRequest.Body = new SelectEQP_EQP_IPPCMAPPINGAPIListRequestBody();
		SelectEQP_EQP_IPPCMAPPINGAPIListResponse selectEQP_EQP_IPPCMAPPINGAPIListResponse = ((ModBusSoap)this).SelectEQP_EQP_IPPCMAPPINGAPIList(selectEQP_EQP_IPPCMAPPINGAPIListRequest);
		return selectEQP_EQP_IPPCMAPPINGAPIListResponse.Body.SelectEQP_EQP_IPPCMAPPINGAPIListResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SelectEQP_EQP_IPPCMAPPINGAPIList_PResponse ModBusSoap.SelectEQP_EQP_IPPCMAPPINGAPIList_P(SelectEQP_EQP_IPPCMAPPINGAPIList_PRequest request)
	{
		return base.Channel.SelectEQP_EQP_IPPCMAPPINGAPIList_P(request);
	}

	public string SelectEQP_EQP_IPPCMAPPINGAPIList_P(string pParameter)
	{
		SelectEQP_EQP_IPPCMAPPINGAPIList_PRequest selectEQP_EQP_IPPCMAPPINGAPIList_PRequest = new SelectEQP_EQP_IPPCMAPPINGAPIList_PRequest();
		selectEQP_EQP_IPPCMAPPINGAPIList_PRequest.Body = new SelectEQP_EQP_IPPCMAPPINGAPIList_PRequestBody();
		selectEQP_EQP_IPPCMAPPINGAPIList_PRequest.Body.pParameter = pParameter;
		SelectEQP_EQP_IPPCMAPPINGAPIList_PResponse selectEQP_EQP_IPPCMAPPINGAPIList_PResponse = ((ModBusSoap)this).SelectEQP_EQP_IPPCMAPPINGAPIList_P(selectEQP_EQP_IPPCMAPPINGAPIList_PRequest);
		return selectEQP_EQP_IPPCMAPPINGAPIList_PResponse.Body.SelectEQP_EQP_IPPCMAPPINGAPIList_PResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	InsertEQP_EQP_IPPCMAPPINGAPIListResponse ModBusSoap.InsertEQP_EQP_IPPCMAPPINGAPIList(InsertEQP_EQP_IPPCMAPPINGAPIListRequest request)
	{
		return base.Channel.InsertEQP_EQP_IPPCMAPPINGAPIList(request);
	}

	public string InsertEQP_EQP_IPPCMAPPINGAPIList(string pParameter)
	{
		InsertEQP_EQP_IPPCMAPPINGAPIListRequest insertEQP_EQP_IPPCMAPPINGAPIListRequest = new InsertEQP_EQP_IPPCMAPPINGAPIListRequest();
		insertEQP_EQP_IPPCMAPPINGAPIListRequest.Body = new InsertEQP_EQP_IPPCMAPPINGAPIListRequestBody();
		insertEQP_EQP_IPPCMAPPINGAPIListRequest.Body.pParameter = pParameter;
		InsertEQP_EQP_IPPCMAPPINGAPIListResponse insertEQP_EQP_IPPCMAPPINGAPIListResponse = ((ModBusSoap)this).InsertEQP_EQP_IPPCMAPPINGAPIList(insertEQP_EQP_IPPCMAPPINGAPIListRequest);
		return insertEQP_EQP_IPPCMAPPINGAPIListResponse.Body.InsertEQP_EQP_IPPCMAPPINGAPIListResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	UpdateEQP_EQP_IPPCMAPPINGAPIListResponse ModBusSoap.UpdateEQP_EQP_IPPCMAPPINGAPIList(UpdateEQP_EQP_IPPCMAPPINGAPIListRequest request)
	{
		return base.Channel.UpdateEQP_EQP_IPPCMAPPINGAPIList(request);
	}

	public string UpdateEQP_EQP_IPPCMAPPINGAPIList(string pParameter)
	{
		UpdateEQP_EQP_IPPCMAPPINGAPIListRequest updateEQP_EQP_IPPCMAPPINGAPIListRequest = new UpdateEQP_EQP_IPPCMAPPINGAPIListRequest();
		updateEQP_EQP_IPPCMAPPINGAPIListRequest.Body = new UpdateEQP_EQP_IPPCMAPPINGAPIListRequestBody();
		updateEQP_EQP_IPPCMAPPINGAPIListRequest.Body.pParameter = pParameter;
		UpdateEQP_EQP_IPPCMAPPINGAPIListResponse updateEQP_EQP_IPPCMAPPINGAPIListResponse = ((ModBusSoap)this).UpdateEQP_EQP_IPPCMAPPINGAPIList(updateEQP_EQP_IPPCMAPPINGAPIListRequest);
		return updateEQP_EQP_IPPCMAPPINGAPIListResponse.Body.UpdateEQP_EQP_IPPCMAPPINGAPIListResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	DeleteEQP_EQP_IPPCMAPPINGAPIListResponse ModBusSoap.DeleteEQP_EQP_IPPCMAPPINGAPIList(DeleteEQP_EQP_IPPCMAPPINGAPIListRequest request)
	{
		return base.Channel.DeleteEQP_EQP_IPPCMAPPINGAPIList(request);
	}

	public string DeleteEQP_EQP_IPPCMAPPINGAPIList(string pParameter)
	{
		DeleteEQP_EQP_IPPCMAPPINGAPIListRequest deleteEQP_EQP_IPPCMAPPINGAPIListRequest = new DeleteEQP_EQP_IPPCMAPPINGAPIListRequest();
		deleteEQP_EQP_IPPCMAPPINGAPIListRequest.Body = new DeleteEQP_EQP_IPPCMAPPINGAPIListRequestBody();
		deleteEQP_EQP_IPPCMAPPINGAPIListRequest.Body.pParameter = pParameter;
		DeleteEQP_EQP_IPPCMAPPINGAPIListResponse deleteEQP_EQP_IPPCMAPPINGAPIListResponse = ((ModBusSoap)this).DeleteEQP_EQP_IPPCMAPPINGAPIList(deleteEQP_EQP_IPPCMAPPINGAPIListRequest);
		return deleteEQP_EQP_IPPCMAPPINGAPIListResponse.Body.DeleteEQP_EQP_IPPCMAPPINGAPIListResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SelectEQP_IPPCMAPPINGHOSTUserResponse ModBusSoap.SelectEQP_IPPCMAPPINGHOSTUser(SelectEQP_IPPCMAPPINGHOSTUserRequest request)
	{
		return base.Channel.SelectEQP_IPPCMAPPINGHOSTUser(request);
	}

	public string SelectEQP_IPPCMAPPINGHOSTUser(string pParameter)
	{
		SelectEQP_IPPCMAPPINGHOSTUserRequest selectEQP_IPPCMAPPINGHOSTUserRequest = new SelectEQP_IPPCMAPPINGHOSTUserRequest();
		selectEQP_IPPCMAPPINGHOSTUserRequest.Body = new SelectEQP_IPPCMAPPINGHOSTUserRequestBody();
		selectEQP_IPPCMAPPINGHOSTUserRequest.Body.pParameter = pParameter;
		SelectEQP_IPPCMAPPINGHOSTUserResponse selectEQP_IPPCMAPPINGHOSTUserResponse = ((ModBusSoap)this).SelectEQP_IPPCMAPPINGHOSTUser(selectEQP_IPPCMAPPINGHOSTUserRequest);
		return selectEQP_IPPCMAPPINGHOSTUserResponse.Body.SelectEQP_IPPCMAPPINGHOSTUserResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	InsertEQP_IPPCMAPPINGHOSTUserResponse ModBusSoap.InsertEQP_IPPCMAPPINGHOSTUser(InsertEQP_IPPCMAPPINGHOSTUserRequest request)
	{
		return base.Channel.InsertEQP_IPPCMAPPINGHOSTUser(request);
	}

	public string InsertEQP_IPPCMAPPINGHOSTUser(string pParameter)
	{
		InsertEQP_IPPCMAPPINGHOSTUserRequest insertEQP_IPPCMAPPINGHOSTUserRequest = new InsertEQP_IPPCMAPPINGHOSTUserRequest();
		insertEQP_IPPCMAPPINGHOSTUserRequest.Body = new InsertEQP_IPPCMAPPINGHOSTUserRequestBody();
		insertEQP_IPPCMAPPINGHOSTUserRequest.Body.pParameter = pParameter;
		InsertEQP_IPPCMAPPINGHOSTUserResponse insertEQP_IPPCMAPPINGHOSTUserResponse = ((ModBusSoap)this).InsertEQP_IPPCMAPPINGHOSTUser(insertEQP_IPPCMAPPINGHOSTUserRequest);
		return insertEQP_IPPCMAPPINGHOSTUserResponse.Body.InsertEQP_IPPCMAPPINGHOSTUserResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	DeleteEQP_IPPCMAPPINGHOSTUserResponse ModBusSoap.DeleteEQP_IPPCMAPPINGHOSTUser(DeleteEQP_IPPCMAPPINGHOSTUserRequest request)
	{
		return base.Channel.DeleteEQP_IPPCMAPPINGHOSTUser(request);
	}

	public string DeleteEQP_IPPCMAPPINGHOSTUser(string pParameter)
	{
		DeleteEQP_IPPCMAPPINGHOSTUserRequest deleteEQP_IPPCMAPPINGHOSTUserRequest = new DeleteEQP_IPPCMAPPINGHOSTUserRequest();
		deleteEQP_IPPCMAPPINGHOSTUserRequest.Body = new DeleteEQP_IPPCMAPPINGHOSTUserRequestBody();
		deleteEQP_IPPCMAPPINGHOSTUserRequest.Body.pParameter = pParameter;
		DeleteEQP_IPPCMAPPINGHOSTUserResponse deleteEQP_IPPCMAPPINGHOSTUserResponse = ((ModBusSoap)this).DeleteEQP_IPPCMAPPINGHOSTUser(deleteEQP_IPPCMAPPINGHOSTUserRequest);
		return deleteEQP_IPPCMAPPINGHOSTUserResponse.Body.DeleteEQP_IPPCMAPPINGHOSTUserResult;
	}
}
