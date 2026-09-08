using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpEF;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://CyntecMES")]
public class BasicResultOfListOfTrackingCodeList : INotifyPropertyChanged
{
	private bool successField;

	private string resultCodeField;

	private int resultCountField;

	private string errorMessageField;

	private string sQLErrorMessageField;

	private TrackingCodeList[] dataField;

	[XmlElement(Order = 0)]
	public bool Success
	{
		get
		{
			return successField;
		}
		set
		{
			successField = value;
			RaisePropertyChanged("Success");
		}
	}

	[XmlElement(Order = 1)]
	public string ResultCode
	{
		get
		{
			return resultCodeField;
		}
		set
		{
			resultCodeField = value;
			RaisePropertyChanged("ResultCode");
		}
	}

	[XmlElement(Order = 2)]
	public int ResultCount
	{
		get
		{
			return resultCountField;
		}
		set
		{
			resultCountField = value;
			RaisePropertyChanged("ResultCount");
		}
	}

	[XmlElement(Order = 3)]
	public string ErrorMessage
	{
		get
		{
			return errorMessageField;
		}
		set
		{
			errorMessageField = value;
			RaisePropertyChanged("ErrorMessage");
		}
	}

	[XmlElement(Order = 4)]
	public string SQLErrorMessage
	{
		get
		{
			return sQLErrorMessageField;
		}
		set
		{
			sQLErrorMessageField = value;
			RaisePropertyChanged("SQLErrorMessage");
		}
	}

	[XmlArray(Order = 5)]
	public TrackingCodeList[] Data
	{
		get
		{
			return dataField;
		}
		set
		{
			dataField = value;
			RaisePropertyChanged("Data");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
