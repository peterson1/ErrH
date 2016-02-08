using System;
using System.IO;
using System.Linq;
using ErrH.Tools.Extensions;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Wpf.net45.Extensions;

namespace ErrH.Wpf.net45.BinUpdater
{
    public class AppFilesDTO
    {
        public event EventHandler<EArg<string>> FileDiffered;

        public string  app_nid_vid       { get; set; }
        public int     app_nid           { get; set; }
        public int     app_vid           { get; set; }
        public string  app_title         { get; set; }
        public string  app_users         { get; set; }

        public string  app_file_nid_vid  { get; set; }
        public int?    app_file_nid      { get; set; }
        public int?    app_file_vid      { get; set; }
        public string  app_file_name     { get; set; }
        public string  app_file_version  { get; set; }
        public long?   app_file_size     { get; set; }
        public string  app_file_sha1     { get; set; }
        public int?    app_file_fid      { get; set; }


        public string LocalPath => FindLocalFile().FullName;

        public FileInfo FindLocalFile()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            return new FileInfo(Path.Combine(dir, app_file_name));
        }


        public bool IsSame()
        {
            var file = FindLocalFile();
            if (!file.Exists)                       return NotFound();
            if (file.Length    != app_file_size)    return DiffSize(file);
            var ver = file.Version();
            if (file.Version() != app_file_version) return DiffVersion(file);
            if (file.SHA1()    != app_file_sha1)    return false;

            return true;
        }

        private bool NotFound()
            => RaiseDiff($"“{app_file_name}” not found in local dir.");

        private bool DiffSize(FileInfo file)
            => RaiseDiff($"“{app_file_name}” size diff :  {app_file_size.Value.KB()} ↑  vs  {file.Length} ↓");

        private bool DiffVersion(FileInfo file)
            => RaiseDiff($"“{app_file_name}” version diff :  {app_file_version} ↑  vs  {file.Version()} ↓");

        private bool DiffHash(FileInfo file)
            => RaiseDiff($"“{app_file_name}” SHA1 diff :  {app_file_sha1} ↑  vs  {file.SHA1()} ↓");


        private bool RaiseDiff(string msg)
        {
            FileDiffered?.Invoke(this, new EArg<string> { Value = msg });
            return false;
        }

        //private bool Diff(string label, object )
        //{
        //    var msg = $"{app_file_name} diff ‹{label}› : ";
        //    FileDiffered?.Invoke(this, new EArg<string> { Value = msg });
        //    return false;
        //}
    }
}
