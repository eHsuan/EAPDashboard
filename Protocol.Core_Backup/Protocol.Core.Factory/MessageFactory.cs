using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Factory;

public static class MessageFactory
{
	public static BaseMessage Create(string json)
	{
		JObject jo = JObject.Parse(json);
		string targetJson = json;
		JObject targetJo = jo;

		// 判斷是否為封裝在 Payload 內的訊息格式
		if (jo["Payload"] != null && jo["Payload"].Type == JTokenType.String)
		{
			targetJson = jo["Payload"].ToString();
			targetJo = JObject.Parse(targetJson);
		}

		// 檢查是否有 CMD 屬性
		if (targetJo["CMD"] == null)
		{
			throw new NullReferenceException("JSON message does not contain 'CMD' property.");
		}

		CommandType cmd = targetJo["CMD"].ToObject<CommandType>();
		Type type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(delegate(Type t)
		{
			CommandMappingAttribute customAttribute = t.GetCustomAttribute<CommandMappingAttribute>();
			return customAttribute != null && customAttribute.Command == cmd;
		});
		if (type == null)
		{
			throw new NotSupportedException($"CMD not supported: {cmd}");
		}
		return (BaseMessage)JsonConvert.DeserializeObject(targetJson, type);
	}
}
