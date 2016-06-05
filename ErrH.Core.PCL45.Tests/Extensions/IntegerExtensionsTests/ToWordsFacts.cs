using ErrH.Core.PCL45.Extensions;
using FluentAssertions;
using Xunit;

namespace ErrH.Core.PCL45.Tests.Extensions.IntegerExtensionsTests
{
    public class ToWordsFacts
    {
        [Theory(DisplayName = " ")]
        [InlineData(800, "Eight Hundred")]
        public void Case1(int numbr, string expctd)
        {
            numbr.ToWords().Should().Be(expctd);
        }
    }
}
