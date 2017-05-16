using StatsdClient;

namespace StatsDHelper
{
    public class NullStatsDHelper : IStatsDHelper
    {
        internal NullStatsDHelper() { }

        public void LogCount(string name, int count = 1, object tagObject = null) {}

        public void LogGauge(string name, int value, object tagObject = null) { }

        public void LogTiming(string name, long milliseconds, object tagObject = null) { }

        public void LogSet(string name, int value, object tagObject = null) { }

        public IStatsd StatsdClient
        {
            get { return null; }
        }
    }
}