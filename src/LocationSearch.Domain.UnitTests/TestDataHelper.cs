using System;

namespace LocationSearch.Domain.UnitTests;

public static class TestDataHelper
{
    private static readonly Random Rnd = new();
    
    public static Location GenerateRandomLocation()
    {
        return new Location(Rnd.Next(-900000000, 900000000) / 10000000d, Rnd.Next(-1800000000, 1800000000) / 10000000d);
    }
}