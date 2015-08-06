using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.IntegerExtensions
{
    public class KB_Facts
    {
        [Theory(DisplayName = "Long to Bytes")]
        [InlineData(9223372036854775807, "8 EB")]//long.MaxValue
        [InlineData(0, "0 B")]
        [InlineData(1024, "1 KB")]
        [InlineData(2000000, "1.9 MB")]
        [InlineData(-9023372036854775807, "-7.8 EB")]
        //[InlineData(long.MinValue, "8 EB")]//overflows
        public void Long(long sut, string expctd)
        {
            Assert.Equal(expctd, sut.KB());
        }


        [Theory(DisplayName = "Int to Bytes")]
        [InlineData(int.MaxValue, "2 GB")]
        [InlineData(0, "0 B")]
        [InlineData(1024, "1 KB")]
        [InlineData(2000000, "1.9 MB")]
        [InlineData(int.MinValue, "-2 GB")]
        public void Int(int sut, string expctd)
        {
            Assert.Equal(expctd, sut.KB());
        }

    }
}
