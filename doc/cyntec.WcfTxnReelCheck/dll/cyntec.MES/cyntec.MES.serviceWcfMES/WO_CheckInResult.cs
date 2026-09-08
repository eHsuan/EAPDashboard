using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_CheckInResult", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_CheckInResult : TxnInfo
{
	[OptionalField]
	private string strTxnJsonField;

	[OptionalField]
	private string strTxnResultJsonField;

	[DataMember]
	public string strTxnJson
	{
		get
		{
			return strTxnJsonField;
		}
		set
		{
			if ((object)strTxnJsonField != value)
			{
				strTxnJsonField = value;
				RaisePropertyChanged("strTxnJson");
			}
		}
	}

	[DataMember]
	public string strTxnResultJson
	{
		get
		{
			return strTxnResultJsonField;
		}
		set
		{
			if ((object)strTxnResultJsonField != value)
			{
				strTxnResultJsonField = value;
				RaisePropertyChanged("strTxnResultJson");
			}
		}
	}
}
