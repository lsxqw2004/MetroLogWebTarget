using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MetroLogWebTarget.Core.Infrastructure
{
    /// <summary>
    ///     提供对引擎单例对象的访问
    /// </summary>
    public class EngineContext
    {
        #region Initialization Methods

        /// <summary>h初始化引擎的静态实例</summary>
        /// <param name="forceRecreate">不管对象是否存在，都强制创建新的实例</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                Singleton<IEngine>.Instance = CreateEngineInstance();
                Singleton<IEngine>.Instance.Initialize();
            }
            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        ///     Sets the static engine instance to the supplied engine. Use this method to supply your own engine
        ///     implementation.
        /// </summary>
        /// <param name="engine">The engine to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        /// <summary>
        ///     创建引擎实例，可以通过配置文件方式实现注入
        /// </summary>
        /// <returns>引擎实例</returns>
        public static IEngine CreateEngineInstance()
        {

            //默认应用引擎类
            return new GalEngine();
        }

        #endregion

        /// <summary>访问引擎单例对象，通过它可以访问系统中的各种服务</summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<IEngine>.Instance;
            }
        }
    }
}