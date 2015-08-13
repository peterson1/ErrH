using System;
using System.Collections.Generic;
using ErrH.Tools.ErrorConstructors;

namespace ErrH.Tools.InversionOfControl
{
    public abstract class TypeResolverBase : ITypeResolver
    {
        protected List<InstanceDef> _typeDefs = new List<InstanceDef>();
        protected ILifetimeScopeShim _scopeShim;


        protected abstract void RegisterTypes();

        public abstract ILifetimeScopeShim BeginLifetimeScope();
        public abstract void EndLifetimeScope();


        public T Resolve<T>()
        {
            if (_scopeShim == null)
                Throw.BadAct("Call Resolv.r.BeginLifetimeScope() before resolving anything.");

            return _scopeShim.Resolve<T>();
        }


        public T Resolve<T, TArg>(TArg constructorArg)
        {
            if (_scopeShim == null)
                Throw.BadAct("Call Resolv.r.BeginLifetimeScope() before resolving anything.");

            return _scopeShim.Resolve<T, TArg>(constructorArg);
        }


        protected void Register<TInterface, TImplementation>(bool singleton = false)
        {
            Reg(typeof(TInterface), typeof(TImplementation), singleton);
        }

        protected void Register<TImplementation>(bool singleton = false)
        {
            Reg(null, typeof(TImplementation), singleton);
        }

        protected void Singleton<TInterface, TImplementation>(bool singleton = true)
        {
            Reg(typeof(TInterface), typeof(TImplementation), singleton);
        }

        protected void Singleton<TImplementation>(bool singleton = true)
        {
            Reg(null, typeof(TImplementation), singleton);
        }


        private void Reg(Type intrface, Type implementation, bool singleton)
        {
            this._typeDefs.Add(new InstanceDef
            {
                Interface = intrface,
                Implementation = implementation,
                IsSingleton = singleton
            });
        }

    }
}
