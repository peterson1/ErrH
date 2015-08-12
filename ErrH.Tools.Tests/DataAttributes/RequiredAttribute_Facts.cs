using System.ComponentModel;
using ErrH.Tools.DataAttributes;
using Xunit;

namespace ErrH.Tools.Tests.DataAttributes
{
    public class RequiredAttribute_Facts
    {
        [Fact] public void Case1()
        {
            var sut = new Sut();
            var errI = sut as IDataErrorInfo;

            Assert.Equal("", errI["OptionalText"]);
            Assert.Equal("“RequiredText” should not be ‹NULL›.", errI["RequiredText"]);
            Assert.Equal("“RequiredText” should not be ‹NULL›.", errI.Error);

            sut.OptionalText = "abc";
            sut.RequiredText = "abc";

            Assert.Equal("", errI["OptionalText"]);
            Assert.Equal("", errI["RequiredText"]);
            Assert.Equal("", errI.Error);

            sut.OptionalText = "";
            sut.RequiredText = "";

            Assert.Equal("", errI["OptionalText"]);
            Assert.Equal("“RequiredText” should not be ‹BLANK›.", errI["RequiredText"]);
            Assert.Equal("“RequiredText” should not be ‹BLANK›.", errI.Error);

            sut.RequiredText = " ";
            Assert.Equal("“RequiredText” should not be ‹BLANK›.", errI["RequiredText"]);
        }


        private class Sut : IDataErrorInfo
        {
            [Required]
            public string  RequiredText  { get; set; }

            public string  OptionalText  { get; set; }


            public string Error => DataError.Info(this);

            public string this[string col]
                => DataError.Info(this, col);
        }
    }
}
