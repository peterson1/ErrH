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

        public bool HasLifetimeScope => _scopeShim != null;


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


        protected void Register<TInterface1,
                                TInterface2,
                                TImplementation>()
            => Reg(typeof(TInterface1),
                   typeof(TImplementation),
                   false,
                   typeof(TInterface2));

        protected void Register<TInterface, TImplementation>(bool singleton = false)
            where TImplementation : TInterface
            => Reg(typeof(TInterface), typeof(TImplementation), singleton);


        protected void Register<TImplementation>(bool singleton = false)
            => Reg(null, typeof(TImplementation), singleton);




        protected void Singleton<TInterface1, 
                                 TInterface2, 
                                 TImplementation>()
            => Reg(typeof(TInterface1), 
                   typeof(TImplementation),
                   true, 
                   typeof(TInterface2));


        protected void Singleton<TInterface, TImplementation>()
            => Reg(typeof(TInterface), typeof(TImplementation), true);


        protected void Singleton<TImplementation>()
            => Reg(null, typeof(TImplementation), true);




        private void Reg(Type intrfaceTyp1, 
                         Type implementationTyp, 
                         bool isSingleton,
                         Type intrfaceTyp2 = null)
        {
            this._typeDefs.Add(new InstanceDef
            {
                Interface1     = intrfaceTyp1,
                Interface2     = intrfaceTyp2,
                Implementation = implementationTyp,
                IsSingleton    = isSingleton
            });
        }

    }
}
