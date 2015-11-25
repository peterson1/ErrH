using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.XunitTools.FixtureAttributes;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.DateTimeExtensions
{
    public class ToArg_Facts
    {
        [Test("ToSqlArg()")]
        [InlineData(2015, 11, 1, "'2015-11-01 00:00:00'")]
        public void Case1(int yr, int mo, int dy, string expctd)
        {
            var d8 = new DateTime(yr, mo, dy);
            Assert.Equal(expctd, d8.ToSqlArg());
        }
    }
}
