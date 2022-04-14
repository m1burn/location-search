using LocationSearch.Core;

namespace LocationSearch.WebApi;

public class WebApplicationConfiguration : IApplicationConfiguration
{
    public WebApplicationConfiguration(IConfiguration configuration)
    {
        LocationsCsvFilePath = configuration["LocationsCsvFilePath"];
        if (string.IsNullOrWhiteSpace(LocationsCsvFilePath))
        {
            throw new ConfigurationException("LocationsCsvFilePath configuration is required");
        }

        if (!File.Exists(LocationsCsvFilePath))
        {
            throw new ConfigurationException($"File \"{LocationsCsvFilePath}\" doesn't exist");
        }
    }

    public string LocationsCsvFilePath { get; }
}