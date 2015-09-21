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
                if (d.IsSingleton)
                    RegisterSingleton(buildr, d);
                else
                    RegisterNormal(buildr, d);
            }

            buildr.RegisterInstance<ITypeResolver>(this);

            return buildr.Build();
        }


        private void RegisterNormal(ContainerBuilder buildr, InstanceDef d)
        {
            if (d.Interface1 != null)
                buildr.RegisterType(d.Implementation)
                      .As(d.Interface1);
            else
                buildr.RegisterType(d.Implementation);
        }


        private void RegisterSingleton(ContainerBuilder buildr, InstanceDef d)
        {
            if (d.Interface2 != null)
                buildr.RegisterType(d.Implementation)
                      .As(d.Interface1)
                      .As(d.Interface2)
                      .SingleInstance();

            else if (d.Interface1 != null)
                buildr.RegisterType(d.Implementation)
                      .As(d.Interface1)
                      .SingleInstance();

            else if (d.Interface1 == null)
                buildr.RegisterType(d.Implementation)
                      .SingleInstance();
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
