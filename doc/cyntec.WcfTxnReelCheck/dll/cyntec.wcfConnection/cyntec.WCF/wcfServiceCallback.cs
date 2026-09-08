using cyntec.wcfConnection.wcfServiceRef;

namespace cyntec.WCF;

public class wcfServiceCallback : cyntec.wcfConnection.wcfServiceRef.IwcfServiceCallback
{
	public event ConnBroadCastDelegate onBroadcast;

	public void SystemBroadcast(string msg)
	{
	}

	public void SystemCommandReply(string msg)
	{
	}

	public void ObjectBroadCast(byte[] rawData)
	{
		if (this.onBroadcast != null)
		{
			this.onBroadcast(null, rawData);
		}
	}
}
