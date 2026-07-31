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
			jsonReply = client.EqpTransaction(jsonReq);
		}
		catch (Exception ex)
		{
			throw new Exception("Call MES WebService failed: " + ex.Message, ex);
		}
		return MESJsonHelper.Deserialize<TReply>(jsonReply);
	}
}
