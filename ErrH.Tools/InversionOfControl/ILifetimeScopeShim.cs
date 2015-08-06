using System;

namespace ErrH.Tools.InversionOfControl
{
    public interface ILifetimeScopeShim : IDisposable
    {
        T Get<T>();
    }
}
