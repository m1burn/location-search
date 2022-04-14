using System;
using System.Collections.Generic;
using System.Linq;
using LocationSearch.Domain;

namespace LocationSearch.Infrastructure.UnitTests;

public static class TestDataHelper
{
    private static readonly Random Rnd = new();

    public static IEnumerable<Address> GenerateAddresses(int count)
    {
        return Enumerable.Range(0, count).AsParallel().Select(num =>
        {
            var location = GenerateRandomLocation();
            return new Address($"Address {num}", location);
        });
    }

    public static Location GenerateRandomLocation()
    {
        return new Location(Rnd.Next(-900000000, 900000000) / 10000000d, Rnd.Next(-1800000000, 1800000000) / 10000000d);
    }
}