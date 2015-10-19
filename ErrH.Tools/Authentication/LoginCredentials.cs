using System.ComponentModel;
using ErrH.Tools.Extensions;
using PropertyChanged;

namespace ErrH.Tools.Authentication
{
    [ImplementPropertyChanged]
    public class LoginCredentials : IBasicAuthenticationKey
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public string  UserName             { get; set; }
        public string  Password             { get; set; }
        public string  BaseUrl              { get; set; }
                                            
        public bool    ValidSSL             { get; set; }
        public int     RetryIntervalSeconds { get; set; }

        public bool IsCompleteInfo => !UserName.IsBlank()
                                   && !Password.IsBlank()
                                   && !BaseUrl .IsBlank();
    }
}
