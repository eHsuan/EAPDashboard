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
public class StdPartsResultItem : INotifyPropertyChanged
{
	private string stdPartItemField;

	private string stdPartParamField;

	private string stdPartParamSeqField;

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

	[XmlArray(Order = 3)]
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
