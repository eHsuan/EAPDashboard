using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace cyntec.Tools;

public class CyntecTxnRequest
{
	public string TXN_NAME = "";

	public string TXN_ID = "";

	public DateTime TXN_REQUEST_TIME;

	public Dictionary<string, object> TXN_PARAM_LIST = new Dictionary<string, object>();

	public static CyntecTxnRequest ToObject(string strJson)
	{
		return JsonConvert.DeserializeObject<CyntecTxnRequest>(strJson);
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
