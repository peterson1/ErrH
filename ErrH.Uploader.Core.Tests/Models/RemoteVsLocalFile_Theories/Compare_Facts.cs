using ErrH.Uploader.Core.Models;
using ErrH.XunitTools;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using Xunit.Abstractions;

namespace ErrH.Uploader.Core.Tests.Models.RemoteVsLocalFile_Theories
{
    public class Compare_Facts
    {
        public Compare_Facts(ITestOutputHelper output)
        {
            MustExtensions.OutputHelper = output;
        }


        [Fact(DisplayName ="Result: Unavailable")]
        public void Result_Unavailable()
        {
            var sut = new RemoteVsLocalFile("file.txt");

            sut.Compare.MustBe(FileDiff.Unavailable, "result state");
        }


        [Fact(DisplayName = "Result: Unavailable (no values)")]
        public void Result_Unavailable_NoValues()
        {
            var sut    = new RemoteVsLocalFile("file.txt");
            sut.Remote = new AppFileInfo();
            sut.Local  = new AppFileInfo();

            sut.Compare.MustBe(FileDiff.Unavailable, "result state");
            sut.Remote.Equals(sut.Local).MustBe(true, "object Equals()");
        }


        [Fact(DisplayName = "Result: NotInLocal")]
        public void Result_NotInLocal()
        {
            var sut      = new RemoteVsLocalFile("file.txt");
            sut.Remote   = new AppFileInfo();

            sut.Compare.MustBe(FileDiff.NotInLocal, "result state");
            sut.Remote.Equals(sut.Local).MustBe(false, "object Equals()");
        }


        [Fact(DisplayName = "Result: NotInRemote")]
        public void Result_NotInRemote()
        {
            var sut      = new RemoteVsLocalFile("file.txt");
            sut.Local    = new AppFileInfo();

            sut.Compare.MustBe(FileDiff.NotInRemote, "result state");
            sut.Local.Equals(sut.Remote).MustBe(false, "object Equals()");
        }


        [Fact(DisplayName = "Result: Changed Size")]
        public void Result_Changed_Size()
        {
            var sut      = new RemoteVsLocalFile("file.txt");
            sut.Local    = new AppFileInfo();
            sut.Remote   = new AppFileInfo();
            sut.Local.Size = 123;

            sut.Compare.MustBe(FileDiff.Changed, "result state");
            sut.OddProperty.MustBe(nameof(sut.Local.Size), "odd property");
            sut.Remote.Equals(sut.Local).MustBe(false, "object Equals()");
        }


        [Fact(DisplayName = "Result: Changed Version")]
        public void Result_Changed_Version()
        {
            var sut           = new RemoteVsLocalFile("file.txt");
            sut.Local         = new AppFileInfo();
            sut.Remote        = new AppFileInfo();
            sut.Local.Version = "v.5";

            sut.Compare.MustBe(FileDiff.Changed, "result state");
            sut.OddProperty.MustBe(nameof(sut.Local.Version), "odd property");
            sut.Remote.Equals(sut.Local).MustBe(false, "object Equals()");
        }


        [Fact(DisplayName = "Result: Changed SHA1")]
        public void Result_Changed_SHA1()
        {
            var sut        = new RemoteVsLocalFile("file.txt");
            sut.Local      = new AppFileInfo();
            sut.Remote     = new AppFileInfo();
            sut.Local.SHA1 = "123-456-789";

            sut.Compare.MustBe(FileDiff.Changed, "result state");
            sut.OddProperty.MustBe(nameof(sut.Local.SHA1), "odd property");
            sut.Remote.Equals(sut.Local).MustBe(false, "object Equals()");
        }


        [Fact(DisplayName = "Result: Same")]
        public void Result_Same()
        {
            var sut    = new RemoteVsLocalFile("file.txt");
            sut.Local  = new AppFileInfo();
            sut.Remote = new AppFileInfo();

            sut.Local.Size    = sut.Remote.Size    = 123;
            sut.Local.Version = sut.Remote.Version = "v.456";
            sut.Local.SHA1    = sut.Remote.SHA1    = "abc-def-ghi";

            sut.Compare.MustBe(FileDiff.Same, "result state");
            sut.OddProperty.MustBe(null, "odd property");
            sut.Remote.Equals(sut.Local).MustBe(true, "object Equals()");
        }
    }
}
