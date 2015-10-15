namespace ErrH.Tools.Authentication
{
    public class LoginCredentials : IBasicAuthenticationKey
    {
        /// <summary>
        /// Login name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Login password of the user.
        /// </summary>
        public string Password { get; set; }


        public string BaseUrl { get; set; }
        public bool ValidSSL { get; set; }

        public int RetryIntervalSeconds { get; set; }
    }
}
