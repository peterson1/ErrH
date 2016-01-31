using ErrH.Tools.Loggers;

namespace ErrH.Tools.InversionOfControl
{
    public interface IWindowFactory : ILogSource
    {
        T ShowNew<T>() where T : ILogSource;
    }
}
