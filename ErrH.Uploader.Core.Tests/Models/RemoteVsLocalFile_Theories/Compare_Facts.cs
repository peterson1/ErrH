using ErrH.Tools.FileSynchronization;
using ErrH.Uploader.Core.Models;
using ErrH.XunitTools;
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
            var sut = new RemoteVsLocalFile("file.txt", null, null);

            sut.Comparison.MustBe(FileDiff.Unavailable, "result state");
            sut.NextStep.MustBe(FileTask.Analyze);
            sut.Target.MustBe(Target.Both);
        }


        [Fact(DisplayName = "Result: NotInLocal")]
        public void Result_NotInLocal()
        {
            var sut = new RemoteVsLocalFile("file.txt",
                                            new SyncableFileInfo(),
                                            null);

            sut.Comparison.MustBe(FileDiff.NotInLocal, "result state");
            sut.NextStep.MustBe(FileTask.Delete);
            sut.Target.MustBe(Target.Remote);
        }


        [Fact(DisplayName = "Result: NotInRemote")]
        public void Result_NotInRemote()
        {
            var sut = new RemoteVsLocalFile("file.txt",
                                            null,
                                            new SyncableFileInfo());

            sut.Comparison.MustBe(FileDiff.NotInRemote, "result state");
            sut.NextStep.MustBe(FileTask.Create);
            sut.Target.MustBe(Target.Remote);
        }


        [Fact(DisplayName = "Result: Changed Size")]
        public void Result_Changed_Size()
        {
            var sut = new RemoteVsLocalFile("file.txt",
                                           new SyncableFileInfo(),
                                           new SyncableFileInfo { Size = 123 });

            sut.Comparison.MustBe(FileDiff.Changed, "result state");
            sut.OddProperty.MustBe(nameof(SyncableFileInfo.Size), "odd property");
            sut.NextStep.MustBe(FileTask.Replace);
            sut.Target.MustBe(Target.Remote);
        }


        [Fact(DisplayName = "Result: Changed Version")]
        public void Result_Changed_Version()
        {
            var sut = new RemoteVsLocalFile("file.txt",
                                           new SyncableFileInfo(),
                                           new SyncableFileInfo { Version = "v.5" });

            sut.Comparison.MustBe(FileDiff.Changed, "result state");
            sut.OddProperty.MustBe(nameof(SyncableFileInfo.Version), "odd property");
            sut.NextStep.MustBe(FileTask.Replace);
            sut.Target.MustBe(Target.Remote);
        }


        [Fact(DisplayName = "Result: Changed SHA1")]
        public void Result_Changed_SHA1()
        {
            var sut = new RemoteVsLocalFile("file.txt",
                                           new SyncableFileInfo(),
                                           new SyncableFileInfo { SHA1 = "123-456-789" });

            sut.Comparison.MustBe(FileDiff.Changed, "result state");
            sut.OddProperty.MustBe(nameof(SyncableFileInfo.SHA1), "odd property");
            sut.NextStep.MustBe(FileTask.Replace);
            sut.Target.MustBe(Target.Remote);
        }


        [Fact(DisplayName = "Result: Same")]
        public void Result_Same()
        {
            var rem = new SyncableFileInfo();
            var loc = new SyncableFileInfo();

            loc.Size    = rem.Size    = 123;
            loc.Version = rem.Version = "v.456";
            loc.SHA1    = rem.SHA1    = "abc-def-ghi";

            var sut    = new RemoteVsLocalFile("file.txt", rem, loc);

            sut.Comparison.MustBe(FileDiff.Same, "result state");
            sut.OddProperty.MustBe(null, "odd property");
            sut.NextStep.MustBe(FileTask.Ignore);
            sut.Target.MustBe(Target.Both);
        }
    }
}
