using ErrH.Tools.Extensions;

namespace ErrH.Tools.Authentication
{
    public class LoginCredentials : IBasicAuthenticationKey
    {
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
