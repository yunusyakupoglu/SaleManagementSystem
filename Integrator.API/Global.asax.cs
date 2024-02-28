using Data.IServices;
using Data.IServices.Integrators;
using Data.Services;
using Data.Services.Integrators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;

namespace Integrator.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new UnityContainer().AddExtension(new Diagnostic());

            // 3. Repository'leri IoC konteynerine kaydedin
            container.RegisterType<IIntegratorService, IntegratorService>();
            container.RegisterType<IFakeStoreService, FakeStoreService>();
            container.RegisterType<ITrendyolService, TrendyolService>();


            // 4. IoC konteynerini Web API uygulamasına tanıtın
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}
