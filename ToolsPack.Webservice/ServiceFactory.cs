using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ToolsPack.Webservice
{
    public static class ServiceFactory
    {
        [Obsolete("Newer svcutil generates the Client consumer, use it instead")]
        public static T CreateServiceProxy<T>(Uri url, int maxReceivedMessageSize = 100000) where T : class
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

            var endpoint = new EndpointAddress(url);

            HttpBindingBase binding;
            if (url.Scheme == Uri.UriSchemeHttps)
                binding = new BasicHttpsBinding();
            else binding = new BasicHttpBinding();

            binding.MaxReceivedMessageSize = maxReceivedMessageSize;

            var factory = new ChannelFactory<T>(binding, endpoint);
            return factory.CreateChannel();
        }

        /// <summary>
        /// tell the service to use the logger to log raw request, using the given logger
        /// </summary>
        public static void AddLog<T>(this ClientBase<T> svc, ILogger logger) where T : class
        {
            svc?.Endpoint.EndpointBehaviors.Add(new LoggingEndpointBehaviour(new LoggingMessageInspector(logger)));
        }
    }
}
