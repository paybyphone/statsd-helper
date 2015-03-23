using System.Configuration;

namespace StatsDHelper.Tests
{
    public abstract class BaseTest
    {
        public const string TestHost = "host";
        public const int TestPort = 8152;
        public const string TestApplicationName = "ApplicationName";

        public void EmptyAppSettings()
        {
            var configuration = OpenConfiguration();

            configuration.AppSettings.Settings.Remove("StatsD.Host");
            configuration.AppSettings.Settings.Remove("StatsD.Port");
            configuration.AppSettings.Settings.Remove("StatsD.ApplicationName");
            SaveAndRefresh(configuration);
        }

        public void PopulateAppSettings()
        {
            EmptyAppSettings();
            var configuration = OpenConfiguration();
            configuration.AppSettings.Settings.Add("StatsD.Host", TestHost);
            configuration.AppSettings.Settings.Add("StatsD.Port", TestPort.ToString());
            configuration.AppSettings.Settings.Add("StatsD.ApplicationName", TestApplicationName);
            SaveAndRefresh(configuration);
        }      

        private static void SaveAndRefresh(Configuration configuration)
        {
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        private static Configuration OpenConfiguration()
        {
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
    }
}