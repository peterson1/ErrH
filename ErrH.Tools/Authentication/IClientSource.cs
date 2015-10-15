namespace ErrH.Tools.Authentication
{
    public interface IClientSource
    {
        ISessionClient           Client  { get; }
        IBasicAuthenticationKey  AuthKey { get; }
    }
}
