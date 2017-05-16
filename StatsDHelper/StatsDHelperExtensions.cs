namespace StatsDHelper
{
    public static class StatsDHelperExtensions
    {
        public static TimerToken LogTiming(this IStatsDHelper helper, string name, object tagObject = null)
        {
            return new TimerToken(helper, name, tagObject);
        }
    }
}