using MetaQuotes.Geobase.Models;

namespace MetaQuotes.Geobase.Contracts;

/// <summary>
/// Интерфейс предоставляющий основные функции Geobase
/// </summary>
public interface IGeobaseProvider
{
     /// <summary>
     /// Получить координаты по IP
     /// </summary>
     /// <param name="ipStr">ip в строковом формате</param>
     /// <returns>представление координат</returns>
     CoordinatesView? GetCoordinatesByIp(string ipStr);
     /// <summary>
     /// Получить локации по городу
     /// </summary>
     /// <param name="city">Город</param>
     /// <returns>Коллекция представлений локаций</returns>
     List<LocationView> GetLocationsByCity(string city);
}