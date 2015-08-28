using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{
    public class Between_Facts
    {
        [Theory(DisplayName = "Between: seek last from start")]
        [InlineData("abcdef", "bc", "e", "d")]
        public void BetweenSeekFromStart(string full, string before, string after, string expctd)
        {
            var actual = full.Between(before, after, false);
            Assert.Equal(expctd, actual);
        }


        [Theory(DisplayName = "Between: seek last from end")]
        [InlineData("http://localhost/api", "//", "/", "localhost")]
        public void BetweenSeekFromEnd(string full, string before, string after, string expctd)
        {
            var actual = full.Between(before, after, true);
            Assert.Equal(expctd, actual);
        }


        [Theory(DisplayName = "Between: end string found in start ")]
        [InlineData("ab.cd.ef.g", "ab.cd.", ".", "ef")]
        public void EndFoundInStart(string full, string before, string after, string expctd)
        {
            var actual = full.Between(before, after, false);
            Assert.Equal(expctd, actual);
        }
    }
}
