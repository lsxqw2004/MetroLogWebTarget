using Autofac;
using MetroLogWebTarget.Core.Infrastructure;

namespace MetroLogWebTarget.Core.DependencyManagement
{
    public interface IDependencyRegistrar
    {
        void Register(ContainerBuilder builder, ITypeFinder typeFinder);

        int Order { get; }

    }
}
