namespace cyntec.WCF;

public interface IwcfTransactionResult
{
	bool isError { get; set; }

	string replyMsg { get; set; }
}
