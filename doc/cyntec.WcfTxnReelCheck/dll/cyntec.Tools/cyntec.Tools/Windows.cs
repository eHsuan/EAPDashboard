using System;
using System.Runtime.InteropServices;

namespace cyntec.Tools;

public class Windows
{
	private const int MF_BYCOMMAND = 0;

	private const int SC_CLOSE = 61536;

	[DllImport("user32.dll")]
	protected static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

	[DllImport("user32.dll")]
	protected static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

	[DllImport("kernel32.dll", ExactSpelling = true)]
	protected static extern IntPtr GetConsoleWindow();

	[DllImport("kernel32.dll", ExactSpelling = true)]
	public static extern int Beep(int dwFreq, int dwDuration);

	public static void DisableColse()
	{
		DeleteMenu(GetSystemMenu(GetConsoleWindow(), bRevert: false), 61536, 0);
	}
}
