using System.Collections.Immutable;
using LocationSearch.Core;
using LocationSearch.Infrastructure.Cache;
using NetTopologySuite.Geometries;
using Location = LocationSearch.Domain.Location;

namespace LocationSearch.Infrastructure.Repositories;

public class CacheSearchLocationRepository : ISearchLocationRepository
{
    private readonly IAddressCache _addressCache;

    public CacheSearchLocationRepository(IAddressCache addressCache)
    {
        _addressCache = addressCache;
    }

    public async Task<SearchLocationResult> Search(Location location, int maxDistance, int maxResults)
    {
        Guard.NotNull(location, nameof(location));
        Guard.NotNegative(maxDistance, nameof(maxDistance));
        Guard.NotNegative(maxResults, nameof(maxResults));

        var boundaries = location.CalculateBoundaries(maxDistance);
        var envelope = new Envelope(boundaries.MinLongitude, boundaries.MaxLongitude, boundaries.MinLatitude,
            boundaries.MaxLatitude);
        var cache = await _addressCache.Get();
        var results = cache.Query(envelope)
            .Select(node =>
            {
                var distance = location.CalculateDistance(node.Data.Location);
                return new SearchLocationResult.ResultItem(node.Data.Location, node.Data.FullAddress, distance);
            })
            .Where(resultItem => resultItem.Distance <= maxDistance)
            .OrderBy(resultItem => resultItem.Distance)
            .Take(maxResults)
            .ToImmutableList();
        return new SearchLocationResult(results);
    }
}