using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CyntecMqttLib.Util;

public static class JsonUtil
{
	private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
	{
		Converters = { new StringEnumConverter() }
	};

	public static string Serialize(object obj)
	{
		return JsonConvert.SerializeObject(obj, _settings);
	}

	public static T Deserialize<T>(string json)
	{
		return JsonConvert.DeserializeObject<T>(json, _settings);
	}

	public static T Convert<T>(object obj)
	{
		return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj, _settings), _settings);
	}
}
