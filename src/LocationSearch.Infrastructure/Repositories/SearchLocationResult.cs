using System.Collections.Immutable;
using LocationSearch.Domain;

namespace LocationSearch.Infrastructure.Repositories;

public record SearchLocationResult(ImmutableList<SearchLocationResult.ResultItem> Results)
{
    public record ResultItem(Location Location, string Address, double Distance);
}