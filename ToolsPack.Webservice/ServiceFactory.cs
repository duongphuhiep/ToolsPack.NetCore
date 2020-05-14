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

#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable CC0022 // Should dispose object
            var factory = new ChannelFactory<T>(binding, endpoint);
#pragma warning restore CC0022 // Should dispose object
#pragma warning restore CA2000 // Dispose objects before losing scope
            return factory.CreateChannel();
        }

        /// <summary>
        /// tell the service to use the logger to log raw request, using the given logger
        /// </summary>
        public static void AddLog<T>(this ClientBase<T> svc, ILogger logger, LogLevel level = LogLevel.Trace) where T : class
        {
            svc?.Endpoint.EndpointBehaviors.Add(new LoggingEndpointBehaviour(new LoggingMessageInspector(logger, level)));
        }
    }
}
