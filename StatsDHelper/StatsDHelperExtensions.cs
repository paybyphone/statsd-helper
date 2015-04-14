namespace StatsDHelper
{
    public static class StatsDHelperExtensions
    {
        public static TimerToken LogTiming(this IStatsDHelper helper, string name)
        {
            return new TimerToken(helper, name);
        }
    }
}