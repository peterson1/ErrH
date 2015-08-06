using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{
    public class TruncateStart_Facts
    {
                        //			 10		   20		 30
                        //  123456789 123456789 123456789 1234567
        const string SUT = "Sphinx of black quartz, judge my vow.";

        [Theory(DisplayName = "TruncateStart")]
        [InlineData(SUT, 12, "", "udge my vow.")]
        [InlineData(SUT, 17, "", "tz, judge my vow.")]
        [InlineData(SUT, 12, "...", "...e my vow.")]
        public void Case1(string sut, int maxChars, string markr, string expctd)
        {
            Assert.Equal(expctd, sut.TruncateStart(maxChars, markr));
        }

    }
}
