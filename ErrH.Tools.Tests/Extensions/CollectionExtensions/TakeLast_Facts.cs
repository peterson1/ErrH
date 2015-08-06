using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.CollectionExtensions
{
    public class TakeLast_Facts
    {
        [Fact(DisplayName = "Take Last")]
        public void TakeLast()
        {
            var list = new List<string> { "a", "b", "c" };

            var expctd = new List<string> { "c" };
            var actual = list.TakeLast(1);
            Assert.Equal(expctd, actual);

            expctd = new List<string> { "b", "c" };
            actual = list.TakeLast(2);
            Assert.Equal(expctd, actual);

            expctd = new List<string> { "a", "b", "c" };
            actual = list.TakeLast(3);
            Assert.Equal(expctd, actual);
        }
    }
}
