using ErrH.Tools.Loggers;

namespace ErrH.Tools.Authentication
{
    public interface IBasicAuthKeyFile : IBasicAuthenticationKey, ILogSource
    {
        new string  UserName   { get; set; }
        new string  Password   { get; set; }
        new string  BaseUrl    { get; set; }

        bool  ReadFrom    (string fileName);
        //bool  ReadFrom <T>(string fileName) where T : IBasicAuthenticationKey;
        bool  SaveChanges ();
    }
}
