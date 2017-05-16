using System;
using System.Diagnostics;

namespace StatsDHelper
{
    //No way to extend TimerToken in the client so just reimplent using the helper
    public sealed class TimerToken : IDisposable
    {
        private readonly IStatsDHelper _helper;
        private readonly string _name;
        private readonly object _tagObject;
        private readonly Stopwatch _stopwatch;

        internal TimerToken(IStatsDHelper helper, string name, object tagObject = null)
        {
            _stopwatch = Stopwatch.StartNew();
            _helper = helper;
            _name = name;
            _tagObject = tagObject;
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _helper.LogTiming(_name, (int)_stopwatch.ElapsedMilliseconds, _tagObject);
        }
    }
}