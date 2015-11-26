namespace ErrH.Tools.SqlHelpers
{
    public class HardCodedServerKeyFile : SqlServerKeyFile
    {

        public HardCodedServerKeyFile(string serverUrlOrFilePath = "",
                                      string databaseName = "",
                                      string userName = "",
                                      string password = "") 
            : base(null, null)
        {
            ServerURL    = serverUrlOrFilePath;
            DatabaseName = databaseName;
            UserName     = userName;
            Password     = password;
        }

        public override bool ReadFromExeDir(string fileName) => true;
    }
}
