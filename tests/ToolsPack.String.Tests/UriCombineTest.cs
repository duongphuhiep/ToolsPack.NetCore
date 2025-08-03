using Xunit;

namespace ToolsPack.String.Tests;

public class UriCombineTest
{
    [Fact]
    public void JoinTest()
    {
        Assert.Equal("http://www.my.domain/relative/path",
            UriCombine.Join("http://www.my.domain/", "relative/path").ToString());
        Assert.Equal("http://www.my.domain/absolute/path",
            UriCombine.Join("http://www.my.domain/something/other", "/absolute/path").ToString());
    }
}