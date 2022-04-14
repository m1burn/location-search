using LocationSearch.Domain;

namespace LocationSearch.Infrastructure.Repositories;

public interface ISearchLocationRepository
{
    public Task<SearchLocationResult> Search(Location location, int maxDistance, int maxResults);
}