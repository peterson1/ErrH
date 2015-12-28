using System;
using System.Collections.Generic;
using System.Linq;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.FileSystemShims
{
    public class FolderShim : LogSourceBase
    {
        private IFileSystemShim _fs;



        public FolderShim(IFileSystemShim fsShim, string folderPath)
        {
            this._fs = fsShim;
            this.Path = folderPath;
        }



        public string Path { get; private set; }


        public string Name { get { return this._fs.GetDirName(this.Path); } }

        public FileShim File(string searchPattern, bool expect1Match = true)
        {
            if (!expect1Match)
                return _fs.File(Path.Bslash(searchPattern));

            Trace_i("Finding file: “{0}”...", searchPattern);
            string e; List<FileShim> ff; FileShim f = null;
            if (!
                _fs.TryGetDirFiles(this.Path, searchPattern, out ff, out e)
            )
                return Error_o_(f, "Error in finding file." + L.F + e);

            if (ff == null) return Error_o_(f, "Didn't expect files list to be NULL.");
            if (ff.Count > 1) return Error_o_(f, "{0:file} matched the search pattern.", ff.Count);
            if (ff.Count == 0) return Warn_o_(f, "File not found.");
            return Trace_o_(ff.FirstOrDefault(), "File found in folder: " + this.Path);
        }


        public bool Found
        {
            get
            {
                Trace_i("Looking for folder...");
                if (
                    _fs.IsFolderFound(this.Path)
                )
                    return Trace_o("Folder found: " + this.Path);
                else
                    return Warn_o($"Missing folder: “{Path}”");
            }
        }


        /// <summary>
        /// Checks if file exists in this folder.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Has(string fileName)
        {
            return _fs.IsFileFound(Append(fileName));
        }



        private string Append(string path)
        {
            return _fs.CombinePath(this.Path, path);
        }


        public FolderShim SubFolder(string subFolderName)
        {
            return _fs.Folder(Append(subFolderName));
        }


        public FolderShim Parent
        {
            get
            {
                return _fs.Folder(_fs.GetParentDir(this.Path));
            }
        }



        public bool Create()
        {
            if (this.Found) return true;

            string e; Trace_i("Creating new folder...");
            if (
                _fs.TryCreateDir(this.Path, out e)
            )
                return Trace_o("Successfully created new folder.");
            else
                return Error_o("Failed to create new folder." + L.F + e);
        }



        public List<FileShim> Files(string searchPattern = "*.*")
        {
            //var pattrn = "*.*";
            string e; List<FileShim> ff;

            Trace_i(@"Finding {0} in \{1}\", searchPattern, this.Path.TruncateStart(23, "~"));
            if (!
                _fs.TryGetDirFiles(this.Path, searchPattern, out ff, out e)
            )
                Error_o("Error in getting list of files." + L.F + e);

            if (ff == null) ff = new List<FileShim>();
            if (ff.Count == 0) Warn_o("No files found.");
            else Trace_o("Found {0:file}.", ff.Count);
            return ff;
        }


        public bool Delete(bool recursive = true)
        {
            bool deleted; string err;
            try {
                deleted = _fs.TryDeleteDir(this.Path, recursive, out err);
            }
            catch (Exception ex){ return LogError("_fs.DeleteDir", ex); }

            if (!deleted)
                Warn_n("Failed to delete directory", this.Path + L.f + err);

            return deleted;
        }
    }
}
