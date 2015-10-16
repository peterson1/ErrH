using System.Diagnostics;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;

namespace ErrH.WinTools.ProcessTools
{
    public class BatchFileShim : LogSourceBase
    {
        private IFileSystemShim _fs;

        public FileShim File { get; private set; }


        public BatchFileShim(IFileSystemShim fileSystemShim)
        {
            _fs = ForwardLogs(fileSystemShim);
        }


        public void Run(string batchFilePath)
        {
            File = _fs.File(batchFilePath);

            // if it's just a file name,
            //  - look for it beside the folder
            if (!File.Found)
            {
                var path = _fs.GetAssemblyDir().Bslash(batchFilePath);
                File = _fs.File(path);
            }

            ExecuteCmd(File);
        }


        public void RunAgain() => ExecuteCmd(File);


        private void ExecuteCmd(FileShim cmdFile)
        {
            Info_n("Running command from file...", cmdFile.Name);
            if (!cmdFile.Found)
            {
                Warn_n("Batch file not found: " + cmdFile.Name, cmdFile.Path);
                return;
            }

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c " + cmdFile.Path;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            
            Info_n("Standard Output:", p.StandardOutput.ReadToEnd());
            Warn_n("Standard Error:", p.StandardError.ReadToEnd());

            p.WaitForExit();
        }
    }
}
