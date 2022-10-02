using MetaQuotes.Geobase;
using MetaQuotes.Tests.Builders;
using MetaQuotes.Tests.Extensions;

namespace MetaQuotes.Tests;

public class GeobaseBuilder
{
    private readonly GeobaseHeaderBuilder _geobaseHeaderBuilder;
    private readonly GeobaseRangesBuilder _geobaseRangesBuilder;
    private readonly GeobaseLocationsBuilder _geobaseLocationsBuilder;
    private readonly GeobaseIndexesBuilder _geobaseIndexesBuilder;

    private GeobaseBuilder()
    {
        _geobaseHeaderBuilder = GeobaseHeaderBuilder.Create();
        _geobaseRangesBuilder = GeobaseRangesBuilder.Create();
        _geobaseLocationsBuilder = GeobaseLocationsBuilder.Create();
        _geobaseIndexesBuilder = GeobaseIndexesBuilder.Create();
    }

    public static GeobaseBuilder Create() => new();

    public GeobaseBuilder ConfigureHeader(Action<GeobaseHeaderBuilder> options)
    {
        options.Invoke(_geobaseHeaderBuilder);
        return this;
    }

    public GeobaseBuilder ConfigureRanges(Action<GeobaseRangesBuilder> options)
    {
        options.Invoke(_geobaseRangesBuilder);
        return this;
    }

    public GeobaseBuilder ConfigureLocations(Action<GeobaseLocationsBuilder> options)
    {
        options.Invoke(_geobaseLocationsBuilder);
        return this;
    }

    public GeobaseBuilder ConfigureIndexes(Action<GeobaseIndexesBuilder> options)
    {
        options.Invoke(_geobaseIndexesBuilder);
        return this;
    }

    public GeobaseProvider Build()
    {
        var db = new byte[GeobaseHeader.GeobaseHeaderOffset +
                          _geobaseRangesBuilder.Count * GeobaseRange.GeobaseRangeOffset +
                          _geobaseLocationsBuilder.Count * GeobaseLocation.GeobaseLocationOffset +
                          _geobaseIndexesBuilder.Count * GeobaseIndex.GeobaseIndexOffset];
        var o = Offset.Create();
        _geobaseHeaderBuilder.Header.Copy(db, o.Shift(GeobaseHeader.GeobaseHeaderOffset));
        _geobaseRangesBuilder.Ranges.Copy(db, o.Shift(_geobaseRangesBuilder.Count * GeobaseRange.GeobaseRangeOffset));
        _geobaseLocationsBuilder.Locations.Copy(db,
            o.Shift(_geobaseLocationsBuilder.Count * GeobaseLocation.GeobaseLocationOffset));

        _geobaseIndexesBuilder.Indexes.Copy(db,
            o.Shift(_geobaseIndexesBuilder.Count * GeobaseIndex.GeobaseIndexOffset));
        return GeobaseProvider.Load(db);
    }
}