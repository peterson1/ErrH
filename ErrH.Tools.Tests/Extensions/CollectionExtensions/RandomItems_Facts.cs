using System.Collections.Generic;
using System.Linq;
using ErrH.Tools.Extensions;
using ErrH.Tools.Randomizers;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.CollectionExtensions
{
    public class RandomItems_Facts
    {
        [Theory]
        [InlineData(1, 6)]
        [InlineData(2, 6)]
        [InlineData(3, 6)]
        [InlineData(4, 6)]
        [InlineData(5, 6)]
        [InlineData(6, 6)]
        public void RandomItems_LessThanMax(int itemCount, int listCount)
        {
            var sut = new FakeFactory();
            var list = new List<string>();

            for (int i = 0; i < listCount; i++)
                list.Add(sut.Word);

            var items = list.RandomItems(itemCount);

            Assert.Equal(itemCount, items.Count());
            Assert.True(items.IsAllUnique());
        }

    }
}
