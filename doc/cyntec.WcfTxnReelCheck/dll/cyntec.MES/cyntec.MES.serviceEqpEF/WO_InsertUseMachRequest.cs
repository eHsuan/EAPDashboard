using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpEF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(WrapperName = "WO_InsertUseMach", WrapperNamespace = "http://CyntecMES", IsWrapped = true)]
public class WO_InsertUseMachRequest
{
	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 0)]
	public string pEqpNo;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 1)]
	public string pWONO;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 2)]
	public string pUserID;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 3)]
	[XmlElement(IsNullable = true)]
	public DateTime? ModifyDate;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 4)]
	public bool IsSkipRepeat;

	public WO_InsertUseMachRequest()
	{
	}

	public WO_InsertUseMachRequest(string pEqpNo, string pWONO, string pUserID, DateTime? ModifyDate, bool IsSkipRepeat)
	{
		this.pEqpNo = pEqpNo;
		this.pWONO = pWONO;
		this.pUserID = pUserID;
		this.ModifyDate = ModifyDate;
		this.IsSkipRepeat = IsSkipRepeat;
	}
}
