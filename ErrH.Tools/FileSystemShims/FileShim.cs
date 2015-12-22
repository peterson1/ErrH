using System;
using System.IO;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.FileSystemShims
{
    public class FileShim : LogSourceBase//LogEventRaiser
    {
        private IFileSystemShim _fs;
        private string _dir;



        public FileShim(IFileSystemShim fsShim, string filePath)
        {
            this._fs = fsShim;
            this.Name = this._fs.GetFileNameWithoutPath(filePath);
            this._dir = this._fs.GetParentDir(filePath);
            this.Path = this._dir.Bslash(this.Name);

            this.DefaultLevel = L4j.Trace;
        }


        /// <summary>
        /// Name and extension of the file (without folder path).
        /// </summary>
        public string Name { get; private set; }



        /// <summary>
        /// Complete path with filename + extension.
        /// </summary>
        public string Path { get; private set; }



        public FolderShim Parent
        {
            get
            {
                return this._fs.Folder(this._dir);
            }
        }




        /// <summary>
        /// Attempts to move this file to the target folder.
        /// If folder contains a file with same name, and it's currently in use, we rename the hot file beforehand.
        /// </summary>
        /// <param name="foldr"></param>
        /// <returns>True if operation was successful.</returns>
        public bool HotSwapTo(FolderShim foldr)
        {
            Trace_i("Hotswapping “{0}” to \\{1}\\...", this.Name, foldr.Name);
            string e;
            var destFilePath = _fs.CombinePath(foldr.Path, this.Name);
            if (_fs.TryMoveFile(this.Path, out e, destFilePath))
                return Trace_o(
"Normal Move operation succeeded because target file is not in use.");

            var newLocation = destFilePath + "_hot-swapped";
            if (!_fs.TryMoveFile(destFilePath, out e, newLocation))
                return Error_o(
"Unable to move hot file to temp location." + L.F + e);

            if (!_fs.TryMoveFile(this.Path, out e, destFilePath))
                return Error_o(
"Unable to move file to the original location of the relocated hot file." + L.F + e);

            if (!_fs.TryMoveFile(newLocation, out e, this.Path))
                return Error_o(
"Unable to move relocated hot file to the original location of the source file." + L.F + e);

            return Trace_o("Hot swap successful. Currently in-use: " + this.Path);
        }




        public bool Found
        {
            get
            {
                Trace_i("Checking existence of “{0}”...", this.Name);
                if (
                    _fs.IsFileFound(this.Path)
                )
                    return Trace_o("File exists: " + this.Path);
                else
                    return Warn_o("Missing file: " + this.Path);
            }
        }


        /// <summary>
        /// Silent version of Found method.
        /// </summary>
        public bool _Found => _fs.IsFileFound(Path);
        


        public string ReadUTF8
        {
            get
            {
                Trace_i("Reading from UTF8 file...");
                try
                {
                    return Trace_o_
              (
                  _fs.ReadFileUTF8(this.Path),
                      "Successfully read from UTF8 file.");
                }
                catch (Exception ex)
                {
                    return Error_o_("", "Failed to read from UTF8 file."
                                    + L.F + ex.Details(true, false));
                }
            }
        }


        public string ToBase64
        {
            get
            {
                //Trace_i("Encoding file to Base64...");
                Intro_("Encoding file to Base64...");

                if (!_fs.IsFileFound(this.Path))
                    return
Error_o_("", "File not found: " + this.Path);

                byte[] b; try
                {
                    b = _fs.ReadFileBytes(this.Path);
                }
                catch (Exception ex)
                {
                    return Error_o_("", "Failed to read bytes from file."
                                    + L.F + ex.Details(true, false));
                }

                var b64 = Convert.ToBase64String(b);
                Outro_("Successfully encoded file to Base64.");
                return b64;
            }
        }





        public string Version
            => _fs.GetFileVersion(this.Path) ?? "";


        public Stream Stream
        {
            get
            {
                return this._fs.OpenFileStream(this.Path);
            }
        }


        public string SHA1
        {
            get
            {
                return this.Stream.SHA1();
            }
        }


        public long Size
        {
            get
            {
                return this._fs.GetFileSize(this.Path);
            }
        }


        public string Extension
        {
            get
            {
                return this._fs.GetFileExtension(this.Path);
            }
        }


        public string LessExtension
        {
            get
            {
                return this._fs.GetFileNameWithoutExtension(this.Path);
            }
        }


        public bool MoveTo(string targetPath)
        {
            string e; Trace_i("Moving file to new location...");
            if (
                _fs.TryMoveFile(this.Path, out e, targetPath)
            )
                return Trace_o("File successfully moved to new location.");
            else
                return Error_o("Failed to move file." + L.F + e);
        }


        public bool Hidden
        {
            get { return _fs.IsFileHidden(this.Path); }
            set
            {
                Trace_i("Setting Hidden attribute to: " + value);
                string e;
                var b = value ? _fs.TryHideFile(this.Path, out e)
                              : _fs.TryUnhideFile(this.Path, out e);

                if (b) Trace_o("Successfully set Hidden attribute of file.");
                else Error_o("Failed to set Hidden attribute of file.");
            }
        }



        public bool Write(byte[] bytes, bool overwriteExisting = true)
        {
            string e; Trace_i("Writing bytes to file...");
            if (
                _fs.TryWriteFile(this.Path, out e, bytes, overwriteExisting)
            )
                return Trace_o("Successfully written bytes to file.");
            else
                return Error_o("Failed to write bytes to file." + L.F + e);
        }


        public bool Write(string content,
                          EncodeAs encoding = EncodeAs.UTF8,
                          bool overwriteExisting = true)
        {
            if (_Found && overwriteExisting)
                if (!Delete()) return false;

            string e; Trace_i("Writing text to file as {0}...", encoding);
            bool ok;

            try {
                ok = _fs.TryWriteFile(this.Path, out e, content, encoding, overwriteExisting);
            }
            catch (Exception ex) { return LogError("_fs.TryWriteFile", ex); }

            if (ok)
                return Trace_o("Successfully written text to file.");
            else
                return Error_o("Failed to write text to file." + L.F + e);
        }


        public bool Delete()
        {
            string e; Trace_i($"Deleting {Name}...");

            if (!_Found) return 
                Debug_o("No need to delete non-existent file.");

            if (!_fs.TryDeleteFile(Path, out e))
                return Error_o("Failed to delete file." + L.F + e);
            
            if (_Found)
                return Error_o("_fs.TryDeleteFile() returned success but file is still there.");

            return Trace_o("Successfully deleted file.");
        }

    }
}
