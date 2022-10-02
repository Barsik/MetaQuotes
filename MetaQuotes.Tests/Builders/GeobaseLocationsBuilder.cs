using MetaQuotes.Tests.Extensions;
using System.Text;
using MetaQuotes.Geobase;

namespace MetaQuotes.Tests.Builders;

public class GeobaseLocationsBuilder
{
    private byte[] _locations;
    public int Count { get; private set; }
    public byte[] Locations => _locations;

    private GeobaseLocationsBuilder()
    {
        _locations = Array.Empty<byte>();
    }

    public static GeobaseLocationsBuilder Create() => new();

    public GeobaseLocationsBuilder AddLocation(string city, float latitude, float longitude)
    {
        var locations = new byte[Count * GeobaseLocation.GeobaseLocationOffset + GeobaseLocation.GeobaseLocationOffset];

        Array.Copy(_locations, locations, _locations.Length);
        _locations = locations;

        var o = Offset.Create(Count * GeobaseLocation.GeobaseLocationOffset + GeobaseLocation.CountryOffset +
                              GeobaseLocation.RegionOffset + GeobaseLocation.PostalOffset);

        var cityBytes = Encoding.UTF8.GetBytes(city);
        
        cityBytes.Copy(_locations, o.Shift(GeobaseLocation.CityOffset));
        o.Shift(GeobaseLocation.OrganizationOffset);
        
        BitConverter.GetBytes(latitude)
            .Copy(_locations, o.Shift(GeobaseLocation.LatitudeOffset));
        BitConverter.GetBytes(longitude)
            .Copy(_locations, o.Shift(GeobaseLocation.LongitudeOffset));
        Count++;
        return this;
    }
}