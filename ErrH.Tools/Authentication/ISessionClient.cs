using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.Authentication
{
    public interface ISessionClient : ILogSource
    {
        event EventHandler<UserEventArg> LoggedIn;
        event EventHandler<UserEventArg> LoggedOut;

        string BaseUrl                 { get; }
        bool   IsLoggedIn              { get; }
        bool   HasSavedSession         { get; }
        int    RetryIntervalSeconds    { get; set; }
        int    LowRetryIntervalSeconds { get; set; }


        Task<bool> Login(string baseUrl, 
                         string userName, 
                         string password, 
                         CancellationToken cancelToken);

        Task<bool> Login(IBasicAuthenticationKey credentials, 
                         CancellationToken cancelToken);

        Task<bool> Logout(CancellationToken cancelToken);

        void SaveSession();
        void DeleteSavedSession();
        void LoadSession();

        bool LocalizeSessionFile(IBasicAuthenticationKey authKey);
    }
}
