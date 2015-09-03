namespace ErrH.Uploader.Core.Configuration
{
    public interface IRemoteSettings
    {

        string Server { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        int AppNid { get; set; }
        bool ValidSSL { get; set; }

        /// <summary>
        /// Minutes interval between update checks.
        /// </summary>
        int Interval { get; set; }

    }
}
