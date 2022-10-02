namespace MetaQuotes.Geobase;

/// <summary>
/// Работа с индексами
/// </summary>
public static class GeobaseIndex
{
    // список офсетов индекса
    public const int GeobaseIndexOffset = 4;
    public static uint Get(ReadOnlySpan<byte> index) => BitConverter.ToUInt32(index);
}