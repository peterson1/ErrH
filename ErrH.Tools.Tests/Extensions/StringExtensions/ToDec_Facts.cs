using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{
    public class ToDec_Facts
    {

        [Theory]
        [InlineData("123.456", 123.456)]
        [InlineData("123", 123.0)]
        [InlineData("1,234.5", 1234.5)]
        [InlineData("  1,234.50 ", 1234.5)]
        [InlineData("1,234,567", 1234567)]
        public void Case(string sut, decimal expctd)
        {
            Assert.Equal(expctd, sut.ToDec());
        }
    }
}
