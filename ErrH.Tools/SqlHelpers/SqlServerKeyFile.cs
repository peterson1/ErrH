using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.Tools.Serialization;

namespace ErrH.Tools.SqlHelpers
{
    public class SqlServerKeyFile : LogSourceBase, ISqlServerKey
    {
        private IFileSystemShim _fs;
        private ISerializer     _serialr;

        public bool    IsLoaded     { get; private set; }

        public virtual string  ServerURL    { get; protected set; }
        public virtual string  DatabaseName { get; protected set; }
        public virtual string  UserName     { get; protected set; }
        public virtual string  Password     { get; protected set; }



        public SqlServerKeyFile(IFileSystemShim fsShim, ISerializer serializer)
        {
            _fs      = fsShim;
            _serialr = serializer;
        }


        public virtual bool ReadFromExeDir(string fileName = "SqlServerKey.cfg")
        {
            var fPath = _fs.GetAssemblyDir().Bslash(fileName);
            var file  = _fs.File(fPath);
            if (!file.Found)
            {
                var s = _serialr.Write(new SqlServerKey(), true);
                file.Write(s);
                return Warn_n("Server key file not found. (new one created)", file.Path);
            }

            var obj           = _serialr.Read<SqlServerKey>(file);
            this.ServerURL    = obj.ServerURL;
            this.DatabaseName = obj.DatabaseName;
            this.UserName     = obj.UserName;
            this.Password     = obj.Password;

            return IsLoaded = true;
        }


    }
}
