using Xunit;

namespace ToolsPack.String.Tests
{
    public class StringExtTest
    {
        [Fact]
        public void JoinNonEmptyTest()
        {
            Assert.Equal("a, c , b", StringExt.JoinNonEmpty(", ", "a", null, "c ", string.Empty, "b"));
            Assert.Equal("a,c ,b", StringExt.JoinNonEmpty(',', new[] { "a", null, string.Empty, "c ", "b" }));
        }
    }
}
