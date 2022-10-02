using System.Text;
using MetaQuotes.Geobase;

namespace MetaQuotes.Tests;

public class GeobaseHeaderTests
{
    [Fact]
    public void GeobaseHeader_SuccessCreating()
    {
        var version = 100;
        var name = "test";
        long timeStamp = 9999;
        var records = 10;
        var builder = GeobaseBuilder.Create();
        builder.ConfigureHeader(o => o.SetHeaderData(version, name, timeStamp).SetRecords(records));
        var provider = builder.Build();

        Assert.Equal(version, provider.Header.Version);
        Assert.Equal(name, provider.Header.Name.TrimEnd((char)0));
        Assert.Equal(timeStamp, provider.Header.TimeStamp);
        Assert.Equal(records, provider.Header.Records);

        Assert.Equal((long)GeobaseHeader.GeobaseHeaderOffset, provider.Header.OffsetRanges);
        Assert.Equal((long)GeobaseHeader.GeobaseHeaderOffset + records * GeobaseRange.GeobaseRangeOffset,
            provider.Header.OffsetLocations);

        Assert.Equal(
            (long)GeobaseHeader.GeobaseHeaderOffset + records * GeobaseRange.GeobaseRangeOffset +
            records * GeobaseLocation.GeobaseLocationOffset, provider.Header.OffsetCities);
    }
}