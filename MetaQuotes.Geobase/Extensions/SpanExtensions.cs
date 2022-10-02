using System.Buffers;

namespace MetaQuotes.Geobase.Extensions;

public static class SpanExtensions
{
    public static ReadOnlySpan<T> Slice<T>(this ReadOnlySpan<T> span, Offset.Slice slice)
    {
        return span.Slice(slice.Start, slice.Length);
    }
}