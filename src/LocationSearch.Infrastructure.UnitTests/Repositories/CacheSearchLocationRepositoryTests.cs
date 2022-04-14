using System;
using System.Linq;
using System.Threading.Tasks;
using LocationSearch.Infrastructure.Cache;
using LocationSearch.Infrastructure.Repositories;
using NUnit.Framework;

namespace LocationSearch.Infrastructure.UnitTests.Repositories;

[TestFixture]
public class CacheSearchLocationRepositoryTests
{
    private InMemoryAddressCache _addressCache;
    private CacheSearchLocationRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _addressCache = new InMemoryAddressCache();
        _repository = new CacheSearchLocationRepository(_addressCache);
    }

    [Test]
    public void Search_LocationIsNull_ThrowsException()
    {
        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => _repository.Search(null, 0, 0));
    }

    [Test]
    public void Search_MaxDistanceLessThanZero_ThrowsException()
    {
        // Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _repository.Search(TestDataHelper.GenerateRandomLocation(), -1, 12));
    }

    [Test]
    public void Search_MaxResultsLessThanZero_ThrowsException()
    {
        // Assert
        Assert.ThrowsAsync<ArgumentException>(() => _repository.Search(TestDataHelper.GenerateRandomLocation(), 0, -1));
    }

    [Test]
    public async Task Search_CorrectInput_ReturnsAddressesFilteredByDistanceAndSorted()
    {
        // Arrange
        var location = TestDataHelper.GenerateRandomLocation();
        var maxDistance = 100000;
        var maxResults = 50;
        var addresses = TestDataHelper.GenerateAddresses(100000).ToList();

        await _addressCache.Set(addresses);

        // Act
        var results = await _repository.Search(location, maxDistance, maxResults);

        // Assert
        var filteredLocations = addresses
            .Select(address =>
                new SearchLocationResult.ResultItem(address.Location, address.FullAddress,
                    address.Location.CalculateDistance(location)))
            .Where(item => item.Distance <= maxDistance)
            .OrderBy(item => item.Distance)
            .Take(maxResults);

        CollectionAssert.AreEqual(filteredLocations, results.Results);
    }
}