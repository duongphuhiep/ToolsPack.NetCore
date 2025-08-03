using Xunit;

namespace ToolsPack.String.Tests;

public class ArrayDisplayerTest
{
    [Fact]
    public void ArrayFormatTest()
    {
        var arr = new[] { "01234", "11234", "212", "3123" };
        var s = arr.Display().MaxItems(3).MaxItemLength(3).ToString();
        Assert.NotEmpty(s);
    }
}