using System.Net;
using MetaQuotes.Geobase;

namespace MetaQuotes.Tests;

public class GeobaseProviderTests
{
    [Fact]
    public void GetCoordinatesByIp_WhenFound()
    {
        var ipStr = "1.1.1.1";

        var city1Latitude = (float)10.2;
        var city1Longitude = (float)11.3;

        var provider = StubGeobaseProvider.Create();

        var coordinates = provider.GetCoordinatesByIp(ipStr);

        Assert.NotNull(coordinates);
        Assert.Equal(city1Latitude, coordinates.Value.Latitude);
        Assert.Equal(city1Longitude, coordinates.Value.Longitude);
    }

    [Fact]
    public void GetCoordinatesByIp_WhenNotFound()
    {
        var ipStr = "10.1.1.2";
        var provider = StubGeobaseProvider.Create();
        var coordinates = provider.GetCoordinatesByIp(ipStr);
        Assert.Null(coordinates);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("null")]
    [InlineData("")]
    public void GetCoordinatesByIp_WhenIncorrectIp(string ipStr)
    {
        var provider = StubGeobaseProvider.Create();
        var coordinates = provider.GetCoordinatesByIp(ipStr);
        Assert.Null(coordinates);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("ab")]
    public void GetLocationsByCity_WhenFoundOne(string city)
    {
        var provider = StubGeobaseProvider.Create();
        var locations = provider.GetLocationsByCity(city);
        
        Assert.Single(locations);
        Assert.Equal(city,locations.First().City);
    }
    
    [Fact]
    public void GetLocationsByCity_WhenFoundMultiple()
    {
        var city = "abc";
        var provider = StubGeobaseProvider.Create();
        var locations = provider.GetLocationsByCity(city);
        
        Assert.Equal(2,locations.Count);
        Assert.All(locations, l => { Assert.Equal(city,l.City);});
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("qqwerty")]
    public void GetLocationsByCity_WhenNotFound(string city)
    {
        var provider = StubGeobaseProvider.Create();
        var locations = provider.GetLocationsByCity(city);
        
        Assert.Empty(locations);
    }
}