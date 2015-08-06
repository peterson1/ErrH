using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.DecimalExtensions
{
    public class AsPeso_Facts
    {
        [Theory(DisplayName = "Decimal As Peso")]
        [InlineData(0, "₱ 0.00")]
        [InlineData(1, "₱ 1.00")]
        [InlineData(10.25, "₱ 10.25")]
        [InlineData(100.5, "₱ 100.50")]
        [InlineData(2000.75, "₱ 2,000.75")]
        public void AsPesoTheories(decimal val, string expected)
        {
            Assert.Equal(expected, val.AsPeso());
        }
    }
}
