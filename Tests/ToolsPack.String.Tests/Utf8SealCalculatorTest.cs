using System;
using Xunit;

namespace ToolsPack.String.Tests
{
    public class Utf8SealCalculatorTest
    {
        [Fact]
        public void SHA1ManagedTest()
        {
            Assert.Equal("d77610c9eb0b966442caf33e244c1b3512f3f020",
                Utf8SealCalculator.SHA1Managed("INTERACTIVE+1250+0+TEST+978+PAYMENT+SINGLE+76041374+20200405211920+h00001+0+V2+9081725181838840", Utf8SealCalculator.ToHex));
        }
    }
}
