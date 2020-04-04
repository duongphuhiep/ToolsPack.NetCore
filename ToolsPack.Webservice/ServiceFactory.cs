using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace ToolsPack.Webservice
{
    public static class ServiceFactory<T>
    {
        public static T GetServiceProxy(Uri url, int maxReceivedMessageSize)
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

        public static T GetServiceProxy(Uri url)
        {
            return GetServiceProxy(url, 100000);
        }
    }
}
