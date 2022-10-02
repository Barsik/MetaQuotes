using MetaQuotes.Tests.Extensions;
using System.Text;
using MetaQuotes.Geobase;

namespace MetaQuotes.Tests.Builders;

public class GeobaseHeaderBuilder
{
    private byte[] _header;
    public int Records { get; private set; }

    public byte[] Header => _header;
    private GeobaseHeaderBuilder(byte[]? header = null)
    {
        _header = header ?? new byte[GeobaseHeader.GeobaseHeaderOffset];
    }

    public static GeobaseHeaderBuilder Create(byte[]? header=null) => new(header);

    public GeobaseHeaderBuilder SetHeaderData(int version, string name, long timeStamp)
    {
        var offset = Offset.Create();
        BitConverter.GetBytes(version).Copy(_header, offset.Shift(GeobaseHeader.VersionOffset));
        Encoding.UTF8.GetBytes(name).Copy(_header, offset.Shift(GeobaseHeader.NameOffset));
        BitConverter.GetBytes(timeStamp).Copy(_header, offset.Shift(GeobaseHeader.TimeStampOffset));

        return this;
    }

    public GeobaseHeaderBuilder SetRecords(int records)
    {
        Records = records;
        
        var offset =
            Offset.Create(GeobaseHeader.VersionOffset + GeobaseHeader.NameOffset + GeobaseHeader.TimeStampOffset);
        BitConverter.GetBytes(records).Copy(_header, offset.Shift(GeobaseHeader.RecordsOffset));

        BitConverter.GetBytes(GeobaseHeader.GeobaseHeaderOffset)
            .Copy(_header, offset.Shift(GeobaseHeader.OffsetRangesOffset));

        BitConverter.GetBytes(GeobaseHeader.GeobaseHeaderOffset + GeobaseRange.GeobaseRangeOffset * records +
                              GeobaseLocation.GeobaseLocationOffset * records)
            .Copy(_header, offset.Shift(GeobaseHeader.OffsetCitiesOffset));

        BitConverter.GetBytes(GeobaseHeader.GeobaseHeaderOffset + GeobaseRange.GeobaseRangeOffset * records)
            .Copy(_header, offset.Shift(GeobaseHeader.OffsetLocationsOffset));

        return this;
    }
}