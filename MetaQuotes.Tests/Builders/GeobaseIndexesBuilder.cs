using MetaQuotes.Tests.Extensions;
using System.Text;
using MetaQuotes.Geobase;

namespace MetaQuotes.Tests.Builders;

public class GeobaseIndexesBuilder
{
    private byte[] _indexes;
    public int Count { get; private set; }
    public byte[] Indexes => _indexes;

    private GeobaseIndexesBuilder()
    {
        _indexes = Array.Empty<byte>();
    }

    public static GeobaseIndexesBuilder Create() => new();

    public GeobaseIndexesBuilder AddIndex(int index)
    {
        var indexes = new byte[Count * GeobaseIndex.GeobaseIndexOffset + GeobaseIndex.GeobaseIndexOffset ];

        Array.Copy(_indexes, indexes, _indexes.Length);
        _indexes = indexes;

        var o = Offset.Create(Count * GeobaseIndex.GeobaseIndexOffset);

        BitConverter.GetBytes(index * GeobaseLocation.GeobaseLocationOffset)
            .Copy(_indexes, o.Shift(GeobaseIndex.GeobaseIndexOffset));
        Count++;
        return this;
    }
}