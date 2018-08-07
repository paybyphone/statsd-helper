using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using StatsdClient;

[assembly:InternalsVisibleTo("StatsDHelper.Tests")]
[assembly:InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace StatsDHelper
{
    public class StatsDHelper : IStatsDHelper
    {
        private readonly IPrefixProvider _prefixProvider;
        private readonly IStatsd _statsDClient;
        private string _prefix;

        private static readonly object InstancePadlock = new object();
        private static IStatsDHelper _instance;
        
        private static readonly object ConfigPadlock = new object();
        private static StatsDHelperConfig _config;

        internal StatsDHelper(IPrefixProvider prefixProvider, IStatsd statsDClient)
        {
            _prefixProvider = prefixProvider;
            _statsDClient = statsDClient;
        }

        public IStatsd StatsdClient => _statsDClient;

        public void LogCount(string name, int count = 1)
        {
            SafeCaller(() => _statsDClient.LogCount($"{GetStandardPrefix}.{name}", count));
        }

        public void LogGauge(string name, int value)
        {
            SafeCaller(() => _statsDClient.LogGauge($"{GetStandardPrefix}.{name}", value));
        }

        public void LogTiming(string name, long milliseconds)
        {
            SafeCaller(() => _statsDClient.LogTiming($"{GetStandardPrefix}.{name}", milliseconds));
        }

        public void LogSet(string name, int value)
        {
            SafeCaller(() => _statsDClient.LogSet($"{GetStandardPrefix}.{name}", value));
        }

        private static void SafeCaller(Action action)
        {
            try
            {
                action();
            }
            catch (Exception) {}
        }

        private string GetStandardPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(_prefix))
                {
                    _prefix = _prefixProvider.GetPrefix(_config);
                }
                return _prefix;
            }
        }

        public static IStatsDHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (InstancePadlock)
                    {
                        if (_instance == null)
                        {
                            lock (ConfigPadlock)
                            {
                                if (_config == null)
                                {
                                    Debug.WriteLine(
                                        "The StatsD Helper need to be configured first by calling the Configure method.");
                                    throw new InvalidOperationException(
                                        "StatsD Helper configuration has not been set. Call Configure method before calling Instance.");
                                }

                                var host = _config.StatsDServerHost;
                                var port = _config.StatsDServerPort;
                                var applicationName = _config.ApplicationName;

                                if (string.IsNullOrEmpty(host)
                                    || !port.HasValue
                                    || string.IsNullOrEmpty(applicationName))
                                {
                                    Debug.WriteLine(
                                        "One or more StatsD Client Settings missing. Ensure an application name, host and port are set or no metrics will be sent. Set Values: Host={0} Port={1}");
                                    return new NullStatsDHelper();
                                }

                                _instance = new StatsDHelper(new PrefixProvider(new HostPropertiesProvider()),
                                    new Statsd(host, port.Value));
                            }
                        }
                    }
                }
                return _instance;
            }
        }

        public static void Configure(StatsDHelperConfig configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            lock (ConfigPadlock)
            {
                _config = configuration;
            }
        }

        internal static void Cleanup()
        {
            if (_instance != null)
            {
                lock (InstancePadlock)
                {
                    _instance = null;
                }
            }

            if (_config != null)
            {
                lock (ConfigPadlock)
                {
                    _config = null;
                }
            }
        }
    }
}