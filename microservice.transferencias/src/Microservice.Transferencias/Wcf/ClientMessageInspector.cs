using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Microservice.Transferencias.Wcf
{
    public class ClientMessageInspector : IClientMessageInspector
    {
        private readonly ILogger _logger;

        public ClientMessageInspector(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (_logger != null)
            {
                if (reply != null)
                    _logger.LogDebug(reply.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (_logger != null)
            {
                if (request != null)
                    _logger.LogDebug(request.ToString());
            }

            return null;
        }
    }
}