using System;

namespace CyntecMqttLib.Internal;

internal static class MqttTopicMatcher
{
	public static bool IsMatch(string topic, string filter)
	{
		if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(filter))
		{
			return false;
		}
		string[] topicLevels = topic.Split('/');
		string[] filterLevels = filter.Split('/');
		for (int i = 0; i < filterLevels.Length; i++)
		{
			if (i >= topicLevels.Length)
			{
				return false;
			}
			string f = filterLevels[i];
			string t = topicLevels[i];
			if (f == "#")
			{
				return true;
			}
			if (!(f == "+") && !string.Equals(f, t, StringComparison.Ordinal))
			{
				return false;
			}
		}
		return topicLevels.Length == filterLevels.Length;
	}
}
