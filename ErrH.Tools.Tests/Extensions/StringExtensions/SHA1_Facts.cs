using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{
    public class SHA1_Facts
    {
        [Theory(DisplayName = "SHA-1")]
        [InlineData("3e92f6c3-60c53c7f-538a78e1-4866fb59-38a31c76", "pers")]
        [InlineData("dd0bce76-b363f2e0-1abb1fad-a7e26d6f-d5099463", "isa pa")]
        public void Case1(string expctd, string sut)
        {
            Assert.Equal(expctd, sut.SHA1());
        }
    }
}
