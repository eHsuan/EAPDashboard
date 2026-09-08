using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using cyntec.Base;

namespace cyntec.Windows;

public class cyntecMsgBox
{
	public static string strLanguage = "zh-TW";

	public static bool IsKeepActive = true;

	public static bool displayDontShowMessageAgain = true;

	internal static bool resultDontShowMessageAgain = false;

	internal static Dictionary<string, bool> dontShowMessage = new Dictionary<string, bool>();

	public cyntecCulture m_Culture = null;

	public static frmMsgBox MyForm = null;

	private static bool isDontShowMessageAgain(string messageId, out string messageKey)
	{
		try
		{
			StackTrace stackTrace = new StackTrace();
			int num = 1;
			MethodBase method;
			while (true)
			{
				num++;
				method = stackTrace.GetFrame(num).GetMethod();
				if (!(method.ReflectedType == typeof(cyntecMsgBox)))
				{
					break;
				}
				bool flag = true;
			}
			messageKey = method.ReflectedType.Name + "." + method.Name + "." + messageId;
			return dontShowMessage.ContainsKey(messageKey);
		}
		catch
		{
			messageKey = "";
			return false;
		}
	}

	public static void ShowMsgForm(string MsgID, string Message, messageStyle msgStyl)
	{
		if (MyForm == null)
		{
			MyForm = new frmMsgBox();
			MyForm.FormClosed += MyForm_FormClosed;
			MyForm.VisibleChanged += MyForm_VisibleChanged;
		}
		MyForm.Text = MsgID;
		MyForm.style = msgStyl;
		MyForm.btnDetail.Visible = false;
		MyForm.show(MsgID, Message);
		MyForm.Show();
	}

	private static void MyForm_VisibleChanged(object sender, EventArgs e)
	{
	}

	public static void ShowMsgForm(string MsgID, string Message, messageStyle msgStyl, IWin32Window iwin32)
	{
		if (MyForm == null)
		{
			MyForm = new frmMsgBox();
			MyForm.FormClosed += MyForm_FormClosed;
		}
		MyForm.Text = MsgID;
		MyForm.TopMost = true;
		MyForm.style = msgStyl;
		MyForm.btnDetail.Visible = false;
		MyForm.show(MsgID, Message);
		if (MyForm.Tag == null)
		{
			MyForm.Tag = "SHOW";
			MyForm.Show(iwin32);
		}
		else
		{
			MyForm.Show();
		}
	}

	public static void ShowMsgForm(string MsgID, string Message, messageStyle msgStyl, IWin32Window iwin32, bool IsVoice)
	{
		ShowMsgForm(MsgID, Message, msgStyl, iwin32);
		MyForm.chkIsVoice.Checked = IsVoice;
	}

