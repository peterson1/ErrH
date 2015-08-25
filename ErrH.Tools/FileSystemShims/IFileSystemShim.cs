using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.FileSystemShims
{
    public interface IFileSystemShim : ILogSource
    {
        FileShim File(string filePath);
        FolderShim Folder(string folderPath);

        FileShim ExeFile { get; }
        FileShim TempFile(string content, EncodeAs encoding = EncodeAs.UTF8);
        FolderShim Folder(SpecialDir specialDir, string subDirectory = null);
        FolderShim LocalAppDataDir(string subDirectory = null);
        FolderShim ExeFolder { get; }

        string GetFileNameWithoutPath(string filePath);
        string GetFileExtension(string filePath);
        string GetFileNameWithoutExtension(string filePath);
        string GetFileVersion(string filePath);
        string GetParentDir(string fileOrFolderPath);
        string GetAssemblyFile();
        string GetAssemblyDir();
        string GetSpecialDir(SpecialDir specialDir, string subDirectory = null);
        string ReadFileUTF8(string filePath);
        string CombinePath(params string[] paths);
        string GetDirName(string directoryPath);

        bool IsFolderFound(string folderPath);
        bool IsFileFound(string filePath);
        bool IsFileHidden(string filePath);

        long GetFileSize(string filePath);
        Stream OpenFileStream(string filePath);
        byte[] ReadFileBytes(string filePath);



        bool TryGetDirFiles(string folderPath, string searchPattern, out List<FileShim> fileList, out string errorMsg);




        /*
		 *		Write Operations 
		 * 
		 */
        bool TryCreateDir(string folderPath, out string errorMessage);
        bool TryDeleteFile(string filePath, out string errorMessage);
        bool TryHideFile(string filePath, out string errorMessage);
        bool TryUnhideFile(string filePath, out string errorMessage);
        bool TryWriteFile(string filePath, out string errorMessage, byte[] bytes, bool overwriteExisting = true);
        bool TryWriteFile(string filePath, out string errorMessage, string content, EncodeAs encoding, bool overwriteExisting = true);
        bool TryMoveFile(string oldPath, out string errorMessage, string newPath);
        bool TryCreateNewTempFile(out string newTempFilePath, out string errorMsg);

    }
}
