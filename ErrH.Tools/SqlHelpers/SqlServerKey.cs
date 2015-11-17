namespace ErrH.Tools.SqlHelpers
{
    public class SqlServerKey : ISqlServerKey
    {
        public string  ServerURL    { get; set; }
        public string  DatabaseName { get; set; }
        public string  UserName     { get; set; }
        public string  Password     { get; set; }
   }
}
