using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.Tools.Serialization;

namespace ErrH.Tools.Authentication
{
    public class LoginCfgFile : LogSourceBase, IBasicAuthKeyFile
    {
        public const string Filename = "Login.cfg";

        private ISerializer             _serialr;
        private IBasicAuthenticationKey _key;


        public string  UserName       => _key?.UserName;
        public string  Password       => _key?.Password;
        public string  BaseUrl        => _key?.BaseUrl;
        public bool    IsCompleteInfo => _key?.IsCompleteInfo ?? false;
                         
        public FileShim  File  { get; private set; }



        public LoginCfgFile(ISerializer serializer)
        {
            _serialr = ForwardLogs(serializer);
        }


        public IBasicAuthenticationKey ReadFrom(FolderShim folder)
        {
            _key = null;
            File = folder.File(Filename, false);
            if (!File.Found)
                return Warn_(_key, "Missing Login Config file.", File.Path);

            return _key = _serialr.Read<LoginCredentials>(File);
        }


        public bool SaveTo(FolderShim folder, string baseUrl, string userName)
        {
            var obj = new LoginCredentials
            {
                BaseUrl = baseUrl,
                UserName = userName,
            };
            var s = _serialr.Write(obj, true);
            File  = folder.File(Filename, false);
            return File.Write(s, EncodeAs.UTF8);
        }
    }
}
