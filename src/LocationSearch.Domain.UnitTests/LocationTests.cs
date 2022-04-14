using System;
using NUnit.Framework;

namespace LocationSearch.Domain.UnitTests;

[TestFixture]
public class LocationTests
{
    private Location _location;

    [SetUp]
    public void SetUp()
    {
        _location = TestDataHelper.GenerateRandomLocation();
    }

    [Test]
    public void CalculateBoundaries_DistanceIsLessThanZero_ThrowsException()
    {
        // Assert
        Assert.Throws<ArgumentException>(() => _location.CalculateBoundaries(-1));
    }
    
    [Test]
    public void CalculateBoundaries_LocationIsInvalid_ThrowsException()
    {
        // Arrange
        _location = new Location(-91, 100);
        
        // Assert
        Assert.Throws<InvalidLocationException>(() => _location.CalculateBoundaries(100));
    }

    [Test]
    public void CalculateBoundaries_CorrectInput_ReturnsCorrectBoundaries()
    {
        // Arrange
        var distance = 40233d;
        var expectedMinLatitude = 33.705279224017289d;
        var expectedMaxLatitude = 34.429904375982716d;
        var expectedMinLongitude = -118.8350853642583d;
        var expectedMaxLongitude = -117.9603328357417d;

        _location = new Location(34.0675918, -118.3977091);

        // Act
        var boundaries = _location.CalculateBoundaries(distance);

        // Assert
        Assert.AreEqual(expectedMinLatitude, boundaries.MinLatitude);
        Assert.AreEqual(expectedMaxLatitude, boundaries.MaxLatitude);
        Assert.AreEqual(expectedMinLongitude, boundaries.MinLongitude);
        Assert.AreEqual(expectedMaxLongitude, boundaries.MaxLongitude);
    }
}