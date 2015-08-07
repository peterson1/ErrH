﻿using System;
using Autofac;
using Autofac.Core;
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


        public T Get<T>()
        {
            try
            {
                return this._autofacScope.Resolve<T>();
            }
            catch (DependencyResolutionException ex)
            { throw CantResolve<T>(ex); }
        }


        private static TypeLoadException CantResolve<T>(DependencyResolutionException ex)
        {
            var e = ex.Message;
            var d = Dict.onary("Unable to resolve type", typeof(T).Name);

            if (e.Contains("DefaultConstructorFinder' on type '"))
                d.Add("Error in constructor of", e.Between("DefaultConstructorFinder' on type '", "' can be invoked with the available"));

            if (e.Contains("Cannot resolve parameter '"))
                d.Add("Cannot resolve parameter", e.Between("Cannot resolve parameter '", "' of constructor 'Void .ctor("));

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