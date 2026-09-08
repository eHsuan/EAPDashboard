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
[MessageContract(WrapperName = "EqpCarrierAdd", WrapperNamespace = "http://CyntecMES", IsWrapped = true)]
public class EqpCarrierAddRequest
{
	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 0)]
	public string pEqpNo;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 1)]
	public string pWONO;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 2)]
	public WOCarrierItem[] pCarrierNoList;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 3)]
	public string pInOut;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 4)]
	public string pUserID;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 5)]
	[XmlElement(IsNullable = true)]
	public DateTime? pModifyDate;

	[MessageBodyMember(Namespace = "http://CyntecMES", Order = 6)]
	public bool IsSkipRepeat;

	public EqpCarrierAddRequest()
	{
	}

	public EqpCarrierAddRequest(string pEqpNo, string pWONO, WOCarrierItem[] pCarrierNoList, string pInOut, string pUserID, DateTime? pModifyDate, bool IsSkipRepeat)
	{
		this.pEqpNo = pEqpNo;
		this.pWONO = pWONO;
		this.pCarrierNoList = pCarrierNoList;
		this.pInOut = pInOut;
		this.pUserID = pUserID;
		this.pModifyDate = pModifyDate;
		this.IsSkipRepeat = IsSkipRepeat;
	}
}
