using System;
using FluentAssertions;
using Xunit;

namespace ErrH.Core.PCL45.Tests
{
    public class Class1
    {
        [Fact(DisplayName = "Native DateParse")]
        public void Case1()
        {
            var str    = "Mar  9 2016  1:54PM";
            var expctd = new DateTime(2016, 3, 9, 13, 54, 0);
            var actual = DateTime.Parse(str);
            actual.Should().Be(expctd);
        }
    }
}
