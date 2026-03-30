using Newtonsoft.Json;

namespace Protocol.Core.Utils;

public static class JsonHelper
{
	public static string Serialize(object obj)
	{
		return JsonConvert.SerializeObject(obj, (Formatting)0);
	}

	public static T Deserialize<T>(string json)
	{
		return JsonConvert.DeserializeObject<T>(json);
	}
}
