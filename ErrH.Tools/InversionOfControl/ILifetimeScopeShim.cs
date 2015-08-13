using System;

namespace ErrH.Tools.InversionOfControl
{
    public interface ILifetimeScopeShim : IDisposable
    {
        T Resolve<T>();
        T Resolve<T, TArg>(TArg constructorArg);
    }
}
