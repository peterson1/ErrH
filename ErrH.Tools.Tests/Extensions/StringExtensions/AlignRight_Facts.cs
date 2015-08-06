using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{
    public class AlignRight_Facts
    {
                         //			  10        20        30
                         //  123456789 123456789 123456789 1234567
        const string SUT1 = "Sphinx of black quartz, judge my vow.";

        [Theory(DisplayName = "AlignRight truncates")]
        [InlineData(SUT1, 12, "", "udge my vow.")]
        [InlineData(SUT1, 17, "", "tz, judge my vow.")]
        [InlineData(SUT1, 12, "...", "...e my vow.")]
        public void Truncating(string sut, int maxChars, string markr, string expctd)
        {
            Assert.Equal(expctd, sut.AlignRight(maxChars, markr));
        }


        //			  10        20        30
        //  123456789 123456789 123456789 1234567
        const string SUT2 = "it's short";

        [Theory(DisplayName = "AlignRight pads")]
        [InlineData(SUT2, 12, "", "  it's short")]
        [InlineData(SUT2, 17, "", "       it's short")]
        [InlineData(SUT2, 12, "...", "  it's short")]
        public void Padding(string sut, int maxChars, string markr, string expctd)
        {
            Assert.Equal(expctd, sut.AlignRight(maxChars, markr));
        }


        [Theory(DisplayName = "AlignRight same length")]
        [InlineData(SUT2, 10, "", "it's short")]
        public void SameLength(string sut, int maxChars, string markr, string expctd)
        {
            Assert.Equal(expctd, sut.AlignRight(maxChars, markr));
        }
    }
}
