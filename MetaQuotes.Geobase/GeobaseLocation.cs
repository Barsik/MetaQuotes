using System.Text;
using MetaQuotes.Geobase.Extensions;
using MetaQuotes.Geobase.Models;

namespace MetaQuotes.Geobase;

/// <summary>
/// Работа с локациями
/// </summary>
public static class GeobaseLocation
{
    // список офсетов локации
    public const int GeobaseLocationOffset = 96;
    public const int CountryOffset = 8;
    public const int RegionOffset = 12;
    public const int PostalOffset = 12;
    public const int CityOffset = 24;
    public const int OrganizationOffset = 32;
    public const int LatitudeOffset = 4;
    public const int LongitudeOffset = 4;

    /// <summary>
    /// Создаем представление для локации
    /// </summary>
    /// <param name="location">область локации</param>
    /// <returns>Представление локации</returns>
    public static LocationView Get(ReadOnlySpan<byte> location)
    {
        var offset = Offset.Create();

        return new LocationView()
        {
            Country = Encoding.UTF8.GetString(location.Slice(offset.Shift(CountryOffset)).TrimEnd(byte.MinValue)),
            Region = Encoding.UTF8.GetString(location.Slice(offset.Shift(RegionOffset)).TrimEnd(byte.MinValue)),
            Postal = Encoding.UTF8.GetString(location.Slice(offset.Shift(PostalOffset)).TrimEnd(byte.MinValue)),
            City = Encoding.UTF8.GetString(location.Slice(offset.Shift(CityOffset)).TrimEnd(byte.MinValue)),
            Organization =
                Encoding.UTF8.GetString(location.Slice(offset.Shift(OrganizationOffset)).TrimEnd(byte.MinValue)),
            Latitude = BitConverter.ToSingle(location.Slice(offset.Shift(LatitudeOffset))),
            Longitude = BitConverter.ToSingle(location.Slice(offset.Shift(LongitudeOffset)))
        };
    }

    /// <summary>
    /// Получить представление координаты локации
    /// </summary>
    /// <param name="location">Область байт локации</param>
    /// <returns>Координаты</returns>
    public static CoordinatesView GetCoordinates(ReadOnlySpan<byte> location)
    {
        //сдвигаемся до области координат
        var offset = Offset.Create(GeobaseLocationOffset - LatitudeOffset - LongitudeOffset);

        return new CoordinatesView(
            BitConverter.ToSingle(location.Slice(offset.Shift(LatitudeOffset))),
            BitConverter.ToSingle(location.Slice(offset.Shift(LongitudeOffset))));
    }

    /// <summary>
    /// Валидируем входной город
    /// </summary>
    /// <param name="city">Входной город</param>
    /// <param name="cityBytes">Если город валидный, то возвращаем его байты</param>
    /// <returns>Валиден город или нет</returns>
    public static bool TryValidateCity(string? city, out byte[] cityBytes)
    {
        if (city == null)
        {
            cityBytes = Array.Empty<byte>();
            return false;
        }

        cityBytes = Encoding.UTF8.GetBytes(city);
        //если входной город имеет больше байт, чем офсет в базе, то город заведомо невалиден
        return cityBytes.Length <= CityOffset;
    }

    /// <summary>
    /// Сравнить город локации с входным городом (в байтах)  
    /// </summary>
    /// <param name="location">span байтов локации</param>
    /// <param name="cityBytes">байты города, с которым сравниваем</param>
    /// <returns>
    /// -1 - входной город больше;
    ///  0 - равны
    /// +1 - входной город меньше
    /// </returns>
    public static int CompareByCity(ReadOnlySpan<byte> location, byte[] cityBytes)
    {
        //сдвигаемся до области с городом
        var offset = Offset.Create(CountryOffset + RegionOffset + PostalOffset);
        //убираем нулевые байты с конца города
        var locationCity = location.Slice(offset.Shift(CityOffset)).TrimEnd(byte.MinValue);
        // если размеры отличаются, то где байтов больше тот и больше

        // сравниваем последовательно все байты
        for (var i = 0; i < Math.Min(locationCity.Length, cityBytes.Length); i++)
        {
            if (locationCity[i] != cityBytes[i])
            {
                //чей байт больше, тем название города меньше
                return locationCity[i] > cityBytes[i] ? -1 : 1;
            }
        }
        
        if (locationCity.Length != cityBytes.Length)
        {
            //чей длина названия больше другого, тем название меньше 
            return locationCity.Length > cityBytes.Length ? -1 : 1;
        }

        //равны
        return 0;
    }

    /// <summary>
    /// Сравниваем города двух локаций
    /// </summary>
    /// <param name="location1">Первая локация</param>
    /// <param name="location2">Вторая локация</param>
    /// <returns>Равны или нет</returns>
    public static bool EqualsByCities(ReadOnlySpan<byte> location1, ReadOnlySpan<byte> location2)
    {
        //офсеты для двух локаций
        var offset1 = Offset.Create(CountryOffset + RegionOffset + PostalOffset);
        var offset2 = Offset.Create(CountryOffset + RegionOffset + PostalOffset);
        //города двух локаций
        var location1City = location1.Slice(offset1.Shift(CityOffset)).TrimEnd(byte.MinValue);
        var location2City = location2.Slice(offset2.Shift(CityOffset)).TrimEnd(byte.MinValue);
        return location1City.SequenceEqual(location2City);
    }
}