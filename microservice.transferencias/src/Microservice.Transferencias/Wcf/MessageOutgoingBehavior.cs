using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Microservice.Transferencias.Wcf
{
    public class MessageOutgoingBehavior : IEndpointBehavior
    {
        private readonly ILogger _logger;
        private readonly string _username;
        private readonly string _password;

        public MessageOutgoingBehavior(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public MessageOutgoingBehavior(string username, string password, ILogger logger) : this(username, password)
        {
            _logger = logger;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new SecurityCreatorInspector(_username, _password));
            clientRuntime.ClientMessageInspectors.Add(new ClientMessageInspector(_logger));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}