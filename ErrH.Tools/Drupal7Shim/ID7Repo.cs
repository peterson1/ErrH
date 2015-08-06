using System;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.Drupal7Shim
{
    public interface ID7Repo : ILogSource, IDisposable
    {
        Task<bool> Connect(string baseUrl,
                           string username,
                           string password,
                           string resource);

        Task<bool> RefreshData(params object[] queryParams);

        Task<bool> Disconnect();

        bool IsConnected { get; }
    }
}
