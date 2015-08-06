using System;

namespace ErrH.Tools.InversionOfControl
{
    public interface ITypeResolver
    {
        T Get<T>();

        ILifetimeScopeShim BeginLifetimeScope();
    }



    public struct InstanceDef
    {
        public Type Interface;
        public Type Implementation;
        public bool IsSingleton;
    }
}
