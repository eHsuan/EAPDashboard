using System.Collections.Generic;

namespace Protocol.MES.Messages;

public class StdCheckItem
{
	public string PlantNo;

	public string MatGroupNo;

	public string LineCode;

	public string MatNo;

	public string WorkCenterNo;

	public string StdPartItem;

	public string StdPartParam;

	public string StdPartParamSeq;

	public bool IsKeepPartNo;

	public string Frequence;

	public string FrequenceUnit;

	public int MatID;

	public string ParamValue;

	public string LowerValue;

	public string UpperValue;

	public string StdPartID;

	public string StdPartType;

	public string StdPartsAlias;

	public string SysID;

	public string Unit;

	public int ValidHours;

	public string ToolingID;

	public List<string> DataList;

	public bool IsComplete;

	public string WONo;
}
