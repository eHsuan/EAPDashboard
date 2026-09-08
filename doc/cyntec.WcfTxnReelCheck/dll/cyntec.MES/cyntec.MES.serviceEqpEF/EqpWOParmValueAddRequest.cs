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
[MessageContract(WrapperName = "EqpWOParmValueAdd", WrapperNamespace = "http://CyntecMES", IsWrapped = true)]
public class EqpWOParmValueAddRequest
{
	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 0)]
	public string pEqpNo;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 1)]
	public string pWONO;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 2)]
	public string pUserID;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 3)]
	public string pInOut;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 4)]
	public WOParmValue[] pWOParmValueList;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 5)]
	[XmlElement(IsNullable = true)]
	public DateTime? pModifyDate;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 6)]
	public bool IsSkipRepeat;

	public EqpWOParmValueAddRequest()
	{
	}

	public EqpWOParmValueAddRequest(string pEqpNo, string pWONO, string pUserID, string pInOut, WOParmValue[] pWOParmValueList, DateTime? pModifyDate, bool IsSkipRepeat)
	{
		this.pEqpNo = pEqpNo;
		this.pWONO = pWONO;
		this.pUserID = pUserID;
		this.pInOut = pInOut;
		this.pWOParmValueList = pWOParmValueList;
		this.pModifyDate = pModifyDate;
		this.IsSkipRepeat = IsSkipRepeat;
	}
}
