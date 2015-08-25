using System;
using MetroLogWebTarget.Core.DependencyManagement;

namespace MetroLogWebTarget.Core.Infrastructure
{
    public interface IEngine
    {
        ContainerManager ContainerManager { get; }
        
        void Initialize();

        T Resolve<T>() where T : class;

        object Resolve(Type type);

        T[] ResolveAll<T>();
    }
}
