using System.Buffers;
using System.Net;
using MetaQuotes.Geobase.Contracts;
using MetaQuotes.Geobase.Models;

namespace MetaQuotes.Geobase;

public sealed class GeobaseProvider : IGeobaseProvider
{
    public GeobaseHeader Header { get; init; }
    private ReadOnlySequence<byte> _db;

    private GeobaseProvider()
    {
    }

    /// <summary>
    /// Бинарный поиск координат по IP
    /// </summary>
    /// <param name="ipStr">ip в формате строки</param>
    /// <returns></returns>
    public CoordinatesView? GetCoordinatesByIp(string ipStr)
    {
        if(!IPAddress.TryParse(ipStr, out var ipAddress)) return null;
        //конвертируем ip в uint
        var ip = BitConverter.ToUInt32(ipAddress.GetAddressBytes());
        //получаем область интервалов
        var ranges = _db.Slice(Header.OffsetRanges, GeobaseRange.GeobaseRangeOffset * Header.Records);

        //задаем начальный интервал для бинарного поиска
        var left = 0;
        var right = Header.Records - 1;

        while (left <= right)
        {
            //получаем центральную позицию
            var mid = (left + right) / 2;
            //получаем обоасть интервала для центральной позиции
            var range = GeobaseRange.Get(ranges.Slice(mid * GeobaseRange.GeobaseRangeOffset,
                GeobaseRange.GeobaseRangeOffset).FirstSpan);

            if (range.IpFrom <= ip && ip <= range.IpTo)
            {
                var location = GetLocationByIndex(range.LocationIndex);
                var view = GeobaseLocation.Get(location);
                return GeobaseLocation.GetCoordinates(location);
            }

            if (ip < range.IpFrom)
            {
                //правую границу смещаем влево от центра
                right = mid - 1;
            }
            else
            {
                //смещаем левую границу справа от центра
                left = mid + 1;
            }
        }

        return null;
    }

    /// <summary>
    /// Бинарный поиск по названию города
    /// </summary>
    /// <param name="city"></param>
    /// <returns>Коллекция локаций, с соответствующим городом</returns>
    public List<LocationView> GetLocationsByCity(string city)
    {
        var result = new List<LocationView>();
        //Если название города валидно, то можно продолжат поиск
        if (GeobaseLocation.TryValidateCity(city, out var cityBytes))
        {
            //Область индексов
            var indexes = _db.Slice(Header.OffsetCities, GeobaseIndex.GeobaseIndexOffset * Header.Records);
            //левая и правая начальные границы
            var left = 0;
            var right = Header.Records - 1;

            while (left <= right)
            {
                var mid = (left + right) / 2;
                //получаем офсет локации
                var locationOffset = GeobaseIndex.Get(indexes.Slice(mid * GeobaseIndex.GeobaseIndexOffset).FirstSpan);
                //область локации
                var location = GetLocationByOffset(locationOffset);
                var view = GeobaseLocation.Get(location);
                //сравнение с входным городом
                var compareResult = GeobaseLocation.CompareByCity(location, cityBytes);
                //если совпадение
                if (compareResult == 0)
                {
                    result.Add(GeobaseLocation.Get(location));
                    var currentPosition = mid;
                    //сдвигаемся влево
                    while (--currentPosition >= 0)
                    {
                        var currentOffset =
                            GeobaseIndex.Get(indexes.Slice(currentPosition * GeobaseIndex.GeobaseIndexOffset).FirstSpan);
                        var currentLocation = GetLocationByOffset(currentOffset);
                        var foobar = GeobaseLocation.Get(currentLocation);
                        if (GeobaseLocation.EqualsByCities(location, currentLocation))
                        {
                            result.Add(GeobaseLocation.Get(currentLocation));
                            continue;
                        }

                        break;
                    }

                    currentPosition = mid;
                    //сдвигаемся вправо
                    while (++currentPosition <= Header.Records - 1)
                    {
                        var currentOffset =
                            GeobaseIndex.Get(indexes.Slice(currentPosition * GeobaseIndex.GeobaseIndexOffset).FirstSpan);
                        var currentLocation = GetLocationByOffset(currentOffset);
                        var foobar = GeobaseLocation.Get(currentLocation);
                        if (GeobaseLocation.EqualsByCities(location, currentLocation))
                        {
                            result.Add(GeobaseLocation.Get(currentLocation));
                            continue;
                        }

                        break;
                    }

                    break;
                }
                //смещаем границу влево
                if (compareResult == -1)
                {
                    right = mid - 1;
                }
                //смещаем границу вправо
                else
                {
                    left = mid + 1;
                }
            }
        }

        return result;
    }
    

    /// <summary>
    /// Получить локацию с учетом её индекса
    /// </summary>
    /// <param name="index">Индекс локации</param>
    /// <returns>Span локации</returns>
    private ReadOnlySpan<byte> GetLocationByIndex(uint index)
    {
        var location = _db.Slice(Header.OffsetLocations + index * GeobaseLocation.GeobaseLocationOffset,
                GeobaseLocation.GeobaseLocationOffset).FirstSpan;

        return location;
    }

    /// <summary>
    /// Получить локацию с учетом офсета
    /// </summary>
    /// <param name="offset">Офсет относительно Header.OffsetLocations</param>
    /// <returns>Span локации</returns>
    private ReadOnlySpan<byte> GetLocationByOffset(uint offset)
    {
        var location = _db.Slice(Header.OffsetLocations + offset,
                GeobaseLocation.GeobaseLocationOffset).FirstSpan;

        return location;
    }

    public static GeobaseProvider Load(byte[] geobaseData)
    {
        return new GeobaseProvider()
        {
            Header = GeobaseHeader.Create(geobaseData),
            _db = new ReadOnlySequence<byte>(geobaseData)
        };
    }
}