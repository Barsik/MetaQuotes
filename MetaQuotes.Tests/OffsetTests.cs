using MetaQuotes.Geobase;

namespace MetaQuotes.Tests;

public class OffsetTests
{
    [Fact]
    public void Offset_WhenSequenceOfShifts()
    {
        var o = Offset.Create();
        Assert.Equal(new Offset.Slice(0, 10), o.Shift(10));
        Assert.Equal(new Offset.Slice(10, 20), o.Shift(20));
        Assert.Equal(new Offset.Slice(30, 20), o.Shift(20));
    }
}