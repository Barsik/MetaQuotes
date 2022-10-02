using System.Text;
using MetaQuotes.Geobase;

namespace MetaQuotes.Tests.Builders;

public class GeobaseLocationBuilder
{
    private byte[] _data = new byte[GeobaseLocation.GeobaseLocationOffset];

    private GeobaseLocationBuilder()
    {
    }

    public static GeobaseLocationBuilder Create() => new();

    public GeobaseLocationBuilder SetCity(string city )
    {
        var str = Encoding.UTF8.GetBytes(city);
        Array.Copy(str, 0, _data,
            GeobaseLocation.CountryOffset + GeobaseLocation.RegionOffset + GeobaseLocation.PostalOffset, str.Length);
        return this;
    }

    public Span<byte> AsSpan()
    {
        return _data.AsSpan();
    }
}
