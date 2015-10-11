using System;
using ErrH.Tools.Loggers;
using Xunit;

namespace ErrH.Tools.Tests.Loggers.TextLog_Theories
{
    public class ShortDate_Facts
    {
        [Theory]
        [InlineData("2015-12-30 17:25:22", "1230 5:25p")]
        [InlineData("2015-12-30 12:25:22", "1230 12:25")]
        [InlineData("2015-02-26 12:25:22", "2-26 12:25")]
        [InlineData("2015-12-01 04:25:22", "12-1 4:25a")]
        public void Cases(string date, string expctd)
        {
            var dt = DateTime.Parse(date);
            Assert.Equal(expctd, TextLog.ShortDate(dt));
        }
    }
}
