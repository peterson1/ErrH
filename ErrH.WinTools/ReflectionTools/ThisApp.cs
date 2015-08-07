using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ErrH.Tools.Extensions;

namespace ErrH.WinTools.ReflectionTools
{
    public class ThisApp
    {

        public static FileInfo File
        {
            get
            {
                return new FileInfo(Assembly.GetExecutingAssembly().Location);
            }
        }



        public static DirectoryInfo Folder
        {
            get
            {
                return ThisApp.File.Directory;
            }
        }



        public static FileInfo[] Files(string searchPattern)
        {
            return ThisApp.Folder.GetFiles(searchPattern,
                            SearchOption.TopDirectoryOnly);
        }


        public static FileInfo[] DLLs
        {
            get
            {
                return ThisApp.Files("*.dll");
            }
        }


        public static FileInfo LatestDll
        {
            get
            {
                return ThisApp.DLLs
                        .OrderByDescending(f => f.LastWriteTime)
                        .First();
            }
        }


        public static DateTime LastUpdate
        {
            get
            {
                return ThisApp.LatestDll.LastWriteTime;
            }
        }


        public static string LastUpdated
        {
            get
            {
                return ThisApp.LastUpdate.TimeAgo();
            }
        }

    }
}
