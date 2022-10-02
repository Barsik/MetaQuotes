using System.Buffers;
using System.Text;
using MetaQuotes.Geobase.Extensions;

namespace MetaQuotes.Geobase;
public class GeobaseHeader
{
    public const int GeobaseHeaderOffset = 60;
    public const int VersionOffset = 4;
    public const int NameOffset = 32;
    public const int TimeStampOffset = 8;
    public const int RecordsOffset = 4;
    
    public const int OffsetRangesOffset = 4;
    public const int OffsetCitiesOffset = 4;
    public const int OffsetLocationsOffset = 4;

    public int Version { get; init; }
    public string Name { get; init; }
    public long TimeStamp { get; init; }
    public int Records { get; init; }
    public uint OffsetRanges { get; init; }
    public uint OffsetCities { get; init; }
    public uint OffsetLocations { get; init; }

    private GeobaseHeader()
    {
    }

    public static GeobaseHeader Create(ReadOnlySpan<byte> header)
    {
        var offset = Offset.Create();

        return new GeobaseHeader()
        {
            Version = BitConverter.ToInt32(header.Slice(offset.Shift(VersionOffset))),
            Name = Encoding.UTF8.GetString(header.Slice(offset.Shift(NameOffset))),
            TimeStamp = BitConverter.ToInt64(header.Slice(offset.Shift(TimeStampOffset))),
            Records = BitConverter.ToInt32(header.Slice(offset.Shift(RecordsOffset))),
            OffsetRanges = BitConverter.ToUInt32(header.Slice(offset.Shift(OffsetRangesOffset))),
            OffsetCities = BitConverter.ToUInt32(header.Slice(offset.Shift(OffsetCitiesOffset))),
            OffsetLocations = BitConverter.ToUInt32(header.Slice(offset.Shift(OffsetLocationsOffset))),
        };
    }
}