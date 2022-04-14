namespace LocationSearch.Domain;

/// <summary>
/// Location boundaries
/// </summary>
/// <param name="MinLatitude">The upper boundary latitude point in decimal notation</param>
/// <param name="MaxLatitude">The lower boundary latitude point in decimal notation</param>
/// <param name="MinLongitude">The left boundary longitude point in decimal notation</param>
/// <param name="MaxLongitude">The right boundary longitude point in decimal notation</param>
public record LocationBoundaries(double MinLatitude, double MaxLatitude, double MinLongitude, double MaxLongitude);