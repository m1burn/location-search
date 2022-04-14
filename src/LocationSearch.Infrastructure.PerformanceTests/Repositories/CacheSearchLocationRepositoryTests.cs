using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LocationSearch.Infrastructure.Cache;
using LocationSearch.Infrastructure.Repositories;
using NUnit.Framework;

namespace LocationSearch.Infrastructure.PerformanceTests.Repositories;

[TestFixture]
public class CacheSearchLocationRepositoryTests
{
    private IAddressCache _addressCache;
    private CacheSearchLocationRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _addressCache = new InMemoryAddressCache();
        _repository = new CacheSearchLocationRepository(_addressCache);
    }

    /// <summary>
    /// Increases number of locations and verify that search time is within 50ms
    /// </summary>
    [Test]
    [TestCase(100_000)]
    [TestCase(1_000_000)]
    [TestCase(2_000_000)]
    [TestCase(3_000_000)]
    public async Task SearchShouldNotSlowDownSignificantlyIfTheNumberOfLocationsIncreases(int numberOfLocations)
    {
        // Arrange
        var stopwatch = new Stopwatch();
        
        await _addressCache.Set(TestDataHelper.GenerateAddresses(numberOfLocations));
        
        stopwatch.Start();

        // Act
        var result = await _repository.Search(TestDataHelper.GenerateRandomLocation(), 100000, 1000);

        // Assert
        stopwatch.Stop();

        Console.WriteLine(
            $"Search time: {stopwatch.ElapsedMilliseconds}ms. Found locations: {result.Results.Count}");

        // Search time must be less than 50ms
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(50));
    }
}