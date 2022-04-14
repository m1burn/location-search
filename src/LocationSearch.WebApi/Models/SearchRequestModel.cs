namespace LocationSearch.WebApi.Models;

public record SearchRequestModel(double Latitude, double Longitude, int MaxDistance, int MaxResults);