using System.Diagnostics;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.WinTools.FileSystemTools;

namespace ErrH.WinTools.ProcessTools
{
    public class BatchFileShim : LogSourceBase
    {
        public FileShim File { get; private set; }



        public void Run(string batchFilePath)
        {
            File = ForwardLogs(new WindowsFsShim()
                                .File(batchFilePath));
            ExecuteCmd(File);
        }


        public void RunAgain() => ExecuteCmd(File);


        private void ExecuteCmd(FileShim cmdFile)
        {
            Info_n("Running command from file...", cmdFile.Name);
            if (!cmdFile.Found) return;

            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + File.Found);

            processInfo.WorkingDirectory       = cmdFile.Parent.Path;
            processInfo.CreateNoWindow         = true;
            processInfo.UseShellExecute        = false;
            processInfo.RedirectStandardError  = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);
            //todo: test if this works on AcctgPC, even without line below
            //process.WaitForExit();

            Info_n("Process started.", "");
        }
    }
}
