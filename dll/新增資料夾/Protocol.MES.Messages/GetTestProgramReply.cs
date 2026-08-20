using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetTestProgramReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string CancelDate { get; set; }

	public string CancelUser { get; set; }

	public string DeltaMatNo { get; set; }

	public string DraftDate { get; set; }

	public string DraftUser { get; set; }

	public string FileCheckCode { get; set; }

	public string FileData { get; set; }

	public string FileName { get; set; }

	public string FileVersion { get; set; }

	public string ID { get; set; }

	public string MatGroupNo { get; set; }

	public string ReleaseDate { get; set; }

	public string ReleaseUser { get; set; }

	public string Remark { get; set; }

	public string SeqNo { get; set; }

	public string StatusCode { get; set; }

	public string UpdateDate { get; set; }

	public string UpdateUser { get; set; }

	public string UploadDate { get; set; }

	public string UploadUser { get; set; }

	public string ValidDate { get; set; }

	public GetTestProgramReply()
	{
		base.TransactionName = MESTransactionName.GetTestProgram;
	}
}
