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
[MessageContract(WrapperName = "EqpWOInputAdd", WrapperNamespace = "http://CyntecMES", IsWrapped = true)]
public class EqpWOInputAddRequest
{
	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 0)]
	public string pEqpNo;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 1)]
	public string pWONO;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 2)]
	public string pUserID;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 3)]
	public WOInputItem[] pWOInputItemList;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 4)]
	[XmlElement(IsNullable = true)]
	public DateTime? pModifyDate;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 5)]
	public bool IsSkipRepeat;

	public EqpWOInputAddRequest()
	{
	}

	public EqpWOInputAddRequest(string pEqpNo, string pWONO, string pUserID, WOInputItem[] pWOInputItemList, DateTime? pModifyDate, bool IsSkipRepeat)
	{
		this.pEqpNo = pEqpNo;
		this.pWONO = pWONO;
		this.pUserID = pUserID;
		this.pWOInputItemList = pWOInputItemList;
		this.pModifyDate = pModifyDate;
		this.IsSkipRepeat = IsSkipRepeat;
	}
}
