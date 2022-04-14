using LocationSearch.Domain;
using NetTopologySuite.Index.KdTree;

namespace LocationSearch.Infrastructure.Cache;

public interface IAddressCache
{
    Task Set(IEnumerable<Address> addresses);

    Task<KdTree<Address>> Get();
}