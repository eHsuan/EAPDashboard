using System;

namespace cyntec.MES;

[Serializable]
public class WO_Packing
{
	public string MatGroupNo = "";

	public string WONo = "";

	public string MatNo = "";

	public string BatchNo = "";

	public string WorkCenterNo = "";

	public string PackType_CheckIn = "";

	public string PackType_CheckOut = "";

	public string LabelNumber_CheckIn = "";

	public string LabelNumber_CheckOut = "";

	public bool Enable;

	public string CreateUser = "";

	public DateTime CreateDate;

	public string UpdateUser = "";

	public DateTime UpdateDate;

	public int FlowID = 0;
}
