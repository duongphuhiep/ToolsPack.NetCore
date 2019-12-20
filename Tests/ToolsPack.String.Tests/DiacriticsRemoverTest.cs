using System;
using Xunit;

namespace ToolsPack.String.Tests
{
    public class DiacriticsRemoverTest
    {
        [Fact]
        public void RemoveTest()
        {
            Assert.Equal("Leo lacreve", "Léo lacrève".RemoveDiacritics());
        }
    }
}
