#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

#endregion

namespace MetroLogWebTarget.Core.Infrastructure
{
    /// <summary>
    ///     本类在当前执行的AppDomain所加载的程序集中查找所需的类型
    ///     只有名称匹配给定规则的程序集会被查找
    ///     <see cref="AssemblyNames" />属性中列出的程序集一定会被查找，该列表是可选的
    /// </summary>
    public class AppDomainTypeFinder : ITypeFinder
    {
        #region Fields

        private bool ignoreReflectionErrors = true;
        private bool loadAppDomainAssemblies = true;

        private string assemblySkipLoadingPattern =
            "^System|^mscorlib|^Microsoft|^CppCodeProvider|^VJSharpCodeProvider|^WebDev|^Castle|^Iesi|^log4net|^NHibernate|^nunit|^TestDriven|^MbUnit|^Rhino|^QuickGraph|^TestFu|^Telerik|^ComponentArt|^MvcContrib|^AjaxControlToolkit|^Antlr3|^Remotion|^Recaptcha";

        private string assemblyRestrictToLoadingPattern = ".*";
        private IList<string> assemblyNames = new List<string>();

        /// <summary>
        ///     缓存有特性的程序集信息，这样它们不需要重新读取
        /// </summary>
        private readonly List<AttributedAssembly> _attributedAssemblies = new List<AttributedAssembly>();

        /// <summary>
        ///     缓存已经查过过的程序集特性
        /// </summary>
        private readonly List<Type> _assemblyAttributesSearched = new List<Type>();

        #endregion

        #region Ctor

        #endregion

        #region Properties

        /// <summary>在此AppDomain中查找类型</summary>
        public virtual AppDomain App
        {
            get { return AppDomain.CurrentDomain; }
        }

        /// <summary>当加载应用的类型时，是否遍历AppDomain中的程序集。当加载这些程序集时应用加载规则</summary>
        public bool LoadAppDomainAssemblies
        {
            get { return loadAppDomainAssemblies; }
            set { loadAppDomainAssemblies = value; }
        }

        /// <summary>除了在AppDomain中加载的程序集之外的那些在启动时加载的程序集</summary>
        public IList<string> AssemblyNames
        {
            get { return assemblyNames; }
            set { assemblyNames = value; }
        }

        /// <summary>需要跳过类型查找的dll的名称所匹配规则</summary>
        public string AssemblySkipLoadingPattern
        {
            get { return assemblySkipLoadingPattern; }
            set { assemblySkipLoadingPattern = value; }
        }

        /// <summary>
        ///     要被查找的dll的名称规则。为了方便此属性默认被设置为匹配全部，为了提高性能you might want to configure a pattern that includes assemblies and your
        ///     own.
        /// </summary>
        /// <remarks>
        ///     If you change this so that LCM assemblies arn't investigated (e.g. by not including something like "^LCM|..."
        ///     you may break core functionality.
        /// </remarks>
        public string AssemblyRestrictToLoadingPattern
        {
            get { return assemblyRestrictToLoadingPattern; }
            set { assemblyRestrictToLoadingPattern = value; }
        }

        #endregion

        #region Nested classes

        private class AttributedAssembly
        {
            internal Assembly Assembly { get; set; }
            internal Type PluginAttributeType { get; set; }
        }

        #endregion

