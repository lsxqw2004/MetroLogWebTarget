using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Galenical.Core.Caching;
using MetroLogWebTarget.Core.Caching;
using MetroLogWebTarget.Data;
using MetroLogWebTarget.Domain;
using MetroLogWebTarget.Service;

namespace MetroLogWebTarget.Web.Framework
{
    public static class DependencyRegistrar
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();
            builder.Register<IDbContext>(c => new MetroLogContext()).InstancePerRequest();
            builder.RegisterGeneric(typeof(SlEfRepository<>)).As(typeof(IRepository<>)).InstancePerRequest();

            builder.RegisterType<LogEnvironmentService>().As<ILogEnvironmentService>().InstancePerRequest();
        }
    }
}