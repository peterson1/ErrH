namespace ErrH.Tools.Authentication
{
    public interface IBasicAuthenticationKey
    {
        string  UserName       { get; }
        string  Password       { get; }
        string  BaseUrl        { get; }
        bool    IsCompleteInfo { get; }
    }
}
