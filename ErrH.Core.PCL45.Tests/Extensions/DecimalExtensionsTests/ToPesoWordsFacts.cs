using ErrH.Core.PCL45.Extensions;
using FluentAssertions;
using Xunit;

namespace ErrH.Core.PCL45.Tests.Extensions.DecimalExtensionsTests
{
    public class ToPesoWordsFacts
    {
        [Theory(DisplayName=" ")]
        [InlineData(1, "One peso")]
        [InlineData(2, "Two pesos")]
        [InlineData(2.5, "Two pesos and Fifty centavos")]
        [InlineData(835937.5, "Eight Hundred Thirty Five Thousand Nine Hundred Thirty Seven pesos and Fifty centavos")]
        public void Case1(decimal numbr, string expctd)
        {
            numbr.ToPesoWords().Should().Be(expctd);
        }
    }
}
