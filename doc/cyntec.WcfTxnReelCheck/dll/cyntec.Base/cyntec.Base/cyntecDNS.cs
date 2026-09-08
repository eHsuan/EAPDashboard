using System.Collections.Generic;
using System.Net;

namespace cyntec.Base;

public class cyntecDNS
{
	public static string[] GetIP()
	{
		string hostName = Dns.GetHostName();
		return GetIP(hostName);
	}

	public static string[] GetIP(string hostName)
	{
		IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
		List<string> list = new List<string>();
		IPAddress[] addressList = hostEntry.AddressList;
		foreach (IPAddress iPAddress in addressList)
		{
			list.Add(iPAddress.ToString());
		}
		return list.ToArray();
	}
}
