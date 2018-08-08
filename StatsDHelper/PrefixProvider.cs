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

        public string GetPrefix(StatsDHelperConfig config)
        {
            if (config == null)
            {
                throw new System.ArgumentNullException(nameof(config));
            }

            var applicationName = config.ApplicationName;
            return $"{GetFullyQualifiedDomainName()}.{applicationName}";
        }

        string GetFullyQualifiedDomainName()
        {
            var domainName = _hostPropertiesProvider.GetDomainName();
            var hostName = _hostPropertiesProvider.GetHostName();
            var domainSegment = string.Join(".", domainName.Split('.').Reverse());

            return $"{domainSegment}.{hostName}";
        }
    }
}