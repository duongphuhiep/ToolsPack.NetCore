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
    
    [Fact]
    public void NewShortGuidTest()
    {
        var g1 = ShortGuid.New();
        var g2 = ShortGuid.New();
        Assert.Equal(22, g1.Length);
        Assert.Equal(22, g2.Length);
        Assert.NotEqual(g1, g2);
    }
    [Fact]
    public void NewShortGuidUrlFriendlyTest()
    {
        var g1 = ShortGuid.New(true);
        var g2 = ShortGuid.New(true);
        Assert.Equal(22, g1.Length);
        Assert.Equal(22, g2.Length);
        Assert.NotEqual(g1, g2);
        Assert.False(g1.Contains('/'));
        Assert.False(g1.Contains('+'));
        Assert.False(g2.Contains('/'));
        Assert.False(g2.Contains('+'));
    }
}