using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using MetroLogWebTarget.Core.Infrastructure;
using MetroLogWebTarget.Web.Framework;

namespace MetroLogWebTarget.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            EngineContext.Initialize(false);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
