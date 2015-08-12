using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{

    public class IsNumeric_Facts
    {
        [Theory(DisplayName = " ")]

        [InlineData(true , "123")]// obvious
        [InlineData(false, "abc")]//

        [InlineData(true , " 123")]// allow leading
        [InlineData(true , "123 ")]//  & trailing spaces

        [InlineData(true , "1.23"  )]// decimals
        [InlineData(false, "1.23.4")]//

        [InlineData(true , "-123" )]// negatives
        [InlineData(true , "-1.23")]//
        [InlineData(true, "- 1.23")]//
        public void Cases(bool expctd, string sut)
        {
            Assert.Equal(expctd, sut.IsNumeric());
            //if (!expctd) return;
            //if (sut.IsInteger()) sut.ToInt(); //later: IsInteger()
            //sut.ToDecimal();                  //later: ToDecimal()
        }
    }
}
