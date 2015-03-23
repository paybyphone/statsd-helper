using System.Net;
using System.Net.NetworkInformation;

namespace StatsDHelper
{
    internal class HostPropertiesProvider : IHostPropertiesProvider
    {
        public string GetDomainName()
        {
            return IPGlobalProperties.GetIPGlobalProperties().DomainName;
        }

        public string GetHostName()
        {
            return Dns.GetHostName();
        }
    }
}