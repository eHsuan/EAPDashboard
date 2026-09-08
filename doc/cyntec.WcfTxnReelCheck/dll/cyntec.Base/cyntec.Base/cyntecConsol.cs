using System;

namespace cyntec.Base;

public class cyntecConsol
{
	public static bool IsShowOnConsol = true;

	public static bool IsLogFile = true;

	public static bool IsWait = false;

	public static void LogMsg(string msg)
	{
		if (IsShowOnConsol)
		{
			Console.WriteLine(msg);
		}
		IsWait = false;
	}

	public static void LogMsg(string msg, ConsoleColor color)
	{
		if (IsShowOnConsol)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(msg);
		}
		IsWait = false;
	}

	public static void LogMsgTime(string msg, ConsoleColor color, ConsoleColor TimeColor)
	{
		if (IsShowOnConsol)
		{
			Console.ForegroundColor = TimeColor;
			Console.Write(DateTime.Now.ToString("hh:mm:ss>"));
			Console.ForegroundColor = color;
			Console.WriteLine(msg);
		}
		IsWait = false;
	}

	public static void LogMsgWait(string msg, ConsoleColor color)
	{
		if (IsShowOnConsol)
		{
			Console.ForegroundColor = color;
			Console.Write(msg);
		}
		IsWait = true;
	}
}
