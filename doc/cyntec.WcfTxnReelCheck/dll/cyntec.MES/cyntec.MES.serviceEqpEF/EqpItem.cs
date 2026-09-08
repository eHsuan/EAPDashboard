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
public class EqpItem : INotifyPropertyChanged
{
	private string eqpNoField;

	private bool insertedField;

	[XmlElement(Order = 0)]
	public string EqpNo
	{
		get
		{
			return eqpNoField;
		}
		set
		{
			eqpNoField = value;
			RaisePropertyChanged("EqpNo");
		}
	}

	[XmlElement(Order = 1)]
	public bool Inserted
	{
		get
		{
			return insertedField;
		}
		set
		{
			insertedField = value;
			RaisePropertyChanged("Inserted");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
