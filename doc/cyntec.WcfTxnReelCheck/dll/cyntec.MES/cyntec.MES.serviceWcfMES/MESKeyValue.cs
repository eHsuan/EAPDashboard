using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MESKeyValue", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class MESKeyValue : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string KeyField;

	[OptionalField]
	private string ValueField;

	[Browsable(false)]
	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public string Key
	{
		get
		{
			return KeyField;
		}
		set
		{
			if ((object)KeyField != value)
			{
				KeyField = value;
				RaisePropertyChanged("Key");
			}
		}
	}

	[DataMember]
	public string Value
	{
		get
		{
			return ValueField;
		}
		set
		{
			if ((object)ValueField != value)
			{
				ValueField = value;
				RaisePropertyChanged("Value");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
