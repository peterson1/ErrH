using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{
    public class Slash_Facts
    {
        [Theory]
        [InlineData("http://base.url/suburl", "http://base.url", "suburl")]
        [InlineData("http://base.url/suburl", "http://base.url/", "suburl")]
        [InlineData("http://base.url/suburl", "http://base.url", "/suburl")]
        [InlineData("http://base.url/suburl", "http://base.url/", "/suburl")]
        public void AddsSlashAsNeeded(string full, string baseU, string sub)
        {
            Assert.Equal(full, baseU.Slash(sub));
        }


        [Theory(DisplayName = "Slash")]
        [InlineData("http://base.url/suburl", "http://base.url", "suburl")]
        [InlineData("http://base.url/suburl", "http://base.url/", "suburl")]
        [InlineData("http://base.url/suburl", "http://base.url", "/suburl")]
        [InlineData("http://base.url/suburl", "http://base.url/", "/suburl")]
        public void AddsSlashAsNeeded_2(string expctd, string baseU, string sub)
        {
            Assert.Equal(expctd, baseU.Slash(sub));
        }

        [Theory(DisplayName = "Bslash")]
        [InlineData(@"C:\aa\b", @"C:\aa", @"b")]
        [InlineData(@"C:\aa\b", @"C:\aa\", @"b")]
        [InlineData(@"C:\aa\b", @"C:\aa", @"\b")]
        [InlineData(@"C:\aa\b", @"C:\aa\", @"\b")]
        [InlineData(@"C:\aa\", @"C:\aa", null)]
        public void BackSlash(string expctd, string s1, string s2)
        {
            Assert.Equal(expctd, s1.Bslash(s2));
        }
    }
}
