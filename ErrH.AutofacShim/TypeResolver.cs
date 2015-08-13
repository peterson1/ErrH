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
                if (d.Interface != null && d.IsSingleton)
                    buildr.RegisterType(d.Implementation).As(d.Interface).SingleInstance();

                else if (d.Interface == null && d.IsSingleton)
                    buildr.RegisterType(d.Implementation).SingleInstance();

                else if (d.Interface != null && !d.IsSingleton)
                    buildr.RegisterType(d.Implementation).As(d.Interface);

                else if (d.Interface == null && !d.IsSingleton)
                    buildr.RegisterType(d.Implementation);
            }
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
