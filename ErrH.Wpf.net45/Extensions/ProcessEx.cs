using System.Diagnostics;
using System.IO;
using ErrH.Tools.Extensions;

namespace ErrH.Wpf.net45.Extensions
{
    public static class ProcessEx
    {
        public static void KillOthersThenStart(string exePath, string arguments = null)
        {
            if (!File.Exists(exePath)) return;

            var noExt = Path.GetFileNameWithoutExtension(exePath);
            var procs = Process.GetProcessesByName(noExt);
            foreach (var p in procs) p.Kill();

            if (arguments.IsBlank())
                Process.Start(exePath);
            else
                Process.Start(exePath, arguments);

        }
    }
}
