using MetaQuotes.Geobase;
using MetaQuotes.Tests.Builders;

namespace MetaQuotes.Tests;

public class GeobaseLocationTests
{
    [Fact]
    public void EqualsByCities_WhenSuccess()
    {
        var builder1 = GeobaseLocationBuilder.Create().SetCity("qwe");
        var builder2 = GeobaseLocationBuilder.Create().SetCity("qwe");

        Assert.True(GeobaseLocation.EqualsByCities(builder1.AsSpan(), builder2.AsSpan()));
    }

    [Fact]
    public void EqualsByCities_WhenDifferenceInCapital()
    {
        var builder1 = GeobaseLocationBuilder.Create().SetCity("qwe");
        var builder2 = GeobaseLocationBuilder.Create().SetCity("qWe");

        Assert.False(GeobaseLocation.EqualsByCities(builder1.AsSpan(), builder2.AsSpan()));
    }

    [Fact]
    public void EqualsByCities_WhenDifferenceInSpace()
    {
        var builder1 = GeobaseLocationBuilder.Create().SetCity("qwe");
        var builder2 = GeobaseLocationBuilder.Create().SetCity("qwe ");

        Assert.False(GeobaseLocation.EqualsByCities(builder1.AsSpan(), builder2.AsSpan()));
    }
    
    
    [Fact]
    public void TryValidateCity_WhenValid()
    {
        Assert.True(GeobaseLocation.TryValidateCity("qwerty", out var _));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("asdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiop")]
    public void TryValidateCity_WhenInValid(string city)
    {
        Assert.False(GeobaseLocation.TryValidateCity(city, out var _));
    }
    
}