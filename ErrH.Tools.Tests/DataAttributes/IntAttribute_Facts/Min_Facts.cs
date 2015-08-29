using System.ComponentModel;
using ErrH.Tools.DataAttributes;
using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.DataAttributes.IntAttribute_Facts
{
    public class Min_Facts
    {
        [Fact] public void Test_Min0()
        {
            var sut = new Sut();
            var errI = sut as IDataErrorInfo;

            sut.Min0 = 0;
            Assert.Equal("", errI["Min0"]);
            Assert.Equal("", errI.Error);

            sut.Min0 = 3;
            Assert.Equal("", errI["Min0"]);
            Assert.Equal("", errI.Error);

            sut.Min0 = -1;
            Assert.Equal("“Min0” should not be less than 0.", errI["Min0"]);
            Assert.Equal("“Min0” should not be less than 0.", errI.Error);

            sut.Min1 = 0;
            Assert.Equal("“Min0” should not be less than 0.", errI["Min0"]);
            Assert.Equal("“Min1” should not be less than 1.", errI["Min1"]);
            Assert.Equal("“Min0” should not be less than 0." 
                 + L.f + "“Min1” should not be less than 1.", errI.Error);
        }



        private class Sut : IDataErrorInfo
        {
            [Int(Min = 0)]
            public int Min0 { get; set; } = 0;

            [Int(Min = 1)]
            public int Min1 { get; set; } = 1;

            [Int(Min = 3)]
            public int Min3 { get; set; } = 3;

            [Int(Min = -5)]
            public int MinNeg5 { get; set; } = -5;

            [Int(Min = -1)]
            public int MinNeg1 { get; set; } = -1;

            public int NoMin { get; set; }



            public string Error => DataError.Info(this);

            public string this[string col] 
                => DataError.Info(this, col);
        }
    }
}
