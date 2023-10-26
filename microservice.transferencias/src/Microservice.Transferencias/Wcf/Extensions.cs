using System.ServiceModel;

namespace Microservice.Transferencias.Wcf
{
    public static class Extensions
    {
        /// <summary>
        /// It is valid for BasicHttpBinding and BasicHttpsBinding
        /// See https://github.com/dotnet/wcf/blob/master/release-notes/WCF-Web-Service-Reference-notes.md
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void AddSecurityMessageBehavior<T>(this ClientBase<T> client, string username, string password) 
            where T : class
        {
            client.Endpoint.EndpointBehaviors.Add(new MessageOutgoingBehavior(username, password));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="logger"></param>
        public static void AddSecurityMessageBehavior<T>(this ClientBase<T> client, string username, string password, ILogger logger) 
            where T : class
        {
            client.Endpoint.EndpointBehaviors.Add(new MessageOutgoingBehavior(username, password, logger));
        }
    }
}
