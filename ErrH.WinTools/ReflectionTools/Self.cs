using System.Reflection;
using ErrH.Tools.FileSystemShims;
using ErrH.WinTools.FileSystemTools;

namespace ErrH.WinTools.ReflectionTools
{
    public class Self
    {
        private static IFileSystemShim _fs;
        public  static IFileSystemShim  Fs
            => _fs ?? (_fs = new WindowsFsShim());

        public static string Path
            => Assembly.GetExecutingAssembly().Location;

        public static FileShim   File   => Fs.File(Self.Path);
        public static FolderShim Folder => Self.File.Parent;
    }
}
