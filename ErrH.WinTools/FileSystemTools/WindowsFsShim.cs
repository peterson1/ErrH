using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;

namespace ErrH.WinTools.FileSystemTools
{
    public class WindowsFsShim : LogSourceBase, IFileSystemShim
    {

        public FileShim File(string filePath) { return ForwardLogs(new FileShim(this, filePath)); }
        public FolderShim Folder(string folderPath) { return ForwardLogs(new FolderShim(this, folderPath)); }

        //  if implementing, move these to extensions class
        //
        //public FileShim ExeFile { get { return this.File(this.GetAssemblyFile()); } }
        //public FolderShim Folder(SpecialDir specialDir, string subDirectory) { return this.Folder(this.GetSpecialDir(specialDir, subDirectory)); }
        //public FolderShim LocalAppDataDir(string subDirectory) { return this.Folder(SpecialDir.LocalApplicationData, subDirectory); }
        //public FolderShim ExeFolder { get { return this.Folder(this.GetAssemblyDir()); } }

        public string GetSpecialDir(SpecialDir specialDir, string subDirectory) { return Environment.GetFolderPath((Environment.SpecialFolder)specialDir).Bslash(subDirectory); }
        public string GetFileVersion(string filePath) { return FileVersionInfo.GetVersionInfo(filePath).FileVersion; }
        public string GetFileExtension(string filePath) { return Path.GetExtension(filePath); }
        public string GetFileNameWithoutPath(string filePath) { return Path.GetFileName(filePath); }
        public string GetAssemblyFile() { return Assembly.GetExecutingAssembly().Location; }
        public string GetAssemblyDir() { return this.GetParentDir(this.GetAssemblyFile()); }
        public string GetFileNameWithoutExtension(string filePath) { return Path.GetFileNameWithoutExtension(filePath); }
        public string GetParentDir(string fileOrFolderPath) { return Path.GetDirectoryName(fileOrFolderPath); }
        public string ReadFileUTF8(string filePath) { using (var stream = new FileInfo(filePath).OpenText()) return stream.ReadToEnd(); }
        public string CombinePath(params string[] paths) { return Path.Combine(paths); }
        public string GetDirName(string directoryPath) { return Path.GetDirectoryName(directoryPath); }

        public bool IsFolderFound(string folderPath) { return Directory.Exists(folderPath); }
        public bool IsFileFound(string filePath) { return System.IO.File.Exists(filePath); }
        public bool IsFileHidden(string filePath) { return System.IO.File.GetAttributes(filePath).HasFlag(FileAttributes.Hidden); }
        public bool TryHideFile(string filePath, out string errorMessage) { return SetHiddenAttribute(true, filePath, out errorMessage); }
        public bool TryUnhideFile(string filePath, out string errorMessage) { return SetHiddenAttribute(false, filePath, out errorMessage); }

        public long GetFileSize(string filePath) { return new FileInfo(filePath).Length; }
        public Stream OpenFileStream(string filePath) { return System.IO.File.OpenRead(filePath); }
        public byte[] ReadFileBytes(string filePath) { return System.IO.File.ReadAllBytes(filePath); }





        private static bool SetHiddenAttribute(bool makeHidden, string filePath, out string errMsg)
        {
            return Try(out errMsg, () =>
            {
                if (makeHidden)
                    System.IO.File.SetAttributes(filePath, System.IO.File.GetAttributes(
                                    filePath) | FileAttributes.Hidden);
                else
                {
                    var oldAtts = System.IO.File.GetAttributes(filePath);
                    var newAtts = RemoveAttribute(oldAtts, FileAttributes.Hidden);
                    System.IO.File.SetAttributes(filePath, newAtts);
                }
            });
        }

        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }





        public bool TryCreateNewTempFile(out string newTempFilePath, out string errorMsg)
        {
            errorMsg = newTempFilePath = string.Empty;
            try
            {
                newTempFilePath = Path.GetTempFileName();
            }
            catch (Exception ex)
            {
                errorMsg = ex.Details(true, false);
                return false;
            }
            return true;
        }


        /// <summary>
        /// Creates a temporary file in the system's temp folder.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encodeAs"></param>
        /// <returns></returns>
        public FileShim TempFile(string content, EncodeAs encodeAs = EncodeAs.UTF8)
        {
            string fPath; string errMsg; FileShim tempF = null;
            Debug_i("Creating temporary file...");

            if (!this.TryCreateNewTempFile(out fPath, out errMsg))
                return Error_o_(tempF, "Failed to create temporary file." + L.F + errMsg);

            Debug_o("created: " + fPath);
            tempF = this.File(fPath);
            tempF.Write(content, encodeAs);
            return tempF;
        }



        public bool TryCreateDir(string folderPath, out string errMsg)
        {
            errMsg = string.Empty;
            if (this.IsFolderFound(folderPath)) return true;
            return Try(out errMsg, () =>
            {
                Directory.CreateDirectory(folderPath);
            });
        }



        public bool TryGetDirFiles(string folderPath, string searchPattern,
            out List<FileShim> fileList, out string errorMsg)
        {
            fileList = null; FileInfo[] fileInfos; errorMsg = null;
            try
            {
                fileInfos = new DirectoryInfo(folderPath).GetFiles(searchPattern,
                                                    SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Details(false, false);
                return false;
            }

            fileList = fileInfos.Select(x => this.File(x.FullName)).ToList();
            return true;
        }




        public bool TryWriteFile(string filePath, out string errMsg, byte[] bytes, bool overwriteExisting)
        {
            if (!this.TryCreateDir(this.GetParentDir(filePath), out errMsg)) return false;
            return Try(out errMsg, () =>
            {
                if (overwriteExisting)
                    System.IO.File.WriteAllBytes(filePath, bytes);
                else
                    using (var writr = System.IO.File.OpenWrite(filePath))
                        writr.Write(bytes, 0, bytes.Length);
            });
        }


        public bool TryWriteFile(string filePath, out string errMsg, string content, EncodeAs encoding, bool overwriteExisting)
        {
            if (!this.TryCreateDir(this.GetParentDir(filePath), out errMsg)) return false;
            return Try(out errMsg, () =>
            {
                if (encoding != EncodeAs.UTF8) Throw.Unsupported(encoding);

                var file = new FileInfo(filePath);
                using (var fs = overwriteExisting ? file.Create() : file.OpenWrite())
                {
                    var byts = new UTF8Encoding(false).GetBytes(content);// param "false" : do NOT append Unicode BOM
                    fs.Write(byts, 0, byts.Length);
                }
            });
        }


        public bool TryMoveFile(string oldPath, out string errMsg, string newPath)
        {
            return Try(out errMsg, () =>
            {
                System.IO.File.Move(oldPath, newPath);
            });
        }


        public bool TryDeleteFile(string filePath, out string errMsg)
        {
            return Try(out errMsg, () =>
            {
                System.IO.File.Delete(filePath);
            });
        }


        private static bool Try(out string errMSg, Action code)
        {
            try
            {
                code.Invoke();
            }
            catch (Exception ex)
            {
                errMSg = ex.Message;
                return false;
            }
            errMSg = string.Empty;
            return true;
        }

    }
}
