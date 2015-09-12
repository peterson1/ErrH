using System;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.Authentication
{
    public interface ISessionClient : ILogSource
    {
        event EventHandler<UserEventArg> LoggedIn;
        event EventHandler<UserEventArg> LoggedOut;

        string BaseUrl              { get; }
        bool   IsLoggedIn           { get; }
        bool   HasSavedSession      { get; }
        int    RetryIntervalSeconds { get; set; }


        Task<bool> Login(string baseUrl, string userName, string password);
        Task<bool> Login(LoginCredentials credentials);
        Task<bool> Logout();

        void SaveSession();
        void DeleteSavedSession();
        void LoadSession();
    }
}
