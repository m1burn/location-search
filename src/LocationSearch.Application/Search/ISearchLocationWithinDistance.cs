namespace LocationSearch.Application.Search;

public interface ISearchLocationWithinDistance
{
    Task<SearchLocationResult> Search(double latitude, double longitude, int maxDistance, int maxResults);
}