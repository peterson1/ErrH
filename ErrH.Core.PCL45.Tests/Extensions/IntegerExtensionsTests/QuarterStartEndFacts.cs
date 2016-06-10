using System;
using ErrH.Core.PCL45.Extensions;
using FluentAssertions;
using Xunit;

namespace ErrH.Core.PCL45.Tests.Extensions.IntegerExtensionsTests
{
    public class QuarterStartEndFacts
    {
        [Theory(DisplayName = "Q Start")]
        [InlineData(2016, 1, 2016, 1, 1)]
        [InlineData(2016, 2, 2016, 4, 1)]
        [InlineData(2016, 3, 2016, 7, 1)]
        [InlineData(2016, 4, 2016, 10, 1)]
        public void QtrStart(int year, int qtrNum, int eYr, int eMo, int eDy)
        {
            var expctd = new DateTime(eYr, eMo, eDy);
            var actual = year.QuarterStart(qtrNum);
            actual.Should().Be(expctd);
        }

        [Theory(DisplayName = "Q End")]
        [InlineData(2016, 1, 2016, 3, 31)]
        [InlineData(2016, 2, 2016, 6, 30)]
        [InlineData(2016, 3, 2016, 9, 30)]
        [InlineData(2016, 4, 2016, 12, 31)]
        public void QtrEnd(int year, int qtrNum, int eYr, int eMo, int eDy)
        {
            var expctd = new DateTime(eYr, eMo, eDy);
            var actual = year.QuarterEnd(qtrNum);
            actual.Should().Be(expctd);
        }
    }
}
