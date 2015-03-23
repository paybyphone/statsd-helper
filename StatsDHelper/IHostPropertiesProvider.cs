namespace StatsDHelper
{
    internal interface IHostPropertiesProvider
    {
        string GetDomainName();
        string GetHostName();
    }
}