namespace ErrH.Tools.SqlHelpers
{
    public interface ISqlServerKey
    {
        string  ServerURL    { get; }
        string  DatabaseName { get; }
        string  UserName     { get; }
        string  Password     { get; }
    }
}
