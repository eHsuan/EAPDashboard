using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_ReelCheck_GetCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_ReelCheck_GetCondition : Condition
{
	[OptionalField]
	private string _WoNoField;

	[OptionalField]
	private string _TPNOField;

	[OptionalField]
	private string _ReelIDField;

	[DataMember]
	public string _WoNo
	{
		get
		{
			return _WoNoField;
		}
		set
		{
			if ((object)_WoNoField != value)
			{
				_WoNoField = value;
				RaisePropertyChanged("_WoNo");
			}
		}
	}

	[DataMember(Order = 1)]
	public string _TPNO
	{
		get
		{
			return _TPNOField;
		}
		set
		{
			if ((object)_TPNOField != value)
			{
				_TPNOField = value;
				RaisePropertyChanged("_TPNO");
			}
		}
	}

	[DataMember(Order = 2)]
	public string _ReelID
	{
		get
		{
			return _ReelIDField;
		}
		set
		{
			if ((object)_ReelIDField != value)
			{
				_ReelIDField = value;
				RaisePropertyChanged("_ReelID");
			}
		}
	}
}
