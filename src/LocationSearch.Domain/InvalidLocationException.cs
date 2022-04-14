namespace LocationSearch.Domain;

public class InvalidLocationException : Exception
{
    public InvalidLocationException(Location location)
        : base($"Location is invalid. Latitude: {location.Latitude}; Longitude: {location.Longitude}")
    {
    }
}