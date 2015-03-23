using StatsdClient;

namespace StatsDHelper
{
    public interface IStatsDHelper
    {
        void LogCount(string name, int count = 1);
        void LogGauge(string name, int value);
        void LogTiming(string name, long milliseconds);
        void LogSet(string name, int value);
        IStatsd StatsdClient { get; }
    }
}