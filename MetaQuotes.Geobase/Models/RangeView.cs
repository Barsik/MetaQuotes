namespace MetaQuotes.Geobase.Models;

public record struct RangeView
{
    public uint IpFrom { get; init; }
    public uint IpTo { get; init; }
    public uint LocationIndex { get; init; }
}