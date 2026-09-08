using System.CodeDom.Compiler;
using System.ServiceModel;

namespace cyntec.MES.serviceVPack;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "serviceVPack.VPackSoap")]
public interface VPackSoap
{
	[OperationContract(Action = "http://tempuri.org/HelloWorld", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	string HelloWorld();

	[OperationContract(Action = "http://tempuri.org/StorBoxBinding", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ResultOfString StorBoxBinding(string userID, string password, string woNo, string barcode, string storBoxNo, string empNo, string eqpNo);
}
