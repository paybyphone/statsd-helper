using System.Configuration;

namespace StatsDHelper.Tests
{
    public abstract class BaseTest
    {
        private const string TestHost = "host";
        private const int TestPort = 8152;
        protected const string TestApplicationName = "ApplicationName";

        protected StatsDHelperConfig StatsDHelperConfig = new StatsDHelperConfig()
        {
            ApplicationName = "testApplicationName",
            StatsDServerPort = 8125,
            StatsDServerHost = "testStatsDServerHost"
        };
    }
}