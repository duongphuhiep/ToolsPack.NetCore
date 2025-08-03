using System;
using Xunit;

namespace ToolsPack.String.Tests;

public class ShortGuidTest
{
    [Fact]
    public void ConvertShortGuidTest()
    {
        var g = Guid.NewGuid();

        var shortGuid = g.ToShortGuid();
        Assert.Equal(g, ShortGuid.Parse(shortGuid));

        var shortGuidUrlFriendly = g.ToShortGuid(true);
        Assert.Equal(g, ShortGuid.Parse(shortGuidUrlFriendly, true));
    }
}