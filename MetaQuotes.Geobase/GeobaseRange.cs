using System.Buffers;
using MetaQuotes.Geobase.Extensions;
using MetaQuotes.Geobase.Models;

namespace MetaQuotes.Geobase;

/// <summary>
/// Работа с интервалами IP
/// </summary>
public static class GeobaseRange
{
    // список офсетов интервалов IP
    public const int GeobaseRangeOffset = 12;
    public const int IpFromOffset = 4;
    public const int IpToOffset = 4;
    public const int LocationIndexOffset = 4;

    /// <summary>
    /// Создать представление интервала IP
    /// </summary>
    /// <param name="range">область интервала</param>
    /// <returns>представление интервала</returns>
    public static RangeView Get(ReadOnlySpan<byte> range)
    {
        var offset = Offset.Create();

        return new RangeView
        {
            IpFrom = BitConverter.ToUInt32(range.Slice(offset.Shift(IpFromOffset))),
            IpTo = BitConverter.ToUInt32(range.Slice(offset.Shift(IpToOffset))),
            LocationIndex = BitConverter.ToUInt32(range.Slice(offset.Shift(LocationIndexOffset)))
        };
    }
}