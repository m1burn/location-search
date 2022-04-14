using System;
using Microsoft.Extensions.Configuration;

namespace LocationSearch.WebApi.PerformanceTests;

public static class ApiTestsConfiguration
{
    static ApiTestsConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, false);

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (!string.IsNullOrWhiteSpace(environment))
            configurationBuilder = configurationBuilder.AddJsonFile($"appsettings.{environment}.json", false, false);

        var configuration = configurationBuilder.Build();

        BaseUrl = configuration["BaseUrl"];
        if (BaseUrl.EndsWith("/")) BaseUrl = BaseUrl.Substring(0, BaseUrl.Length - 1);

        TestCasesPath = configuration["TestCasesPath"];
    }

    public static string BaseUrl { get; }

    public static string TestCasesPath { get; }
}