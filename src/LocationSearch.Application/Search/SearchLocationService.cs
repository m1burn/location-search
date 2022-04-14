using System.Collections.Immutable;
using LocationSearch.Core;
using LocationSearch.Domain;
using LocationSearch.Infrastructure.Repositories;

namespace LocationSearch.Application.Search;

public class SearchLocationService : ISearchLocationWithinDistance
{
    private readonly ISearchLocationRepository _searchLocationRepository;

    public SearchLocationService(ISearchLocationRepository searchLocationRepository)
    {
        _searchLocationRepository = searchLocationRepository;
    }

    public async Task<SearchLocationResult> Search(double latitude, double longitude, int maxDistance, int maxResults)
    {
        Guard.NotNegative(maxDistance, nameof(maxDistance));
        Guard.NotNegative(maxResults, nameof(maxResults));

        var location = new Location(latitude, longitude);
        if (!location.IsValid())
        {
            throw new InvalidLocationException(location);
        }

        var locationsWithinBoundaries = await _searchLocationRepository.Search(location, maxDistance, maxResults);
        var results = locationsWithinBoundaries.Results
            .Select(result => new SearchLocationResult.ResultItem(result.Location, result.Address, result.Distance))
            .ToImmutableList();
        return new SearchLocationResult(results);
    }
}