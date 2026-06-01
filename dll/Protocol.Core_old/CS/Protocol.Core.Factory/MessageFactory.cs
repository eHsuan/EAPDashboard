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
		JObject val = JObject.Parse(json);
		CommandType cmd = val["CMD"].ToObject<CommandType>();
		Type type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(delegate(Type t)
		{
			CommandMappingAttribute customAttribute = t.GetCustomAttribute<CommandMappingAttribute>();
			return customAttribute != null && customAttribute.Command == cmd;
		});
		if (type == null)
		{
			throw new NotSupportedException($"CMD not supported: {cmd}");
		}
		return (BaseMessage)JsonConvert.DeserializeObject(json, type);
	}
}
