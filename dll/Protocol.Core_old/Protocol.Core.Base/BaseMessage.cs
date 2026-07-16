using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Protocol.Core.Enums;

namespace Protocol.Core.Base;

public abstract class BaseMessage
{
	[JsonProperty("CMD")]
	[JsonConverter(typeof(StringEnumConverter))]
	public CommandType CMD { get; set; }

	[JsonProperty("DATE")]
	public string Date { get; set; }

	public string Machine { get; set; }

	public string TransactionID { get; set; } = Guid.NewGuid().ToString();

	public T CreateReply<T>() where T : ReplyBase, new()
	{
		T val = new T
		{
			Machine = Machine,
			Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
			TransactionID = TransactionID
		};
		CommandMappingAttribute customAttribute = typeof(T).GetCustomAttribute<CommandMappingAttribute>();
		if (customAttribute != null)
		{
			val.CMD = customAttribute.Command;
		}
		return val;
	}
}
