using System.Text;

namespace cyntec.Tools;

public class cyntecFTP
{
	public CyntecSocketBase oSocketControl = null;

	public byte[] ControlIP = new byte[4] { 127, 0, 0, 1 };

	public void Connect()
	{
		if (oSocketControl != null)
		{
			oSocketControl.Close();
		}
		oSocketControl.IP = new byte[4] { 10, 131, 11, 217 };
		oSocketControl.port = 21;
		oSocketControl.SendTimeout = 5000;
		oSocketControl.ReceiveTimeout = 5000;
		oSocketControl.onError += OSocketControl_onError;
		oSocketControl.onMessageArrive += OSocketControl_onMessageArrive;
		oSocketControl.Init();
	}

	private void OSocketControl_onMessageArrive(object Sender, byte[] msg)
	{
		string text = Encoding.UTF8.GetString(msg);
	}

	private void OSocketControl_onError(object Sender, string msg)
	{
	}
}
