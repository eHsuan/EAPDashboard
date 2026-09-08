using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace cyntec.Tools;

public class cyntecJsonDictionary : Dictionary<string, string>
{
	public string ToJsonString()
	{
		return new JavaScriptSerializer().Serialize(this);
	}

	public override string ToString()
	{
		return new JavaScriptSerializer().Serialize(this);
	}
}
