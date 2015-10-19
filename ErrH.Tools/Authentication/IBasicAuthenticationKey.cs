using System.ComponentModel;

namespace ErrH.Tools.Authentication
{
    public interface IBasicAuthenticationKey : INotifyPropertyChanged
    {
        string  UserName       { get; }
        string  Password       { get; }
        string  BaseUrl        { get; }
        bool    IsCompleteInfo { get; }
    }
}
