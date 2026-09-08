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
public class InputCodeSet : INotifyPropertyChanged
{
	private string inputCodeField;

	private string inputCodeNameField;

	private string requireInputField;

	private string dataListItemField;

	private string defaultValueField;

	private string inputTypeField;

	private string lowerLimitField;

	private string upperLimitField;

	private string verifyField;

	private string enableField;

	[XmlElement(Order = 0)]
	public string InputCode
	{
		get
		{
			return inputCodeField;
		}
		set
		{
			inputCodeField = value;
			RaisePropertyChanged("InputCode");
		}
	}

	[XmlElement(Order = 1)]
	public string InputCodeName
	{
		get
		{
			return inputCodeNameField;
		}
		set
		{
			inputCodeNameField = value;
			RaisePropertyChanged("InputCodeName");
		}
	}

	[XmlElement(Order = 2)]
	public string RequireInput
	{
		get
		{
			return requireInputField;
		}
		set
		{
			requireInputField = value;
			RaisePropertyChanged("RequireInput");
		}
	}

	[XmlElement(Order = 3)]
	public string DataListItem
	{
		get
		{
			return dataListItemField;
		}
		set
		{
			dataListItemField = value;
			RaisePropertyChanged("DataListItem");
		}
	}

	[XmlElement(Order = 4)]
	public string DefaultValue
	{
		get
		{
			return defaultValueField;
		}
		set
		{
			defaultValueField = value;
			RaisePropertyChanged("DefaultValue");
		}
	}

	[XmlElement(Order = 5)]
	public string InputType
	{
		get
		{
			return inputTypeField;
		}
		set
		{
			inputTypeField = value;
			RaisePropertyChanged("InputType");
		}
	}

	[XmlElement(Order = 6)]
	public string LowerLimit
	{
		get
		{
			return lowerLimitField;
		}
		set
		{
			lowerLimitField = value;
			RaisePropertyChanged("LowerLimit");
		}
	}

	[XmlElement(Order = 7)]
	public string UpperLimit
	{
		get
		{
			return upperLimitField;
		}
		set
		{
			upperLimitField = value;
			RaisePropertyChanged("UpperLimit");
		}
	}

	[XmlElement(Order = 8)]
	public string Verify
	{
		get
		{
			return verifyField;
		}
		set
		{
			verifyField = value;
			RaisePropertyChanged("Verify");
		}
	}

	[XmlElement(Order = 9)]
	public string Enable
	{
		get
		{
			return enableField;
		}
		set
		{
			enableField = value;
			RaisePropertyChanged("Enable");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
