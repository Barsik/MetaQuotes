using System.Net;
using MetaQuotes.Geobase;

namespace MetaQuotes.Tests;

public static class StubGeobaseProvider
{
    public static GeobaseProvider Create()
    {
        var ipStr = "1.1.1.1";
        var ipAddress = IPAddress.Parse(ipStr);
        var ip = BitConverter.ToUInt32(ipAddress.GetAddressBytes());

        var city1Latitude = (float)10.2;
        var city1Longitude = (float)11.3;
        var builder = GeobaseBuilder.Create();
        builder
            .ConfigureHeader(o => o.SetRecords(4))
            .ConfigureRanges(o => o
                .AddRange(ip - 10, ip + 10, 0)
                .AddRange(ip + 21, ip + 40, 1)
                .AddRange(ip + 41, ip + 60, 1)
                .AddRange(ip + 61, ip + 70, 1))
            .ConfigureLocations(o => o
                .AddLocation("abc", city1Latitude, city1Longitude)
                .AddLocation("abc", (float)3330.2, (float)4431.3)
                .AddLocation("ab", (float)330.2, (float)431.3)
                .AddLocation("a", (float)30.2, (float)41.3))
            .ConfigureIndexes(o=>o
                .AddIndex(3)
                .AddIndex(2)
                .AddIndex(1)
                .AddIndex(0));

        return builder.Build();
    }
}