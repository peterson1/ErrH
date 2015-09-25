using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.IntegerExtensions
{
    public class x_Facts
    {
        [Theory(DisplayName ="x() pluralizer")]
        [InlineData("3 downs", 3, "down")]
        [InlineData("1 down", 1, "down")]
        [InlineData("No downs", 0, "down")]
        [InlineData("1 life", 1, "life;lives")]
        [InlineData("3 lives", 3, "life;lives")]
        [InlineData("No lives", 0, "life;lives")]
        public void xTheories(string expctd, int count, string arg)
        {
            Assert.Equal(expctd, count.x(arg));
        }
    }
}
