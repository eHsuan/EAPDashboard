using Newtonsoft.Json;
using Protocol.MES.Base;

namespace Protocol.MES.Utils;

public static class MESJsonHelper
{
	public static string Serialize(MESBaseMessage msg)
	{
		return JsonConvert.SerializeObject((object)msg, (Formatting)1);
	}

	public static T Deserialize<T>(string json) where T : MESBaseMessage
	{
		return JsonConvert.DeserializeObject<T>(json);
	}
}
