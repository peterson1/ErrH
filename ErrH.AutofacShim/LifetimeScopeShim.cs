using System;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using ErrH.Tools.Extensions;
using ErrH.Tools.InversionOfControl;

namespace ErrH.AutofacShim
{
    public class LifetimeScopeShim : ILifetimeScopeShim
    {
        private ILifetimeScope _autofacScope;


        public LifetimeScopeShim(TypeResolver typeResolvr)
        {
            this._autofacScope = typeResolvr.BuildContainer()
                                        .BeginLifetimeScope();
        }


        public T Resolve<T>()
        {
            try
            {
                return this._autofacScope.Resolve<T>();
            }
            catch (DependencyResolutionException ex)
            { throw CantResolve<T>(ex); }
        }



        public T Resolve<T, TArg>(TArg constructorArg)
        {
            try
            {
                var arg = new TypedParameter(typeof(TArg), constructorArg);
                return this._autofacScope.Resolve<T>(arg);
            }
            catch (DependencyResolutionException ex)
            { throw CantResolve<T>(ex); }
        }


        private static TypeLoadException CantResolve<T>(DependencyResolutionException ex)
        {
            var e = ex.Message;
            var tName = $"‹{typeof(T).Name}›";
            var d = Dict.onary("Unable to resolve type", tName);

            if (e.Contains("DefaultConstructorFinder' on type '"))
                d.Add("Error in constructor of", e.Between("DefaultConstructorFinder' on type '", "' can be invoked with the available"));

            if (e.Contains("Cannot resolve parameter '"))
                d.Add("Cannot resolve parameter", e.Between("Cannot resolve parameter '", "' of constructor 'Void .ctor("));

            if (ex.InnerException != null)
                d.Add("Inner exception", ex.InnerException.Details(false, false));

            if (ex is ComponentNotRegisteredException)
                d.Add("Component not registered in Type Resolver", tName);

            var s = "IoC Resolver Error";
            foreach (var i in d)
                s += L.F + i.Key + "  :  " + i.Value;

            return new TypeLoadException(s);
        }


        public void Dispose()
        {
            this._autofacScope.Dispose();
        }

    }
}
