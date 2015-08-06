using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.IntegerExtensions
{
    public class WithComma_Facts
    {
        [Theory(DisplayName = "Integer with comma")]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(2000, "2,000")]
        [InlineData(3456, "3,456")]
        [InlineData(3456789, "3,456,789")]
        public void Run(int val, string expected)
        {
            Assert.Equal(expected, val.WithComma());
        }

    }
}
