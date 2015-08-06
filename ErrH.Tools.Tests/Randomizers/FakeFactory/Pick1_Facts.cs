using System.Collections.Generic;
using Xunit;

namespace ErrH.Tools.Tests.Randomizers.FakeFactory
{
    public class Pick1_Facts
    {
        [Fact(DisplayName = "FakeFactory.Pick1")]
        public void PickFromArgs_case1()
        {
            var sut = new ErrH.Tools.Randomizers.FakeFactory();
            var list = new List<string> { "s1", "s2", "s3" };

            var choice = sut.Pick1("s1", "s2", "s3");

            Assert.Contains(choice, list);
        }

    }
}
