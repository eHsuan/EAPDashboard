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
public class ParamSet : INotifyPropertyChanged
{
	private string paramCodeField;

	private string paramNameField;

	private string paramValueField;

	private bool requireInputField;

	private string dataListItemField;

	private string defaultValueField;

	private string refFieldCodeField;

	private string upperValueField;

	private string lowerValueField;

	private bool showableField;

	[XmlElement(Order = 0)]
	public string ParamCode
	{
		get
		{
			return paramCodeField;
		}
		set
		{
			paramCodeField = value;
			RaisePropertyChanged("ParamCode");
		}
	}

	[XmlElement(Order = 1)]
	public string ParamName
	{
		get
		{
			return paramNameField;
		}
		set
		{
			paramNameField = value;
			RaisePropertyChanged("ParamName");
		}
	}

	[XmlElement(Order = 2)]
	public string ParamValue
	{
		get
		{
			return paramValueField;
		}
		set
		{
			paramValueField = value;
			RaisePropertyChanged("ParamValue");
		}
	}

	[XmlElement(Order = 3)]
	public bool RequireInput
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
	public string RefFieldCode
	{
		get
		{
			return refFieldCodeField;
		}
		set
		{
			refFieldCodeField = value;
			RaisePropertyChanged("RefFieldCode");
		}
	}

	[XmlElement(Order = 7)]
	public string UpperValue
	{
		get
		{
			return upperValueField;
		}
		set
		{
			upperValueField = value;
			RaisePropertyChanged("UpperValue");
		}
	}

	[XmlElement(Order = 8)]
	public string LowerValue
	{
		get
		{
			return lowerValueField;
		}
		set
		{
			lowerValueField = value;
			RaisePropertyChanged("LowerValue");
		}
	}

	[XmlElement(Order = 9)]
	public bool Showable
	{
		get
		{
			return showableField;
		}
		set
		{
			showableField = value;
			RaisePropertyChanged("Showable");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
