namespace ErrH.Tools.SqlHelpers
{
    public class ConnStrBuilder
    {
        public static string MsSql2008(string serverUrlOrFilePath, 
                                       string databaseName, 
                                       string userName, 
                                       string password)
            => $"Provider=SQLOLEDB;"
             + $"Data Source={serverUrlOrFilePath};"
             + $"Initial Catalog={databaseName};"
             + $"User ID={userName};"
             + $"Password={password};";


        public static string MsSql2008(ISqlServerKey svrKey)
            => MsSql2008(svrKey.ServerURL, 
                         svrKey.DatabaseName, 
                         svrKey.UserName, 
                         svrKey.Password);
    }
}
