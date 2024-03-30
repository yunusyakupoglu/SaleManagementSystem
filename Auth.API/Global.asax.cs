using Auth.API.IServices;
using Auth.API.Services;
using Data.IServices.AuthApi;
using Data.IServices.Integrators;
using Data.Services.AuthApi;
using Data.Services.Integrators;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;

namespace Auth.API
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
            container.RegisterType<IIntegratorAuthService, IntegratorAuthService>();
            container.RegisterType<IIntegratorUserAuthService, IntegratorUserAuthService>();
            container.RegisterType<ITokenService, TokenService>();

            // 4. IoC konteynerini Web API uygulamasına tanıtın
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}
