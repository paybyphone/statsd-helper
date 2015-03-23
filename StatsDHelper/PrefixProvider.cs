using System.Configuration;
using System.Linq;

namespace StatsDHelper
{
    internal class PrefixProvider : IPrefixProvider
    {
        private readonly IHostPropertiesProvider _hostPropertiesProvider;

        public PrefixProvider(IHostPropertiesProvider hostPropertiesProvider)
        {
            _hostPropertiesProvider = hostPropertiesProvider;
        }

        public string GetPrefix()
        {
            var applicationName = ConfigurationManager.AppSettings["StatsD.ApplicationName"];
            return string.Format("{0}.{1}", GetFullyQualifiedDomainName(), applicationName);
        }

        string GetFullyQualifiedDomainName()
        {
            
            var domainName = _hostPropertiesProvider.GetDomainName();
            var hostName = _hostPropertiesProvider.GetHostName();
            var domainSegment = string.Join(".", domainName.Split('.').Reverse());

            return string.Format("{0}.{1}", domainSegment, hostName);
        }
    }
}