namespace LocationSearch.Domain;

/// <summary>
/// Address entity
/// </summary>
public class Address
{
    /// <summary>
    /// Full text address, contains Street Name, House Number, City, Country, etc.
    /// Example: AH Frieswijkstraat 72, Nijkerk
    /// </summary>
    public string FullAddress { get; set; }

    /// <summary>
    /// Address location
    /// </summary>
    public Location Location { get; set; }

    public Address(string fullAddress, Location location)
    {
        FullAddress = fullAddress;
        Location = location;
    }
}