using System;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.corp.delta.twcynmes01;
using Protocol.MES.Enums;
using Protocol.MES.Messages;
using Protocol.MES.Utils;

namespace Protocol.MES.Factory;

public static class MESMessageFactory
{
	public static string TargetUrl { get; set; }

	public static MESBaseMessage Create(string json)
	{
		dynamic tmp = JsonConvert.DeserializeObject(json);
		string txn = tmp.TransactionName;
		if (!Enum.TryParse<MESTransactionName>(txn.ToString(), out var trans))
		{
			throw new Exception("Unknown MES Transaction: " + txn);
		}
		return trans switch
		{
			MESTransactionName.WOQRY => MESJsonHelper.Deserialize<WOQRYReply>(json), 
			MESTransactionName.WOCHECKIN => MESJsonHelper.Deserialize<WOCHECKINReply>(json), 
			MESTransactionName.WOCHECKOUT => MESJsonHelper.Deserialize<WOCHECKOUTReply>(json), 
			MESTransactionName.EQPSTATUS => MESJsonHelper.Deserialize<EQPSTATUSReply>(json), 
			MESTransactionName.UserVerify => MESJsonHelper.Deserialize<UserVerifyReply>(json), 
			_ => throw new NotImplementedException("Transaction " + txn + " not implemented"), 
		};
	}

	public static TReply CallMes<TRequest, TReply>(TRequest request) where TRequest : MESBaseMessage where TReply : MESBaseMessage
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		string jsonReq = MESJsonHelper.Serialize(request);
		string jsonReply;
		try
		{
			using Eqp_Portal client = new Eqp_Portal();
            if (!string.IsNullOrEmpty(TargetUrl))
            {
                client.Url = TargetUrl;
            }
            // 設定 15 秒超時 (單位為毫秒)
            client.Timeout = 15000; 
			jsonReply = client.EqpTransaction(jsonReq);
		}
		catch (System.Net.WebException ex)
		{
            if (ex.Status == System.Net.WebExceptionStatus.Timeout)
                throw new Exception("MES 呼叫超時 (15s)");
			throw new Exception("MES 網路連線異常: " + ex.Message, ex);
		}
		catch (Exception ex)
		{
			throw new Exception("Call MES WebService failed: " + ex.Message, ex);
		}
		return MESJsonHelper.Deserialize<TReply>(jsonReply);
	}
}
