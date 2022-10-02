namespace MetaQuotes.Geobase;

/// <summary>
/// Offset помогает последовательно смещаться по байтам
/// </summary>
public ref struct Offset
{
    private int _start;

    private Offset(int start)
    {
        _start = start;
    }
    /// <summary>
    /// Производим смещение
    /// </summary>
    /// <param name="offset">смещение</param>
    /// <returns>область</returns>
    public Slice Shift(int offset)
    {
        var start = _start;
        _start += offset;
        return new Slice(start, offset);
    }

    /// <summary>
    /// Определяет область
    /// </summary>
    /// <param name="Start">Начало</param>
    /// <param name="Length">Сколько</param>
    public record struct Slice(int Start, int Length);

    /// <summary>
    /// Создать смещение по умолчанию
    /// </summary>
    /// <param name="start"></param>
    /// <returns></returns>
    public static  Offset Create(int start=default) => new(start);
}