using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ErrH.Tools.Extensions;

namespace ErrH.WinTools.FileSystemTools
{
    public static class FileInfoExtensions
    {
        public static string SemanticVer(this FileInfo file)
        {
            //return v.FileMajorPart + "." 
            //	 + v.FileMinorPart + "." 
            //	 + v.FileBuildPart + "." 
            //	 + v.FilePrivatePart;

            return file.Ver().FileVersion;
        }


        public static FileVersionInfo Ver(this FileInfo file)
        {
            return FileVersionInfo.GetVersionInfo(file.FullName);
        }


        public static string SHA1(this FileInfo file)
        {
            using (var stream = file.OpenRead())
                return stream.SHA1();
        }


        public static string Md5Hash(this FileInfo file)
        {
            using (var md5 = MD5.Create())
            using (var stream = file.OpenRead())
            {
                return md5.ComputeHash(stream).AsHash();
            }
        }

        public static string Description(this FileInfo file)
        {
            return file.Ver().FileDescription;
        }


        /// <summary>
        /// Writes string to file using UTF8 encoding. Overwrites if exists.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content"></param>
        /// <param name="appendUnicodeBOM"></param>
        public static void WriteUTF8(this FileInfo file, string content, bool appendUnicodeBOM = false)
        {
            if (file.Exists) file.Delete();

            using (var fs = file.OpenWrite())
            {
                var byts = new UTF8Encoding(appendUnicodeBOM)
                                          .GetBytes(content);

                fs.Write(byts, 0, byts.Length);
            }
        }


        public static string ReadUTF8(this FileInfo file)
        {
            using (var fs = file.OpenText())
                return fs.ReadToEnd();
        }
    }
}
