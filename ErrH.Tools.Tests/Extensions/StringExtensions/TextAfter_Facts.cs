using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.StringExtensions
{
    public class TextAfter_Facts
    {
        [Theory(DisplayName = "Seek from start")]
        [InlineData("a.b.c", ".", "b.c")]
        public void SeekFromStart(string sut, string markr, string expctd)
        {
            Assert.Equal(expctd, sut.TextAfter(markr, seekFromEnd: false));
        }


        [Theory(DisplayName = "Seek from end")]
        [InlineData("a.b.c", ".", "c")]
        public void SeekFromEnd(string sut, string markr, string expctd)
        {
            Assert.Equal(expctd, sut.TextAfter(markr, seekFromEnd: true));
        }

    }
}
