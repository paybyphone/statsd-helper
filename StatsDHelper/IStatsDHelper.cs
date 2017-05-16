using StatsdClient;

namespace StatsDHelper
{
    public interface IStatsDHelper
    {
        void LogCount(string name, int count = 1, object tagObject = null);
        void LogGauge(string name, int value, object tagObject = null);
        void LogTiming(string name, long milliseconds, object tagObject = null);
        void LogSet(string name, int value, object tagObject = null);
        IStatsd StatsdClient { get; }
    }
}