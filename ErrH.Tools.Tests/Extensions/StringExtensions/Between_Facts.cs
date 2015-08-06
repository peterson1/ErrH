using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{
    public class Between_Facts
    {
        [Theory(DisplayName = "Between: seek last from end")]
        [InlineData("http://localhost/api", "//", "/", "localhost")]
        public void BetweenSeekFromEnd(string full, string before, string after, string expctd)
        {
            var actual = full.Between(before, after, true);
            Assert.Equal(expctd, actual);
        }

    }
}
