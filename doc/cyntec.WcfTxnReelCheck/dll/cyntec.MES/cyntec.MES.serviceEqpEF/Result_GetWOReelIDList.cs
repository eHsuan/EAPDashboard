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
public class Result_GetWOReelIDList : Basic_Result
{
	private ReelIDSet[] reelIDSetListField;

	[XmlArray(Order = 0)]
	public ReelIDSet[] ReelIDSetList
	{
		get
		{
			return reelIDSetListField;
		}
		set
		{
			reelIDSetListField = value;
			RaisePropertyChanged("ReelIDSetList");
		}
	}
}