        #region Methods

        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof (T), onlyConcreteClasses);
        }

        /// <summary>
        ///     查找实现了通过参数指定的接口的类型
        /// </summary>
        /// <param name="assignTypeFrom">接口类型</param>
        /// <param name="onlyConcreteClasses">只查找具体类，默认为true</param>
        /// <returns>实现指定接口的类型列表</returns>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(assignTypeFrom, GetAssemblies(), onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof (T), assemblies, onlyConcreteClasses);
        }

        /// <summary>
        ///     在指定程序集中查找实现了通过参数指定的接口的类型
        /// </summary>
        /// <param name="assignTypeFrom">接口类型</param>
        /// <param name="assemblies">查找的程序集列表</param>
        /// <param name="onlyConcreteClasses">只查找具体类，默认为true</param>
        /// <returns>实现指定接口的类型列表</returns>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies,
            bool onlyConcreteClasses = true)
        {
            var result = new List<Type>();
            try
            {
                foreach (var a in assemblies)
                {
                    Type[] types = null;
                    try
                    {
                        types = a.GetTypes();
                    }
                    catch
                    {
                        //EF6不允许获取类型，会报异常
                        if (!ignoreReflectionErrors)
                        {
                            throw;
                        }
                    }
                    if (types != null)
                    {
                        foreach (var t in types)
                        {
                            if (assignTypeFrom.IsAssignableFrom(t) ||
                                (assignTypeFrom.IsGenericTypeDefinition &&
                                 DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                            {
                                if (!t.IsInterface)
                                {
                                    if (onlyConcreteClasses)
                                    {
                                        if (t.IsClass && !t.IsAbstract)
                                        {
                                            result.Add(t);
                                        }
                                    }
                                    else
                                    {
                                        result.Add(t);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var msg = string.Empty;
                foreach (var e in ex.LoaderExceptions)
                    msg += e.Message + Environment.NewLine;

                var fail = new Exception(msg, ex);
                Debug.WriteLine(fail.Message, fail);

                throw fail;
            }
            return result;
        }

        public IEnumerable<Type> FindClassesOfType<T, TAssemblyAttribute>(bool onlyConcreteClasses = true)
            where TAssemblyAttribute : Attribute
        {
            var found = FindAssembliesWithAttribute<TAssemblyAttribute>();
            return FindClassesOfType<T>(found, onlyConcreteClasses);
        }

        public IEnumerable<Assembly> FindAssembliesWithAttribute<T>()
        {
            return FindAssembliesWithAttribute<T>(GetAssemblies());
        }

        public IEnumerable<Assembly> FindAssembliesWithAttribute<T>(IEnumerable<Assembly> assemblies)
        {
            //check if we've already searched this assembly);)
            if (!_assemblyAttributesSearched.Contains(typeof (T)))
            {
                var foundAssemblies = (from assembly in assemblies
                    let customAttributes = assembly.GetCustomAttributes(typeof (T), false)
                    where customAttributes.Any()
                    select assembly).ToList();
                //now update the cache
                _assemblyAttributesSearched.Add(typeof (T));
                foreach (var a in foundAssemblies)
                {
                    _attributedAssemblies.Add(new AttributedAssembly {Assembly = a, PluginAttributeType = typeof (T)});
                }
            }

            //We must do a ToList() here because it is required to be serializable when using other app domains.
            return _attributedAssemblies
                .Where(x => x.PluginAttributeType.Equals(typeof (T)))
                .Select(x => x.Assembly)
                .ToList();
        }

        public IEnumerable<Assembly> FindAssembliesWithAttribute<T>(DirectoryInfo assemblyPath)
        {
            var assemblies = (from f in Directory.GetFiles(assemblyPath.FullName, "*.dll")
                select Assembly.LoadFrom(f)
                into assembly
                let customAttributes = assembly.GetCustomAttributes(typeof (T), false)
                where customAttributes.Any()
                select assembly).ToList();
            return FindAssembliesWithAttribute<T>(assemblies);
        }

        /// <summary>Gets tne assemblies related to the current implementation.</summary>
        /// <returns>A list of assemblies that should be loaded by the LCM factory.</returns>
        public virtual IList<Assembly> GetAssemblies()
        {
            var addedAssemblyNames = new List<string>();
            var assemblies = new List<Assembly>();

            if (LoadAppDomainAssemblies)
                AddAssembliesInAppDomain(addedAssemblyNames, assemblies);
            AddConfiguredAssemblies(addedAssemblyNames, assemblies);

            return assemblies;
        }

        #endregion

        #region Utilities

        /// <summary>
        ///     Iterates all assemblies in the AppDomain and if it's name matches the configured patterns add it to our list.
        /// </summary>
        /// <param name="addedAssemblyNames"></param>
        /// <param name="assemblies"></param>
        private void AddAssembliesInAppDomain(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (Matches(assembly.FullName))
                {
                    if (!addedAssemblyNames.Contains(assembly.FullName))
                    {
                        assemblies.Add(assembly);
                        addedAssemblyNames.Add(assembly.FullName);
                    }
                }
            }
        }

        /// <summary>
        ///     Adds specificly configured assemblies.
        /// </summary>
        /// <param name="addedAssemblyNames"></param>
        /// <param name="assemblies"></param>
        protected virtual void AddConfiguredAssemblies(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (string assemblyName in AssemblyNames)
            {
                Assembly assembly = Assembly.Load(assemblyName);
                if (!addedAssemblyNames.Contains(assembly.FullName))
                {
                    assemblies.Add(assembly);
                    addedAssemblyNames.Add(assembly.FullName);
                }
            }
        }

        /// <summary>
        ///     Check if a dll is one of the shipped dlls that we know don't need to be investigated.
        /// </summary>
        /// <param name="assemblyFullName">
        ///     The name of the assembly to check.
        /// </param>
        /// <returns>
        ///     True if the assembly should be loaded into LCM.
        /// </returns>
        public virtual bool Matches(string assemblyFullName)
        {
            return !Matches(assemblyFullName, AssemblySkipLoadingPattern)
                   && Matches(assemblyFullName, AssemblyRestrictToLoadingPattern);
        }

        /// <summary>
        ///     Check if a dll is one of the shipped dlls that we know don't need to be investigated.
        /// </summary>
        /// <param name="assemblyFullName">
        ///     The assembly name to match.
        /// </param>
        /// <param name="pattern">
        ///     The regular expression pattern to match against the assembly name.
        /// </param>
        /// <returns>
        ///     True if the pattern matches the assembly name.
        /// </returns>
        protected virtual bool Matches(string assemblyFullName, string pattern)
        {
            return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        ///     Makes sure matching assemblies in the supplied folder are loaded in the app domain.
        /// </summary>
        /// <param name="directoryPath">
        ///     The physical path to a directory containing dlls to load in the app domain.
        /// </param>
        protected virtual void LoadMatchingAssemblies(string directoryPath)
        {
            var loadedAssemblyNames = new List<string>();
            foreach (Assembly a in GetAssemblies())
            {
                loadedAssemblyNames.Add(a.FullName);
            }

            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            foreach (string dllPath in Directory.GetFiles(directoryPath, "*.dll"))
            {
                try
                {
                    var an = AssemblyName.GetAssemblyName(dllPath);
                    if (Matches(an.FullName) && !loadedAssemblyNames.Contains(an.FullName))
                    {
                        App.Load(an);
                    }

                    //old loading stuff
                    //Assembly a = Assembly.ReflectionOnlyLoadFrom(dllPath);
                    //if (Matches(a.FullName) && !loadedAssemblyNames.Contains(a.FullName))
                    //{
                    //    App.Load(a.FullName);
                    //}
                }
                catch (BadImageFormatException ex)
                {
                    Trace.TraceError(ex.ToString());
                }
            }
        }

        /// <summary>
        ///     当openGeneric是泛型接口时，判断type是否实现这个泛型接口
        /// </summary>
        /// <param name="type">实现类的类型</param>
        /// <param name="openGeneric">泛型接口的类型</param>
        /// <returns>类型是否实现泛型接口</returns>
        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
                {
                    if (!implementedInterface.IsGenericType)
                        continue;

                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
                    return isMatch;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}