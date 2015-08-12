using System.ComponentModel;
using ErrH.Tools.DataAttributes;
using ErrH.Tools.FileSystemShims;
using ErrH.WinTools.FileSystemTools;
using Xunit;

namespace ErrH.Tools.Tests.DataAttributes
{
    public class FolderExistsAttribute_Facts
    {
        private IFileSystemShim _fs = new WindowsFsShim();


        [Fact(DisplayName = "FolderExists: null or blank")]
        public void NullOrBlank()
        {
            var sut  = new Sut { FsShim = _fs };
            var errI = sut as IDataErrorInfo;

            Assert.Equal("", errI["Optional"]);
            Assert.Equal("“Required” should not be ‹NULL›.", errI["Required"]);

            sut.Optional = "";
            sut.Required = "";

            Assert.Equal("", errI["Optional"]);
            Assert.Equal("“Required” should not be ‹BLANK›.", errI["Required"]);
        }


        [Fact(DisplayName = "FolderExists: non-existent")]
        public void NonExistent()
        {
            var sut = new Sut { FsShim = _fs };
            var errI = sut as IDataErrorInfo;

            var none = @"C:\abc\non-existent\folder\";
            sut.Optional = none;
            sut.Required = none;

            Assert.Equal($"“Optional” does not exist as a folder in {none}.", errI["Optional"]);
            Assert.Equal($"“Required” does not exist as a folder in {none}.", errI["Required"]);
        }


        [Fact(DisplayName = "FolderExists: Happy! :-)")]
        public void HappyPath()
        {
            var sut  = new Sut { FsShim = _fs };
            var errI = sut as IDataErrorInfo;

            var folder   = _fs.GetSpecialDir(SpecialDir.Desktop);
            sut.Optional = folder;
            sut.Required = folder;

            Assert.Equal("", errI["Optional"]);
            Assert.Equal("", errI["Required"]);
        }


        [Fact(DisplayName = "FolderExists: File instead")]
        public void Case1()
        {
            var sut  = new Sut { FsShim = _fs };
            var errI = sut as IDataErrorInfo;

            var file     = _fs.TempFile("");
            sut.Optional = file.Path;
            sut.Required = file.Path;

            Assert.Equal($"“Optional” does not exist as a folder in {file.Path}.", errI["Optional"]);
            Assert.Equal($"“Required” does not exist as a folder in {file.Path}.", errI["Required"]);
        }


        private class Sut : IDataErrorInfo
        {
            internal IFileSystemShim FsShim = null;

            [FolderExists]
            public string Optional { get; set; }

            [Required, FolderExists]
            public string Required { get; set; }


            public string Error => DataError.Info(this, FsShim);

            public string this[string col]
                => DataError.Info(this, col, FsShim);
        }

    }
}
