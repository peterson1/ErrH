namespace ErrH.Tools.Authentication
{
    public interface IBasicAuthenticationKey
    {
        string  Name     { get; }
        string  Password { get; }
        string  BaseUrl  { get; }
    }
}
