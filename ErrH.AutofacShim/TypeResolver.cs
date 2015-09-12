using System;
using Autofac;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.InversionOfControl;

namespace ErrH.AutofacShim
{
    public abstract class TypeResolver : TypeResolverBase
    {
        internal IContainer BuildContainer()
        {
            this.RegisterTypes();

            var buildr = new ContainerBuilder();
            foreach (var d in this._typeDefs)
            {
                //later: refactor 2 methods out of these

                if (d.Interface2 != null && d.IsSingleton)
                    buildr.RegisterType(d.Implementation)
                        .As(d.Interface1)
                        .As(d.Interface2).SingleInstance();

                else if (d.Interface1 != null && d.IsSingleton)
                    buildr.RegisterType(d.Implementation).As(d.Interface1).SingleInstance();

                else if (d.Interface1 == null && d.IsSingleton)
                    buildr.RegisterType(d.Implementation).SingleInstance();

                else if (d.Interface1 != null && !d.IsSingleton)
                    buildr.RegisterType(d.Implementation).As(d.Interface1);

                else if (d.Interface1 == null && !d.IsSingleton)
                    buildr.RegisterType(d.Implementation);

                //else if (d.Instance != null && d.IsSingleton)
                //    buildr.RegisterInstance(d.Instance);
            }

            buildr.RegisterInstance<ITypeResolver>(this);

            return buildr.Build();
        }



        public override ILifetimeScopeShim BeginLifetimeScope()
        {
            if (_scopeShim != null)
                Throw.BadAct("A lifetime scope was already started.");

            return _scopeShim = new LifetimeScopeShim(this);
        }


        public override void EndLifetimeScope()
        {
            if (_scopeShim != null)
                _scopeShim.Dispose();
        }

    }
}
