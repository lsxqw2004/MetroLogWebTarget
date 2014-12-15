using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using MetroLogWebTarget.Core.DependencyManagement;

namespace MetroLogWebTarget.Core.Infrastructure
{
    public class GalEngine : IEngine
    {
        #region Fields

        private ContainerManager _containerManager;

        #endregion

        #region Ctor

        /// <summary>
		/// Creates an instance of the content engine using default settings and configuration.
		/// </summary>
		public GalEngine() 
            : this(new ContainerConfigurer())
		{
		}

		public GalEngine(ContainerConfigurer configurer)
		{
            InitializeContainer(configurer);
		}
        
        #endregion

        #region Utilities
        
        private void InitializeContainer(ContainerConfigurer configurer)
        {
            var builder = new ContainerBuilder();

            _containerManager = new ContainerManager(builder.Build());
            configurer.Configure(this, _containerManager);
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Initialize components and plugins in the nop environment.
        /// </summary>
        /// <param name="config">Config</param>
        public void Initialize()
        {
            //startup tasks
        }

        public T Resolve<T>() where T : class
		{
            return ContainerManager.Resolve<T>();
		}

        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }
        
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

		#endregion

        #region Properties

        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion
    }
}
