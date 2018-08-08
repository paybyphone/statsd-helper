namespace StatsDHelper
{
    internal interface IPrefixProvider
    {
        string GetPrefix(StatsDHelperConfig config);
    }
}