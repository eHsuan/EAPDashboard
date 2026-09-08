using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpPublic;

[Serializable]
[XmlInclude(typeof(Result_WOParm))]
[XmlInclude(typeof(Result_RptMailUser_Notification))]
[XmlInclude(typeof(Result_GetDataTable_EF))]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "CyntecMES")]
public class Basic_Result : INotifyPropertyChanged
{
	private bool _isSuccessField;

	private string _resultCodeField;

	private string _errorMessageField;

	private string _sQLErrorMessageField;

	private string _userIDField;

	private bool _isExistField;

	[XmlElement(Order = 0)]
	public bool _isSuccess
	{
		get
		{
			return _isSuccessField;
		}
		set
		{
			_isSuccessField = value;
			RaisePropertyChanged("_isSuccess");
		}
	}

	[XmlElement(Order = 1)]
	public string _resultCode
	{
		get
		{
			return _resultCodeField;
		}
		set
		{
			_resultCodeField = value;
			RaisePropertyChanged("_resultCode");
		}
	}

	[XmlElement(Order = 2)]
	public string _errorMessage
	{
		get
		{
			return _errorMessageField;
		}
		set
		{
			_errorMessageField = value;
			RaisePropertyChanged("_errorMessage");
		}
	}

	[XmlElement(Order = 3)]
	public string _sQLErrorMessage
	{
		get
		{
			return _sQLErrorMessageField;
		}
		set
		{
			_sQLErrorMessageField = value;
			RaisePropertyChanged("_sQLErrorMessage");
		}
	}

	[XmlElement(Order = 4)]
	public string _userID
	{
		get
		{
			return _userIDField;
		}
		set
		{
			_userIDField = value;
			RaisePropertyChanged("_userID");
		}
	}

	[XmlElement(Order = 5)]
	public bool _isExist
	{
		get
		{
			return _isExistField;
		}
		set
		{
			_isExistField = value;
			RaisePropertyChanged("_isExist");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
