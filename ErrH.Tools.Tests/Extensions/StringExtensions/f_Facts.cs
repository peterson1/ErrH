using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{
    public class f_Facts
    {
        [Theory(DisplayName = "normal string.Format")]
        [InlineData("s1 then s2 then s3", "{0} then {1} then {2}", "s1", "s2", "s3")]
        [InlineData("1 down, 3 ups", "{0:down}, {1:up}", 1, 3, "")]
        [InlineData("3 downs, 1 up", "{0:down}, {1:up}", 3, 1, "")]
        [InlineData("No downs, 1 up", "{0:down}, {1:up}", 0, 1, "")]
        [InlineData("non-plural: 123", "non-plural: {0}", 123, "", "")]
        [InlineData("3 downs, solo up", "{0:down}, {1:up}", 3, "solo", "")]
        [InlineData("1 life", "{0:life;lives}", 1, "", "")]
        [InlineData("3 lives", "{0:life;lives}", 3, "", "")]
        [InlineData("No lives", "{0:life;lives}", 0, "", "")]
        public void Formatting(string expctd, string sut, object arg1, object arg2, object arg3)
        {
            Assert.Equal(expctd, sut.f(arg1, arg2, arg3));
        }


        [Fact]
        public void CSharp6Interpolate()
        {
            var expctd = "Nat is 2 years old.";
            var nme = "Nat";
            var age = 2;
            
            var actual = $"{nme} is {age} years old.";
            Assert.Equal(expctd, actual);
        }


    }
}
