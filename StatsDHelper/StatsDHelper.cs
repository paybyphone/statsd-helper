using System;
using System.Configuration;
using System.Diagnostics;
using StatsdClient;

namespace StatsDHelper
{
    public class StatsDHelper : IStatsDHelper
    {
        private readonly IPrefixProvider _prefixProvider;
        private readonly IStatsd _statsdClient;
        private string _prefix;

        private static readonly object Padlock = new object();
        private static IStatsDHelper _instance;

        private static StatsDHelperConfig _config;

        internal StatsDHelper(IPrefixProvider prefixProvider, IStatsd statsdClient)
        {
            _prefixProvider = prefixProvider;
            _statsdClient = statsdClient;
        }

        public IStatsd StatsdClient
        {
            get{return _statsdClient;}
        }

        public void LogCount(string name, int count = 1)
        {
            SafeCaller(() => _statsdClient.LogCount(string.Format("{0}.{1}", GetStandardPrefix, name), count));
        }

        public void LogGauge(string name, int value)
        {
            SafeCaller(() => _statsdClient.LogGauge(string.Format("{0}.{1}", GetStandardPrefix, name), value));
        }

        public void LogTiming(string name, long milliseconds)
        {
            SafeCaller(() => _statsdClient.LogTiming(string.Format("{0}.{1}", GetStandardPrefix, name), milliseconds));
        }

        public void LogSet(string name, int value)
        {
            SafeCaller(() => _statsdClient.LogSet(string.Format("{0}.{1}", GetStandardPrefix, name), value));
        }

        private static void SafeCaller(Action action)
        {
            try
            {
                action();
            }
            catch (Exception) {}
        }

        public string GetStandardPrefix
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
                    lock (Padlock)
                    {
                        if (_instance == null)
                        {
                            if (_config == null)
                            {
                                Debug.WriteLine("The StatsD Helper need to be configured first by calling the Configure method.");
                                throw new InvalidOperationException("StatsD Helper configuration has not been set. Call Configure method before calling Instance.");
                            }

                            var host = _config.StatsDServerHost;
                            var port = _config.StatsDServerPort;
                            var applicationName = _config.ApplicationName;

                            if (string.IsNullOrEmpty(host)
                                || !port.HasValue
                                || string.IsNullOrEmpty(applicationName))
                            {
                                Debug.WriteLine("One or more StatsD Client Settings missing. Ensure an application name, host and port are set or no metrics will be sent. Set Values: Host={0} Port={1}");
                                return new NullStatsDHelper();
                            }

                            _instance = new StatsDHelper(new PrefixProvider(new HostPropertiesProvider()), new Statsd(host, port.Value));
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
        }
    }
}