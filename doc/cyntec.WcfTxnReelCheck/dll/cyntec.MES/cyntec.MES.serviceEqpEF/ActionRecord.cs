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
public class ActionRecord : INotifyPropertyChanged
{
	private action _actionField;

	private actionType _actionTypeField;

	private string _actionMemoField;

	private string _actionUserField;

	[XmlElement(Order = 0)]
	public action _action
	{
		get
		{
			return _actionField;
		}
		set
		{
			_actionField = value;
			RaisePropertyChanged("_action");
		}
	}

	[XmlElement(Order = 1)]
	public actionType _actionType
	{
		get
		{
			return _actionTypeField;
		}
		set
		{
			_actionTypeField = value;
			RaisePropertyChanged("_actionType");
		}
	}

	[XmlElement(Order = 2)]
	public string _actionMemo
	{
		get
		{
			return _actionMemoField;
		}
		set
		{
			_actionMemoField = value;
			RaisePropertyChanged("_actionMemo");
		}
	}

	[XmlElement(Order = 3)]
	public string _actionUser
	{
		get
		{
			return _actionUserField;
		}
		set
		{
			_actionUserField = value;
			RaisePropertyChanged("_actionUser");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
