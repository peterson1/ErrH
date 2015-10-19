using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{
    public class IsAllLower_Facts
    {
        [Theory]
        [InlineData(true, "abcd ef g 123s d")]
        [InlineData(false, "aBcd ef g 123s d")]
        public void Case(bool expctd, string sut)
        {
            Assert.Equal(expctd, sut.IsAllLower());
        }
    }
}
