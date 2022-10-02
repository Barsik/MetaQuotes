using MetaQuotes.Tests.Extensions;
using System.Text;
using MetaQuotes.Geobase;

namespace MetaQuotes.Tests.Builders;

public class GeobaseRangesBuilder
{
    private byte[] _ranges;
    public int Count { get; private set; }
    public byte[] Ranges => _ranges;

    private GeobaseRangesBuilder()
    {
        _ranges = new byte[0];
    }

    public static GeobaseRangesBuilder Create() => new();

    public GeobaseRangesBuilder AddRange(uint ipFrom, uint ipTo, uint locationIndex)
    {
        var ranges = new byte[Count * GeobaseRange.GeobaseRangeOffset + GeobaseRange.GeobaseRangeOffset];
        
        Array.Copy(_ranges, ranges, _ranges.Length);
        _ranges = ranges;
        
        var o = Offset.Create(Count * GeobaseRange.GeobaseRangeOffset);
        
        BitConverter.GetBytes(ipFrom).Copy(_ranges, o.Shift(GeobaseRange.IpFromOffset));
        BitConverter.GetBytes(ipTo).Copy(_ranges, o.Shift(GeobaseRange.IpToOffset));
        BitConverter.GetBytes(locationIndex).Copy(_ranges, o.Shift(GeobaseRange.LocationIndexOffset));
        Count++;
        return this;
    }
}