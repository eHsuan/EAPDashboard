using System;
using System.Xml.Linq;

namespace cyntec.WCF;

public interface IwcfMessage
{
	string msgID { get; set; }

	string msgFileName { get; }

	string msgFullClassName { get; }

	string msgClassName { get; }

	string msgFrom { get; set; }

	string msgTo { get; set; }

	string msgTxnID { get; set; }

	string XElementData { get; set; }

	string Message { get; set; }

	DateTime MsgCreateDate { get; set; }

	DateTime MsgFinishDate { get; set; }

	byte[] ToByte();

	XElement ToXElement();
}
