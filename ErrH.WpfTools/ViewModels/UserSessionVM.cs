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
        private ICommand       _loginCmd;
        private ICommand       _logoutCmd;
        private ICommand       _saveSessionCmd;


        public ICommand LoginCmd      => _loginCmd  ?? (_loginCmd = NewLoginCmd());
        public ICommand LogoutCmd     => _logoutCmd ?? (_logoutCmd = NewLogoutCmd());
        public ICommand RememberMeCmd => _saveSessionCmd ?? (_saveSessionCmd = NewRememberMeCmd());

        public bool HasSavedSession   => _client.HasSavedSession;
        public bool IsLoggedIn        => _client.IsLoggedIn;

        public LoginCredentials  Credentials { get; set; }



        public UserSessionVM(ISessionClient sessionClient)
        {
            DisplayName = "not logged in";
            _client = ForwardLogs(sessionClient);
            SetEventHandlers();
        }



        private ICommand NewLoginCmd() => new RelayCommand(
            x => _client.Login(Credentials),
            x => !IsLoggedIn && Credentials != null);

        private ICommand NewLogoutCmd() => new RelayCommand(
            x => _client.Logout(),
            x => IsLoggedIn);

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
