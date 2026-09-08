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
public class NGItem : INotifyPropertyChanged
{
	private string nGCodeField;

	private decimal nGQtyField;

	private decimal finalNGQtyField;

	private bool isInsertedField;

	[XmlElement(Order = 0)]
	public string NGCode
	{
		get
		{
			return nGCodeField;
		}
		set
		{
			nGCodeField = value;
			RaisePropertyChanged("NGCode");
		}
	}

	[XmlElement(Order = 1)]
	public decimal NGQty
	{
		get
		{
			return nGQtyField;
		}
		set
		{
			nGQtyField = value;
			RaisePropertyChanged("NGQty");
		}
	}

	[XmlElement(Order = 2)]
	public decimal FinalNGQty
	{
		get
		{
			return finalNGQtyField;
		}
		set
		{
			finalNGQtyField = value;
			RaisePropertyChanged("FinalNGQty");
		}
	}

	[XmlElement(Order = 3)]
	public bool IsInserted
	{
		get
		{
			return isInsertedField;
		}
		set
		{
			isInsertedField = value;
			RaisePropertyChanged("IsInserted");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
