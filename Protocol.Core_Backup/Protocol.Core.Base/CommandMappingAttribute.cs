using System;
using Protocol.Core.Enums;

namespace Protocol.Core.Base;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CommandMappingAttribute : Attribute
{
	public CommandType Command { get; }

	public CommandMappingAttribute(CommandType command)
	{
		Command = command;
	}
}
