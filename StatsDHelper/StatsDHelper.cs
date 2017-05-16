using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
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

        internal StatsDHelper(IPrefixProvider prefixProvider, IStatsd statsdClient)
        {
            _prefixProvider = prefixProvider;
            _statsdClient = statsdClient;
        }

        public IStatsd StatsdClient
        {
            get{return _statsdClient;}
        }

        public void LogCount(string name, int count = 1, object tagObject = null)
        {
            SafeCaller(() => _statsdClient.LogCount(string.Format("{0}.{1}", GetStandardPrefix, name), count, ExtractTags(tagObject).ToArray()));
        }

        public void LogGauge(string name, int value, object tagObject = null)
        {
            SafeCaller(() => _statsdClient.LogGauge(string.Format("{0}.{1}", GetStandardPrefix, name), value, ExtractTags(tagObject).ToArray()));
        }

        public void LogTiming(string name, long milliseconds, object tagObject = null)
        {
            SafeCaller(() => _statsdClient.LogTiming(string.Format("{0}.{1}", GetStandardPrefix, name), milliseconds, ExtractTags(tagObject).ToArray()));
        }

        public void LogSet(string name, int value, object tagObject = null)
        {
            SafeCaller(() => _statsdClient.LogSet(string.Format("{0}.{1}", GetStandardPrefix, name), value, ExtractTags(tagObject).ToArray()));
        }

        public void LogCount(string name, int count = 1, params KeyValuePair<string, string>[] tags)
        {
            SafeCaller(() => _statsdClient.LogCount(string.Format("{0}.{1}", GetStandardPrefix, name), count, tags));
        }

        public void LogGauge(string name, int value, params KeyValuePair<string, string>[] tags)
        {
            SafeCaller(() => _statsdClient.LogGauge(string.Format("{0}.{1}", GetStandardPrefix, name), value, tags));
        }

        public void LogTiming(string name, long milliseconds, params KeyValuePair<string, string>[] tags)
        {
            SafeCaller(() => _statsdClient.LogTiming(string.Format("{0}.{1}", GetStandardPrefix, name), milliseconds, tags));
        }

        public void LogSet(string name, int value, params KeyValuePair<string, string>[] tags)
        {
            SafeCaller(() => _statsdClient.LogSet(string.Format("{0}.{1}", GetStandardPrefix, name), value, tags));
        }

        private static IEnumerable<KeyValuePair<string, string>> ExtractTags(object tagObject)
        {
            if (tagObject == null)
            {
                yield break;
            }

            var objectType = tagObject.GetType();
            var properties = objectType.GetProperties();

            foreach (var property in properties)
            {
                yield return new KeyValuePair<string, string>(property.Name, property.GetValue(tagObject, null).ToString());
            }
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
                    _prefix = _prefixProvider.GetPrefix();
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
                            var host = ConfigurationManager.AppSettings["StatsD.Host"];
                            var port = ConfigurationManager.AppSettings["StatsD.Port"];
                            var applicationName = ConfigurationManager.AppSettings["StatsD.ApplicationName"];

                            if (string.IsNullOrEmpty(host)
                                || string.IsNullOrEmpty(port)
                                || string.IsNullOrEmpty(applicationName))
                            {
                                Debug.WriteLine("One or more StatsD Client Settings missing. Ensure an application name, host and port are set or no metrics will be sent. Set Values: Host={0} Port={1}");
                                return new NullStatsDHelper();
                            }

                            _instance = new StatsDHelper(new PrefixProvider(new HostPropertiesProvider()), new Statsd(host, int.Parse(port)));
                        }
                    }
                }
                return _instance;
            }
        }
    }
}