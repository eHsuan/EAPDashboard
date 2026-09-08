using System.CodeDom.Compiler;
using System.ServiceModel;

namespace cyntec.MES.serviceEQP;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "CyntecMES", ConfigurationName = "serviceEQP.Eqp_PortalSoap")]
public interface Eqp_PortalSoap
{
	[OperationContract(Action = "CyntecMES/EqpTransaction", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	string EqpTransaction(string pParameter);

	[OperationContract(Action = "CyntecMES/InsertEqpErrorLog", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	string InsertEqpErrorLog(string pParameter);

	[OperationContract(Action = "CyntecMES/InsertEqpTransactionLog", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	string InsertEqpTransactionLog(string pParameter);
}
