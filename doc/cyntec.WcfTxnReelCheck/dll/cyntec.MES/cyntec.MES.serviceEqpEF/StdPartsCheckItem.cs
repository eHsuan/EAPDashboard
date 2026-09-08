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
public class StdPartsCheckItem : INotifyPropertyChanged
{
	private string stdPartItemField;

	private string stdPartParamField;

	private string paramValueField;

	private string upperValueField;

	private string lowerValueField;

	private string stdPartParamSeqField;

	private string frequenceField;

	private string frequenceUnitField;

	private string unitField;

	private string[] dataListField;

	[XmlElement(Order = 0)]
	public string StdPartItem
	{
		get
		{
			return stdPartItemField;
		}
		set
		{
			stdPartItemField = value;
			RaisePropertyChanged("StdPartItem");
		}
	}

	[XmlElement(Order = 1)]
	public string StdPartParam
	{
		get
		{
			return stdPartParamField;
		}
		set
		{
			stdPartParamField = value;
			RaisePropertyChanged("StdPartParam");
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
	public string StdPartParamSeq
	{
		get
		{
			return stdPartParamSeqField;
		}
		set
		{
			stdPartParamSeqField = value;
			RaisePropertyChanged("StdPartParamSeq");
		}
	}

	[XmlElement(Order = 6)]
	public string Frequence
	{
		get
		{
			return frequenceField;
		}
		set
		{
			frequenceField = value;
			RaisePropertyChanged("Frequence");
		}
	}

	[XmlElement(Order = 7)]
	public string FrequenceUnit
	{
		get
		{
			return frequenceUnitField;
		}
		set
		{
			frequenceUnitField = value;
			RaisePropertyChanged("FrequenceUnit");
		}
	}

	[XmlElement(Order = 8)]
	public string Unit
	{
		get
		{
			return unitField;
		}
		set
		{
			unitField = value;
			RaisePropertyChanged("Unit");
		}
	}

	[XmlArray(Order = 9)]
	public string[] DataList
	{
		get
		{
			return dataListField;
		}
		set
		{
			dataListField = value;
			RaisePropertyChanged("DataList");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
