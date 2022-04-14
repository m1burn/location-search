using System.Collections.Immutable;
using System.Globalization;
using CsvHelper;
using LocationSearch.Core;
using LocationSearch.Domain;

namespace LocationSearch.Infrastructure.Repositories;

public class CsvAddressRepository : IAddressRepository
{
    private readonly IApplicationConfiguration _configuration;

    public CsvAddressRepository(IApplicationConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<ImmutableList<Address>> GetAll()
    {
        using var reader = new StreamReader(_configuration.LocationsCsvFilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        return Task.FromResult(csv.GetRecords<Row>()
            .Where(row => row.Latitude != null && row.Longitude != null)
            .Select(row => new Address(row.Address ?? string.Empty,
                new Location(row.Latitude!.Value, row.Longitude!.Value)))
            .ToImmutableList());
    }

    private class Row
    {
        public string? Address { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}