using StatsdClient;

namespace StatsDHelper
{
    public class NullStatsDHelper : IStatsDHelper
    {
        internal NullStatsDHelper() { }
        public void LogCount(string name, int count = 1) {}
        public void LogGauge(string name, int value) {}
        public void LogTiming(string name, long milliseconds) {}
        public void LogSet(string name, int value) {}

        public IStatsd StatsdClient
        {
            get { return null; }
        }
    }
}