using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{
    public class TrimStart_Facts
    {
        [Theory(DisplayName = "TrimStart")]
        [InlineData("abcd", "a", "bcd")]
        [InlineData("abcd", "ab", "cd")]
        [InlineData("abcd", "abcd", "")]
        [InlineData("abcd", "b", "abcd")]
        [InlineData("abcd", "abX", "abcd")]
        public void Run(string sut, string param, string expctd)
        {
            Assert.Equal(expctd, sut.TrimStart(param));
        }

    }
}
