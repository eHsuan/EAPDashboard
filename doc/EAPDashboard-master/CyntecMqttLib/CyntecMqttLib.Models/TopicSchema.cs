namespace CyntecMqttLib.Models;

/// <summary>
/// MQTT Topic 命名標準
/// </summary>
public static class TopicSchema
{
	public static string UpStatus(string fab, string line, string machine)
	{
		return fab + "/" + line + "/" + machine + "/up/status";
	}

	public static string UpData(string fab, string line, string machine)
	{
		return fab + "/" + line + "/" + machine + "/up/data";
	}

	public static string DownCommand(string fab, string line, string machine)
	{
		return fab + "/" + line + "/" + machine + "/down/cmd";
	}
}
