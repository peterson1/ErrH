using System.Diagnostics;
using System.IO;
using ErrH.Tools.Extensions;

namespace ErrH.Wpf.net45.Extensions
{
    public static class FileInfoEx
    {
        public static void Hide(this FileInfo file)
        {
            var path = file.FullName;
            var atts = File.GetAttributes(path);

            File.SetAttributes(path, 
                atts | FileAttributes.Hidden);
        }


        // from http://stackoverflow.com/a/13049909
        public static string IncrementFileName(string fullPath)
        {
            int count       = 1;
            var nmeOnly     = Path.GetFileNameWithoutExtension(fullPath);
            var extnsion    = Path.GetExtension(fullPath);
            var dirPath     = Path.GetDirectoryName(fullPath);
            var newFullPath = fullPath;

            while (File.Exists(newFullPath))
            {
                //var tmpNme = string.Format("{0}({1})", nmeOnly, count++);
                var tmpNme  = $"{nmeOnly}({count++}){extnsion}";
                newFullPath = Path.Combine(dirPath, tmpNme);
            }

            return newFullPath;
        }


        public static string Version(this FileInfo file)
            => FileVersionInfo.GetVersionInfo(file.FullName).FileVersion ?? "";


        public static string SHA1(this FileInfo file)
        {
            using (var stream = file.OpenRead())
            {
                return stream.SHA1();
            }
        }
    }
}
