using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using ErrH.Tools.Authentication;
using ErrH.Tools.MvvmPattern;
using ErrH.WinTools.Cryptography;
using ErrH.WinTools.Extensions;
using ErrH.WpfTools.Commands;
using PropertyChanged;

namespace ErrH.WpfTools.ViewModels
{
    [ImplementPropertyChanged]
    public class UserSessionVM : ViewModelBase, ISecureStringConsumer
    {
        private const string NOT_LOGGED_IN = "not logged in";

        private ISessionClient _client;
        private SecureString   _password;

        public IAsyncCommand  LoginCmd          { get; }
        public IAsyncCommand  LogoutCmd         { get; }
        public ICommand       RememberMeCmd     { get; }
        public ICommand       UseCredentialsCmd { get; }
        public string         UserName          { get; set; }
        public string         SignInAs          { get; private set; }

        public IBasicAuthenticationKey AuthKey { get; private set; }

        public bool  IsCompleteInfo  => AuthKey?.IsCompleteInfo ?? false;
        public bool  HasSavedSession => _client.HasSavedSession;
        public bool  IsLoggedIn      => _client?.IsLoggedIn ?? false;



        public UserSessionVM()
        {
            DisplayName = "not logged in";
            SignInAs    = "No credentials found.";

            LoginCmd = AsyncCommand.Create(
                 tkn => _client.Login(AuthKey, tkn),
                   x => !IsLoggedIn && AuthKey != null);

            LogoutCmd = AsyncCommand.Create(
                  tkn => _client.Logout(tkn),
                    x => IsLoggedIn);

            UseCredentialsCmd = new RelayCommand(
                      async x => await UseCredentials(), 
                            x => _client != null);

            RememberMeCmd = new RelayCommand(
                        x => SaveOrDeleteSessionFile(),
                        x => IsLoggedIn);
        }


        public async Task UseCredentials()
        {
            //AuthKey = new LoginCfgFile(Ioc)

            AuthKey = new LoginCredentials
            {
                UserName     = UserName,
                Password = _password.Decrypt(),
                //BaseUrl  = "" dsfsdf asdf asdf asdf
            };
            await LoginCmd.ExecuteAsync(null);
            if (!_client.IsLoggedIn) AuthKey = null;
        }


        public void SetClient(ISessionClient sessionClient, IBasicAuthenticationKey credentials)
        {
            _client  = ForwardLogs(sessionClient);
            AuthKey  = credentials;
            UserName = AuthKey?.UserName;
            SignInAs = $"Sign in as {AuthKey?.UserName}";
            SetEventHandlers();
        }



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


        public void ReceiveKey(SecureString key) => _password = key;




        protected override void OnDispose()
        {
            _password?.Dispose();
            base.OnDispose();
        }
    }
}
