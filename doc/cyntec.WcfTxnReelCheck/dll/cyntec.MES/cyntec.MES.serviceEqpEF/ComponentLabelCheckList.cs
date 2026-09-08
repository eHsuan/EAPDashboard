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
public class ComponentLabelCheckList : INotifyPropertyChanged
{
	private ComponentLabelInfo[] unCheckComponentListField;

	private ComponentLabelInfo[] checkedComponentListField;

	[XmlArray(Order = 0)]
	public ComponentLabelInfo[] UnCheckComponentList
	{
		get
		{
			return unCheckComponentListField;
		}
		set
		{
			unCheckComponentListField = value;
			RaisePropertyChanged("UnCheckComponentList");
		}
	}

	[XmlArray(Order = 1)]
	public ComponentLabelInfo[] CheckedComponentList
	{
		get
		{
			return checkedComponentListField;
		}
		set
		{
			checkedComponentListField = value;
			RaisePropertyChanged("CheckedComponentList");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
