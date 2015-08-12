using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{
    public class CountOccurence_Facts
    {
        [Theory(DisplayName = "?")]
        [InlineData(1, '.', "1.2")]
        [InlineData(3, '.', "1.23.456.7")]
        [InlineData(0, '.', "1234567")]
        public void Cases(int expctd, char findThis, string sut)
        {
            Assert.Equal(expctd, sut.CountOccurence(findThis));
        }
    }
}
