using System;
using System.Collections.Generic;
using System.Linq;
using MetroLogWebTarget.Core.Infrastructure;

namespace MetroLogWebTarget.Core.DependencyManagement
{
    /// <summary>
    /// Configures the inversion of control container with services used by Nop.
    /// </summary>
    public class ContainerConfigurer
    {
        public virtual void Configure(IEngine engine, ContainerManager containerManager)
        {
            //other dependencies
            containerManager.AddComponentInstance<IEngine>(engine, "core.engine");
            containerManager.AddComponentInstance<ContainerConfigurer>(this, "core.containerConfigurer");

            //type finder
            containerManager.AddComponent<ITypeFinder, WebAppTypeFinder>("core.typeFinder");

            //register dependencies provided by other assemblies
            var typeFinder = containerManager.Resolve<ITypeFinder>();
            containerManager.UpdateContainer(x =>
            {
                var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
                var drInstances = new List<IDependencyRegistrar>();
                foreach (var drType in drTypes)
                    drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
                //sort
                drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
                foreach (var dependencyRegistrar in drInstances)
                    dependencyRegistrar.Register(x, typeFinder);
            });
        }
    }
}
