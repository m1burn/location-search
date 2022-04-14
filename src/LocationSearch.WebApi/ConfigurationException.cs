namespace LocationSearch.WebApi;

public class ConfigurationException : Exception
{
    public ConfigurationException(string message)
        : base(message)
    {
    }
}