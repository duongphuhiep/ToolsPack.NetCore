using Xunit;

namespace ToolsPack.String.Tests;

public class StringGeneratorTest
{
    [Fact]
    public void CreateRandomStringTest()
    {
        var s = StringGenerator.CreateRandomString();
        Assert.Equal(5, s.Length);

        var s2 = StringGenerator.CreateRandomString(5, 10);
    }
}