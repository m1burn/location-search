using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using LocationSearch.Application.Search;
using LocationSearch.Domain;
using Moq;
using NUnit.Framework;
using Repositories = LocationSearch.Infrastructure.Repositories;

namespace LocationSearch.Application.UnitTests.Search;

public class SearchLocationServiceTests
{
    private Mock<Repositories.ISearchLocationRepository> _mockSearchLocationRepository;
    private SearchLocationService _service;

    [SetUp]
    public void Setup()
    {
        _mockSearchLocationRepository = new Mock<Repositories.ISearchLocationRepository>();
        _service = new SearchLocationService(_mockSearchLocationRepository.Object);
    }

    [Test]
    public void Search_LocationIsIncorrect_ThrowsException()
    {
        Assert.ThrowsAsync<InvalidLocationException>(() => _service.Search(-91, 12, 12, 12));
    }

    [Test]
    public void Search_MaxDistanceLessThanZero_ThrowsException()
    {
        Assert.ThrowsAsync<ArgumentException>(() => _service.Search(56, 12, -1, 12));
    }

    [Test]
    public void Search_MaxResultsLessThanZero_ThrowsException()
    {
        Assert.ThrowsAsync<ArgumentException>(() => _service.Search(56, 12, 12, -1));
    }

    [Test]
    public async Task Search_CorrectInput_CallsRepositoryAndReturnsResults()
    {
        // Arrange
        var fakeResults = new Repositories.SearchLocationResult(TestDataHelper.GenerateAddresses(100)
            .Select(address =>
                new Repositories.SearchLocationResult.ResultItem(address.Location, address.FullAddress, 0))
            .ToImmutableList());
        var maxDistance = 1000;
        var maxResults = 10;

        _mockSearchLocationRepository
            .Setup(x => x.Search(It.IsAny<Location>(), maxDistance, maxResults))
            .Returns(Task.FromResult(fakeResults));

        // Act
        var results = await _service.Search(57, 32, maxDistance, maxResults);

        // Assert
        var expectedResults = new SearchLocationResult(fakeResults.Results.Select(fakeResult =>
                new SearchLocationResult.ResultItem(fakeResult.Location, fakeResult.Address, fakeResult.Distance))
            .ToImmutableList());

        CollectionAssert.AreEqual(expectedResults.Results, results.Results);
    }
}