	private static void MyForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		MyForm = null;
	}

	public bool showMsg(string msgID, string msg, messageStyle msgStyl, params string[] args)
	{
		string msgID2 = m_Culture.ToType(msgID);
		string msg2 = m_Culture.ToMSg(msgID, (object[])args);
		return ShowMsg(msgID2, msg2, msgStyl, (string[])null);
	}

	public static bool ShowMsg(string msgID, string msg, messageStyle msgStyl, params string[] args)
	{
		if (args != null)
		{
			msg = replaceItem(msg, args);
		}
		frmMsgBox frmMsgBox2 = new frmMsgBox();
		if (args != null && args.Length != 0)
		{
			frmMsgBox2.m_TitleTag = args[0];
		}
		frmMsgBox2.Text = msgID;
		frmMsgBox2.style = msgStyl;
		frmMsgBox2.btnDetail.Visible = false;
		frmMsgBox2.show(msgID, msg);
		frmMsgBox2.ShowDialog();
		try
		{
			return frmMsgBox2.Result;
		}
		finally
		{
			frmMsgBox2.Close();
		}
	}

	public void showErrorMsg(string msgID, string msg, params string[] args)
	{
		string msgID2 = m_Culture.ToType(msgID);
		string msg2 = m_Culture.ToMSg(msgID, (object[])args);
		ShowErrorMsg(msgID2, msg2, (string[])null);
	}

	public static void ShowErrorMsg(string msgID, string msg, params string[] args)
	{
		frmMsgBox frmMsgBox2 = new frmMsgBox();
		if (args != null && args.Length != 0)
		{
			frmMsgBox2.m_TitleTag = args[0];
		}
		frmMsgBox2.style = messageStyle.Error;
		frmMsgBox2.m_IsKeepActive = IsKeepActive;
		frmMsgBox2.Text = msgID;
		frmMsgBox2.show(msgID, msg);
		frmMsgBox2.TopMost = true;
		frmMsgBox2.ShowDialog();
		frmMsgBox2.Close();
	}

	public static bool ShowWarnMsg(string msgID, string msg, params string[] args)
	{
		frmMsgBox frmMsgBox2 = new frmMsgBox();
		if (args != null && args.Length != 0)
		{
			frmMsgBox2.m_TitleTag = args[0];
		}
		frmMsgBox2.style = messageStyle.askYesNo;
		frmMsgBox2.m_IsKeepActive = IsKeepActive;
		frmMsgBox2.Text = msgID;
		frmMsgBox2.show(msgID, msg);
		frmMsgBox2.TopMost = true;
		frmMsgBox2.ShowDialog();
		return frmMsgBox2.Result;
	}

	public void showErrorMsg(string msgID, string msg, bool isWarn, params string[] args)
	{
		string msgID2 = m_Culture.ToType(msgID);
		string msg2 = m_Culture.ToMSg(msgID, (object[])args);
		ShowErrorMsg(msgID2, msg2, isWarn, args);
	}

	public static void ShowErrorMsg(string msgID, string msg, bool isWarn, params string[] args)
	{
		if (isWarn)
		{
			frmWarnBox frmWarnBox2 = new frmWarnBox();
			frmWarnBox2.style = messageStyle.Waring;
			frmWarnBox2.m_IsKeepActive = IsKeepActive;
			frmWarnBox2.Text = msgID;
			frmWarnBox2.show(msgID, msg);
			frmWarnBox2.TopMost = true;
			frmWarnBox2.ShowDialog();
			frmWarnBox2.Close();
			return;
		}
		frmMsgBox frmMsgBox2 = new frmMsgBox();
		if (args != null && args.Length != 0)
		{
			frmMsgBox2.m_TitleTag = args[0];
		}
		frmMsgBox2.style = messageStyle.Error;
		frmMsgBox2.m_IsKeepActive = IsKeepActive;
		frmMsgBox2.Text = msgID;
		frmMsgBox2.show(msgID, msg);
		frmMsgBox2.TopMost = true;
		frmMsgBox2.ShowDialog();
		frmMsgBox2.Close();
	}

	public void showErrorMsg(string msgID, string msg, string msgDetail, bool isWarn, params string[] args)
	{
		string msgID2 = m_Culture.ToType(msgID);
		string msg2 = m_Culture.ToMSg(msgID, (object[])args);
		ShowErrorMsg(msgID2, msg2, msgDetail, isWarn, args);
	}

	public static void ShowErrorMsg(string msgID, string msg, string msgDetail, bool isWarn, params string[] args)
	{
		if (isWarn)
		{
			frmWarnBox frmWarnBox2 = new frmWarnBox();
			frmWarnBox2.style = messageStyle.Waring;
			frmWarnBox2.m_IsKeepActive = IsKeepActive;
			frmWarnBox2.Text = msgID;
			frmWarnBox2.show(msgID, msg, msgDetail);
			frmWarnBox2.TopMost = true;
			frmWarnBox2.ShowDialog();
			frmWarnBox2.Close();
			return;
		}
		frmMsgBox frmMsgBox2 = new frmMsgBox();
		if (args != null && args.Length != 0)
		{
			frmMsgBox2.m_TitleTag = args[0];
		}
		frmMsgBox2.style = messageStyle.Error;
		frmMsgBox2.m_IsKeepActive = IsKeepActive;
		frmMsgBox2.Text = msgID;
		frmMsgBox2.show(msgID, msg);
		frmMsgBox2.TopMost = true;
		frmMsgBox2.ShowDialog();
		frmMsgBox2.Close();
	}

	public static bool ShowWaringMsg(string msgID, string msg, params string[] args)
	{
		msg = replaceItem(msg, args);
		frmMsgBox frmMsgBox2 = new frmMsgBox();
		if (args != null && args.Length != 0)
		{
			frmMsgBox2.m_TitleTag = args[0];
		}
		frmMsgBox2.style = messageStyle.Waring;
		frmMsgBox2.Text = msgID;
		frmMsgBox2.show(msgID, msg);
		frmMsgBox2.btnDetail.Visible = false;
		frmMsgBox2.ShowDialog();
		try
		{
			return frmMsgBox2.Result;
		}
		finally
		{
			frmMsgBox2.Close();
		}
	}

	public void showMessage(string msgID, string message, params string[] args)
	{
		string text = m_Culture.ToType(msgID);
		string message2 = m_Culture.ToMSg(msgID, (object[])args);
		showMessage(message2);
	}

	public static void showMessage(string message)
	{
		string[] array = new string[1] { "" };
		frmMsgBox frmMsgBox2 = new frmMsgBox();
		frmMsgBox2.style = messageStyle.Prompt;
		string format = translateMessge(message, array);
		object[] args = array;
		frmMsgBox2.show(string.Format(format, args));
		frmMsgBox2.ShowDialog();
	}

	private static string replaceItem(string message, string[] itemList)
	{
		string text = message;
		for (int i = 0; i < itemList.Length; i++)
		{
			text = text.Replace("#" + (i + 1) + "#", itemList[i]);
		}
		return text;
	}

	private static string translateMessge(string message, string[] args)
	{
		if (args == null || args.Length == 0)
		{
			return message;
		}
		StringBuilder stringBuilder = new StringBuilder(message);
		int num = 0;
		while (true)
		{
			int num2 = stringBuilder.ToString().IndexOf('&');
			if (num2 == -1 || num >= args.Length)
			{
				break;
			}
			stringBuilder.Replace("&", args[num], num2, 1);
			num++;
			bool flag = true;
		}
		return stringBuilder.ToString();
	}
}
