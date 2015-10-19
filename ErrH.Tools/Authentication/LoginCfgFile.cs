using System.ComponentModel;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.Tools.Serialization;

namespace ErrH.Tools.Authentication
{
    public class LoginCfgFile : LogSourceBase, IBasicAuthKeyFile
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private ISerializer      _serialr;
        private IFileSystemShim  _fs;
        private FileShim         _file;
        private LoginCredentials _creds;


        public string UserName
        {
            get { return _creds.UserName; }
            set { _creds.UserName = value; }
        }

        public string Password
        {
            get { return _creds.Password; }
            set { _creds.Password = value; }
        }

        public string BaseUrl
        {
            get { return _creds.BaseUrl; }
            set { _creds.BaseUrl = value; }
        }

        public bool IsCompleteInfo => _creds?.IsCompleteInfo ?? false;



        public LoginCfgFile(IFileSystemShim fileSystemShim, ISerializer serializer)
        {
            _fs      = ForwardLogs(fileSystemShim);
            _serialr = ForwardLogs(serializer);
            _creds   = new LoginCredentials();
        }

        
        public virtual bool ReadFrom(string fileName)
        {
            _creds = ReadAs<LoginCredentials>(fileName);
            return _creds != null;
        }


        protected T ReadAs<T>(string fileName) where T : IBasicAuthenticationKey
        {
            _file  = _fs.File(_fs.GetAssemblyDir().Bslash(fileName));
            if (!_file.Found)
                return Warn_(default(T), $"Missing login file “{fileName}”.", _file.Path);

            var ret = _serialr.Read<T>(_file);

            if (ret == null)
                return Error_(ret, $"Failed to parse ‹{typeof(T).Name}›.", "");

            UserName = ret.UserName;
            Password = ret.Password;
            BaseUrl  = ret.BaseUrl;

            return ret;
        }


        public bool SaveChanges()
        {
            if (_file == null)
                return Error_n("Persist() called while _file == NULL.", 
                               "Please call ReadFrom() before Persist().");

            var s = _serialr.Write(_creds, true);
            if (!_file.Write(s, EncodeAs.UTF8)) return false;
            return _file.Hidden = true;
        }
    }
}
