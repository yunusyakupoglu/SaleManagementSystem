using Data.IServices;
using Data.IServices.Integrators;
using Data.Services;
using Data.Services.Integrators;
using SaleManagementSystem.Common;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Unity.AspNet.Mvc;

namespace SaleManagementSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new AuthorizeUserAttribute());
            //GlobalFilters.Filters.Add(new CheckUserAndRoleFilter());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            var container = new UnityContainer().AddExtension(new Diagnostic());

            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IPermissionService, PermissionService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IUnitService, UnitService>();
            container.RegisterType<IStockService, StockService>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IBrandService, BrandService>();
            container.RegisterType<ITagService, TagService>();
            container.RegisterType<ICompanyService, CompanyService>();
            container.RegisterType<ITicketService, TicketService>();
            container.RegisterType<ITicketProductService, TicketProductService>();
            container.RegisterType<ITrendyolService, TrendyolService>();
            container.RegisterType<ICargoService, CargoService>();


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
