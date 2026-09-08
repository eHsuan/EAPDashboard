using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace cyntec.wcfConnection.wcfServiceRef;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IwcfServiceChannel : IwcfService, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
