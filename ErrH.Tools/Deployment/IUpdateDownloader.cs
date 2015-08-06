using System;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.Deployment
{
    public interface IUpdateDownloader : ILogSource
    {
        /// <summary>
        /// Raised when config file says that SSL ain't valid.
        /// </summary>
        event EventHandler<UrlEventArg> CertSelfSigned;



        /// <summary>
        /// Starts to check for updates using the interval defined in config file.
        /// </summary>
        /// <param name="targetDir"></param>
        /// <returns></returns>
        Task StartChecking(string targetDir);



        /// <summary>
        /// Pauses or resumes checking for updates.
        /// </summary>
        void ToggleChecking();
    }
}
