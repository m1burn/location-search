using System;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Threading.Tasks;
using CsvHelper;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using NUnit.Framework;

namespace LocationSearch.WebApi.PerformanceTests;

/// <summary>
/// The test performs stress load testing of developed Web API.
/// The current setup maintains 100 RPS for 1 min and asserts that no requests failed. Also, it prints latency statistics.
/// Before running it, please make sure that LocationSearch.WebApi is running.
/// Also, you may need to update BaseUrl configuration in appsettings.json file.
/// The test verifies that API can handle concurrent requests.
/// </summary>
public class Tests
{
    [Test]
    public void StressTest()
    {
        var testCases = LoadTestCases();
        var handler = new SocketsHttpHandler
        {
            AutomaticDecompression = DecompressionMethods.None,
            PooledConnectionLifetime = TimeSpan.FromMinutes(10),
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(10),
            MaxConnectionsPerServer = 10000,
            EnableMultipleHttp2Connections = true,
            UseCookies = false,
            UseProxy = false,
            SslOptions = new SslClientAuthenticationOptions
            {
                RemoteCertificateValidationCallback = delegate { return true; },
            }
        };
        var httpClient = new HttpClient(handler);
        var httpFactory = HttpClientFactory.Create("http_factory", httpClient);
        var testCasesFeed = Feed.CreateCircular("TestCasesFeed", testCases);
        var testStep = Step.Create("ApiRequestStep", httpFactory, testCasesFeed, async context =>
        {
            var request = Http.CreateRequest("GET", context.FeedItem.Url)
                .WithHeader("Accept", "application/json")
                .WithCheck(response => Task.FromResult(response.IsSuccessStatusCode ? Response.Ok() : Response.Fail()));

            return await Http.Send(request, context);
        }, TimeSpan.FromMinutes(10));

        var scenario = ScenarioBuilder
            .CreateScenario("ApiStressTest", testStep)
            .WithoutWarmUp()
            .WithLoadSimulations(Simulation.KeepConstant(100, TimeSpan.FromMinutes(1)));

        var nodeStats = NBomberRunner.RegisterScenarios(scenario).Run();
        var stepStats = nodeStats.ScenarioStats[0].StepStats[0];

        Assert.That(stepStats.Fail.Request.Count, Is.Zero);
    }

    private ImmutableList<TestCaseModel> LoadTestCases()
    {
        using var reader = new StreamReader(ApiTestsConfiguration.TestCasesPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var rnd = new Random();
        return csv.GetRecords<Row>()
            .Where(row => row.Latitude != null && row.Longitude != null)
            .Select(row =>
            {
                var url =
                    $"{ApiTestsConfiguration.BaseUrl}/search?latitude={row.Latitude}&longitude={row.Longitude}&maxDistance={rnd.Next(1, 10000)}&maxResults={rnd.Next(1, 100)}";
                return new TestCaseModel(url);
            })
            .ToImmutableList();
    }

    private record TestCaseModel(string Url);

    private class Row
    {
        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}