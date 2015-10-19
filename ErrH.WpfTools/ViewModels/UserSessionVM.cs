using System.Security;
using System.Windows.Input;
using ErrH.Tools.Authentication;
using ErrH.Tools.Extensions;
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

        public IAsyncCommand      LoginCmd          { get; }
        public IAsyncCommand      LogoutCmd         { get; }
        public ICommand           RememberMeCmd     { get; }
        public IBasicAuthKeyFile  AuthFile          { get; }


        public bool    HasSavedSession => _client?.HasSavedSession ?? false;
        public bool    IsLoggedIn      => _client?.IsLoggedIn ?? false;
        public bool    AskForInput     => GetAskForInput();
        public string  SignInAs        => SignInAsLabel();
        public string UserName
        {
            get { return AuthFile.UserName; }
            set { AuthFile.UserName = value; }
        }
        public string Password
        {
            get { return AuthFile.Password; }
            set { AuthFile.Password = value; }
        }
        public string BaseUrl
        {
            get { return AuthFile.BaseUrl; }
            set { AuthFile.BaseUrl = value; }
        }



        public UserSessionVM(IBasicAuthKeyFile authKeyFile)
        {
            DisplayName = NOT_LOGGED_IN;
            AuthFile    = ForwardLogs(authKeyFile);

            LoginCmd = AsyncCommand.Create(
                 tkn => _client.Login(AuthFile, tkn),
                   x => !IsLoggedIn && _client != null);

            LogoutCmd = AsyncCommand.Create(
                  tkn => _client.Logout(tkn),
                    x => IsLoggedIn);

            RememberMeCmd = new RelayCommand(
                        x => SaveOrDeleteSessionFile(),
                        x => IsLoggedIn);
        }



        public void SetClient(ISessionClient sessionClient)
        {
            _client = ForwardLogs(sessionClient);
            RaisePropertyChanged(nameof(BaseUrl));
            RaisePropertyChanged(nameof(UserName));
            RaisePropertyChanged(nameof(Password));
            RaisePropertyChanged(nameof(AskForInput));
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
            //AuthFile.PropertyChanged += (s, e) =>
            //{
            //    RaisePropertyChanged(nameof(SignInAs));
            //    RaisePropertyChanged(nameof(IsCompleteInfo));
            //};

            _client.LoggedIn += (s, e) =>
            {
                DisplayName = $"Hi {e.Name}!";
                RaisePropertyChanged(nameof(IsLoggedIn));
                RaisePropertyChanged(nameof(HasSavedSession));
                RaisePropertyChanged(nameof(AskForInput));
                RaisePropertyChanged(nameof(SignInAs));
            };

            _client.LoggedOut += (s, e) =>
            {
                DisplayName = NOT_LOGGED_IN;
                RaisePropertyChanged(nameof(IsLoggedIn));
                RaisePropertyChanged(nameof(HasSavedSession));
                RaisePropertyChanged(nameof(SignInAs));
            };
        }


        private bool GetAskForInput()
        {
            if (IsLoggedIn) return false;
            if (AuthFile == null) return true;
            return !AuthFile.IsCompleteInfo;
        }

        private string SignInAsLabel()
        {
            if (UserName.IsBlank()) return "No credentials found.";
            return IsLoggedIn ? $"Logged in as “{UserName}”"
                              : $"Sign in as “{UserName}”";
        }


        //todo: do not store decrypted in memory
        public void ReceiveKey(SecureString key) 
            => Password = key.Decrypt();


        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
