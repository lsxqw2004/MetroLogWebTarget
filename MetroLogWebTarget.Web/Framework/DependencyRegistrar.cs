using Autofac;
using MetroLogWebTarget.Core.Caching;
using MetroLogWebTarget.Data;
using MetroLogWebTarget.Domain;
using MetroLogWebTarget.Service;
using Microsoft.AspNet.Identity;

namespace MetroLogWebTarget.Web.Framework
{
    public static class DependencyRegistrar
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();
            builder.Register<IDbContext>(c => new MetroLogDbContext()).InstancePerRequest();
            builder.RegisterGeneric(typeof(SlEfRepository<>)).As(typeof(IRepository<>)).InstancePerRequest();

            builder.RegisterType<UserStore<User>>().As<IUserStore<User, int>>().InstancePerRequest();
            builder.RegisterType(typeof(UserManager<User, int>)).AsSelf().InstancePerRequest();
            builder.RegisterType<LogEnvironmentService>().As<ILogEnvironmentService>().InstancePerRequest();
        }
    }
}