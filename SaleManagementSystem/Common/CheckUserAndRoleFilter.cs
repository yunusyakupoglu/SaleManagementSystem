using Data.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SaleManagementSystem.Common
{
    public class CheckUserAndRoleFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var _accountService = DependencyResolver.Current.GetService<IAccountService>();
            var _roleService = DependencyResolver.Current.GetService<IRoleService>();
            var checkCount = _accountService.Check();


            if (checkCount.RoleCount == 0 && checkCount.UserCount == 0)
            {
                _roleService.CreateRoles();
                _accountService.Register(user: null);
                base.OnActionExecuting(filterContext);
            }
            else if (checkCount.RoleCount == 0)
            {
                _roleService.CreateRoles();
                base.OnActionExecuting(filterContext);
            }
            else if (checkCount.UserCount == 0)
            {
                _accountService.Register(user: null);
                base.OnActionExecuting(filterContext);
            }
            else
            {
                // Kullanıcı ve rol varsa devam et
                base.OnActionExecuting(filterContext);
            }
        }
    }
}