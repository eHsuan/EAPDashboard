using Newtonsoft.Json;

namespace CyntecMqttLib.Util;

internal static class JsonUtil
{
	public static string Serialize(object obj)
	{
		return JsonConvert.SerializeObject(obj);
	}

	public static T Deserialize<T>(string json)
	{
		return JsonConvert.DeserializeObject<T>(json);
	}

	public static T Convert<T>(object obj)
	{
		return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
	}
}
