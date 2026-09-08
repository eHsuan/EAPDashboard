using System;

namespace cyntec.WCF;

[Serializable]
public class TransactionResultBase : MessageBase<TransactionResultBase>, IwcfTransactionResult
{
	public const string PASS = "PASS";

	private bool m_isError = false;

	private string m_ReplyMsg = "";

	public bool isError
	{
		get
		{
			return m_isError;
		}
		set
		{
			m_isError = value;
		}
	}

	public string replyMsg
	{
		get
		{
			return m_ReplyMsg;
		}
		set
		{
			m_ReplyMsg = value;
		}
	}

	public int ErrorCode { get; set; }
}
