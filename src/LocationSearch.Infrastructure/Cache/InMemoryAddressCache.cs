using LocationSearch.Core;
using LocationSearch.Domain;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index.KdTree;

namespace LocationSearch.Infrastructure.Cache;

public class InMemoryAddressCache : IAddressCache
{
    private readonly object _lockObject = new();

    private KdTree<Address>? _cache;

    public Task Set(IEnumerable<Address> addresses)
    {
        Guard.NotNull(addresses, nameof(addresses));

        lock (_lockObject)
        {
            var cache = new KdTree<Address>(-1);

            foreach (var address in addresses)
            {
                cache.Insert(new Coordinate(address.Location.Longitude, address.Location.Latitude), address);
            }

            _cache = cache;
        }

        return Task.CompletedTask;
    }

    public Task<KdTree<Address>> Get()
    {
        if (_cache != null)
        {
            return Task.FromResult(_cache);
        }

        throw new NotInitializedException(
            $"Initialize {nameof(InMemoryAddressCache)} via {nameof(Set)} method before use");
    }
}