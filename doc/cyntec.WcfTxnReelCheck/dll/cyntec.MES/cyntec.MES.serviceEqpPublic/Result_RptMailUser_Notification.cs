using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpPublic;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "CyntecMES")]
public class Result_RptMailUser_Notification : Basic_Result
{
	private string mail_SubjectField;

	private string mail_BodyField;

	private int _To_CountField;

	private int _Cc_CountField;

	private int _Bcc_CountField;

	private string[] receivers_ToField;

	private string[] receivers_CcField;

	private string[] receivers_BccField;

	[XmlElement(Order = 0)]
	public string Mail_Subject
	{
		get
		{
			return mail_SubjectField;
		}
		set
		{
			mail_SubjectField = value;
			RaisePropertyChanged("Mail_Subject");
		}
	}

	[XmlElement(Order = 1)]
	public string Mail_Body
	{
		get
		{
			return mail_BodyField;
		}
		set
		{
			mail_BodyField = value;
			RaisePropertyChanged("Mail_Body");
		}
	}

	[XmlElement(Order = 2)]
	public int _To_Count
	{
		get
		{
			return _To_CountField;
		}
		set
		{
			_To_CountField = value;
			RaisePropertyChanged("_To_Count");
		}
	}

	[XmlElement(Order = 3)]
	public int _Cc_Count
	{
		get
		{
			return _Cc_CountField;
		}
		set
		{
			_Cc_CountField = value;
			RaisePropertyChanged("_Cc_Count");
		}
	}

	[XmlElement(Order = 4)]
	public int _Bcc_Count
	{
		get
		{
			return _Bcc_CountField;
		}
		set
		{
			_Bcc_CountField = value;
			RaisePropertyChanged("_Bcc_Count");
		}
	}

	[XmlArray(Order = 5)]
	public string[] Receivers_To
	{
		get
		{
			return receivers_ToField;
		}
		set
		{
			receivers_ToField = value;
			RaisePropertyChanged("Receivers_To");
		}
	}

	[XmlArray(Order = 6)]
	public string[] Receivers_Cc
	{
		get
		{
			return receivers_CcField;
		}
		set
		{
			receivers_CcField = value;
			RaisePropertyChanged("Receivers_Cc");
		}
	}

	[XmlArray(Order = 7)]
	public string[] Receivers_Bcc
	{
		get
		{
			return receivers_BccField;
		}
		set
		{
			receivers_BccField = value;
			RaisePropertyChanged("Receivers_Bcc");
		}
	}
}
