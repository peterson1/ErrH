using System;

namespace ErrH.Tools.InversionOfControl
{
    public interface ITypeResolver
    {
        T Resolve<T>();
        T Resolve<T, TArg>(TArg constructorArg);

        ILifetimeScopeShim BeginLifetimeScope();

        void EndLifetimeScope();
    }



    public struct InstanceDef
    {
        public Type Interface;
        public Type Implementation;
        public bool IsSingleton;
    }
}
