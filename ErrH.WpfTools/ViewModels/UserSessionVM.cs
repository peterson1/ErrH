using System.Windows.Input;
using ErrH.Tools.Authentication;
using ErrH.Tools.MvvmPattern;
using ErrH.WpfTools.Commands;

namespace ErrH.WpfTools.ViewModels
{
    public class UserSessionVM : ViewModelBase
    {
        private const string NOT_LOGGED_IN = "not logged in";

        private ISessionClient _client;
        private IAsyncCommand  _loginCmd;
        private IAsyncCommand  _logoutCmd;
        private ICommand       _saveSessionCmd;


        public IAsyncCommand LoginCmd      => _loginCmd  ?? (_loginCmd = NewLoginCmd());
        public IAsyncCommand LogoutCmd     => _logoutCmd ?? (_logoutCmd = NewLogoutCmd());
        public ICommand      RememberMeCmd => _saveSessionCmd ?? (_saveSessionCmd = NewRememberMeCmd());

        public bool HasSavedSession => _client.HasSavedSession;
        public bool IsLoggedIn      => _client?.IsLoggedIn ?? false;

        public LoginCredentials  Credentials { get; set; }



        public UserSessionVM()
        {
            DisplayName = "not logged in";
        }


        public void SetClient(ISessionClient sessionClient)
        {
            _client = ForwardLogs(sessionClient);
            SetEventHandlers();
        }


        //private ICommand NewLoginCmd() => new RelayCommand(
        //    x => _client.Login(Credentials, cancelToken),
        //    x => !IsLoggedIn && Credentials != null);

        private IAsyncCommand NewLoginCmd()
            => AsyncCommand.Create(tkn => _client.Login(Credentials, tkn));


        //private ICommand NewLogoutCmd() => new RelayCommand(
        //    x => _client.Logout(cancelToken),
        //    x => IsLoggedIn);

        private IAsyncCommand NewLogoutCmd()
            => AsyncCommand.Create(tkn => _client.Logout(tkn));


        private ICommand NewRememberMeCmd() => new RelayCommand(
            x => SaveOrDeleteSessionFile(),
            x => IsLoggedIn);

        private void SaveOrDeleteSessionFile()
        {
            if (HasSavedSession)
                _client.DeleteSavedSession();
            else
                _client.SaveSession();

            RaisePropertyChanged(nameof(HasSavedSession));
        }






        private void SetEventHandlers()
        {
            _client.LoggedIn += (s, e) =>
            {
                DisplayName = $"Hi {e.Name}!";
                RaisePropertyChanged(nameof(IsLoggedIn));
                RaisePropertyChanged(nameof(HasSavedSession));
            };

            _client.LoggedOut += (s, e) =>
            {
                DisplayName = NOT_LOGGED_IN;
                RaisePropertyChanged(nameof(IsLoggedIn));
                RaisePropertyChanged(nameof(HasSavedSession));
            };
        }


    }
}
