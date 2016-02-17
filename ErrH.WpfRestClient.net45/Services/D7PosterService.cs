using System.Diagnostics;
using System.IO;
using ErrH.Tools.Extensions;
using NLog;

namespace ErrH.WpfRestClient.net45.Services
{
    public class D7PosterService
    {

        private const string SUB_PATH = @"D7Poster\ErrH.D7Poster.WPF.exe";


        internal static void LaunchAsNeeded()
        {
            var logr = LogManager.GetCurrentClassLogger();
            var thisExe = Process.GetCurrentProcess().MainModule.FileName;
            logr.Trace("thisExe= {0}", thisExe);
            var dir = Directory.GetParent(thisExe);
            var d7Exe = Path.Combine(dir.FullName, SUB_PATH);
            if (!File.Exists(d7Exe))
            {
                logr.Warn("Missing: {0}", d7Exe);
                return;
            }

            var noExt = Path.GetFileNameWithoutExtension(d7Exe);
            var procs = Process.GetProcessesByName(noExt);
            if (procs.Length > 0)
            {
                //_logr.Info("Running “{0}” instances: {1}", noExt, procs.Length);
                logr.Info("Found {0} of “{1}”. We won't start another one.", procs.Length.x("running instance"), noExt);
                return;
            }
            var proc = Process.Start(d7Exe, "--stealth-mode");
            logr.Info("“{0}” process started. (pid:{1})", proc?.ProcessName, proc?.Id);
        }
    }
}
