using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace cyntec.Tools;

public class CyntecTxnReply
{
	public string TXN_NAME = "";

	public string TXN_ID = "";

	public DateTime TXN_REQUEST_TIME;

	public DateTime TXN_REPLY_TIME;

	public decimal TXN_COST = default(decimal);

	public bool IS_TXN_ERROR = false;

	public string TXN_ERROR_MSG = "";

	public Dictionary<string, object> TXN_PARAM_LIST = new Dictionary<string, object>();

	public void init(CyntecTxnRequest oRequest)
	{
		TXN_NAME = oRequest.TXN_NAME;
		TXN_ID = oRequest.TXN_ID;
		TXN_REQUEST_TIME = oRequest.TXN_REQUEST_TIME;
	}

	public static CyntecTxnReply ToObject(string strJson)
	{
		return JsonConvert.DeserializeObject<CyntecTxnReply>(strJson);
	}

	public virtual string ToJsonString()
	{
		return JsonConvert.SerializeObject((object)this);
	}

	public override string ToString()
	{
		return ToJsonString();
	}
}
