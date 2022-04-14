using LocationSearch.Core;

namespace LocationSearch.Domain;

/// <summary>
/// Location, i.e. latitude and longitude
/// </summary>
/// <param name="Latitude">Latitude</param>
/// <param name="Longitude">Longitude</param>
public record Location(double Latitude, double Longitude)
{
    /// <summary>
    /// Calculates the upper, lower, left and right location boundaries based on a distance, in meters
    /// </summary>
    public LocationBoundaries CalculateBoundaries(double distance)
    {
        Guard.NotNegative(distance, nameof(distance));

        if (!IsValid())
        {
            throw new InvalidLocationException(this);
        }

        const double divisor = 111045;

        var latitudeConversionFactor = distance / divisor;
        var longitudeConversionFactor = distance / divisor / Math.Abs(Math.Cos(Latitude * (Math.PI / 180)));

        var minLatitude = Latitude - latitudeConversionFactor;
        var maxLatitude = Latitude + latitudeConversionFactor;
        var minLongitude = Longitude - longitudeConversionFactor;
        var maxLongitude = Longitude + longitudeConversionFactor;

        // Adjust for passing over coordinate boundaries
        if (minLatitude < -90) minLatitude = 90 - (-90 - minLatitude);
        if (maxLatitude > 90) maxLatitude = -90 + (maxLatitude - 90);

        if (minLongitude < -180) minLongitude = 180 - (-180 - minLongitude);
        if (maxLongitude > 180) maxLongitude = -180 + (maxLongitude - 180);

        return new LocationBoundaries(minLatitude, maxLatitude, minLongitude, maxLongitude);
    }

    /// <summary>
    /// Calculates the distance between this location and another one, in meters.
    /// </summary>
    public double CalculateDistance(Location location)
    {
        var rlat1 = Math.PI * Latitude / 180;
        var rlat2 = Math.PI * location.Latitude / 180;
        var rlon1 = Math.PI * Longitude / 180;
        var rlon2 = Math.PI * location.Longitude / 180;
        var theta = Longitude - location.Longitude;
        var rtheta = Math.PI * theta / 180;
        var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
        dist = Math.Acos(dist);
        dist = dist * 180 / Math.PI;
        dist = dist * 60 * 1.1515;

        return dist * 1609.344;
    }

    public bool IsValid()
    {
        if (Latitude < -90 || Latitude > 90) return false;
        if (Longitude < -180 || Longitude > 180) return false;

        return true;
    }

    public override string ToString()
    {
        return Latitude + ", " + Longitude;
    }
}