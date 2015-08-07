using System.Diagnostics;

namespace ErrH.WinTools.ProcessTools
{
    public class Processes
    {
        public static void Kill(string processName)
        {
            foreach (var proc in Process.GetProcessesByName(processName))
            {
                proc.Kill();
            }
        }
    }
}
