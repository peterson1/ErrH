using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.CollectionExtensions
{
    public class IsAllUnique_Facts
    {
        public static IEnumerable<object[]> Integers
        {
            get
            {
                yield return new object[] { true, new int[] { 1, 2, 3 } };
                yield return new object[] { true, new int[] { 1 } };
                yield return new object[] { false, new int[] { 2, 2, 3 } };
                yield return new object[] { false, new int[] { 1, 3, 3 } }; ;
            }
        }

        public static IEnumerable<object[]> Strings
        {
            get
            {
                yield return new object[] { true, new string[] { "a", "b", "c" } };
                yield return new object[] { true, new string[] { "A", "a", "b" } };
                yield return new object[] { false, new string[] { "a", "b", "a" } };
                yield return new object[] { false, new string[] { "a", "b", "b" } };
            }
        }

        [Theory]
        [MemberData("Strings")]
        public void IsAllUnique<T>(bool expctd, T[] items)
        {
            Assert.Equal(expctd, items.IsAllUnique());
        }

        [Theory, MemberData("Integers")]
        public void IsAllUnique_Int(bool expctd, int[] items)
        {
            Assert.Equal(expctd, items.IsAllUnique());
        }

    }
}
