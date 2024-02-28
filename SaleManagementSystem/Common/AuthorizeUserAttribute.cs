using Data.IServices;
using Data.Models;
using Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Routing;

namespace SaleManagementSystem.Common
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        private readonly string _role;
        private readonly Data.Common.Permission[] _permissions;
        private readonly bool _requireAuthenticated;

        public AuthorizeUserAttribute()
        {

        }

        public AuthorizeUserAttribute(string role, Data.Common.Permission[] permissions, bool requireAuthenticated)
        {
            _role = role;
            _permissions = permissions;
            _requireAuthenticated = requireAuthenticated;
        }

        public AuthorizeUserAttribute(string role, bool requireAuthenticated)
        {
            _role = role;
            _requireAuthenticated = requireAuthenticated;
        }

        public AuthorizeUserAttribute(Data.Common.Permission[] permissions, bool requireAuthenticated)
        {
            _permissions = permissions;
            _requireAuthenticated = requireAuthenticated;
        }

        public AuthorizeUserAttribute(bool requireAuthenticated)
        {
            _requireAuthenticated = requireAuthenticated;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (_requireAuthenticated)
            {
                var _accountService = DependencyResolver.Current.GetService<IAccountService>();
                var _roleService = DependencyResolver.Current.GetService<IRoleService>();
                var _permissionService = DependencyResolver.Current.GetService<IPermissionService>();

                var isAuthenticated = base.AuthorizeCore(httpContext);
                if (!isAuthenticated)
                {
                    return false;
                }

                var userId = httpContext.User.Identity.Name;


                if (httpContext == null)
                {
                    throw new ArgumentNullException("httpContext");
                }

                // Cookie'den kullanıcı bilgisi alınır
                var cookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie == null)
                {
                    return false;
                }

                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                var identity = new FormsIdentity(ticket);
                var principal = new GenericPrincipal(identity, null);

                var username = principal.Identity.Name;

                User user = _accountService.GetCookieUser(username);

                var returnValue = CheckUserRolesAndPermissions(user, _role, _permissions, _permissionService, _roleService, _accountService);

                return returnValue;
            }
            else
            {
                return true;
            }

        }

        private bool CheckUserRolesAndPermissions(User user, string role, Data.Common.Permission[] permissions, IPermissionService permissionService, IRoleService roleService, IAccountService accountService)
        {
            bool isInRole = false;
            if (role != null || role != String.Empty)
            {
                var roleData = roleService.GetRole(user.Role);
                if (roleData != null && role.Equals(roleData.RoleName))
                {
                    isInRole = true;
                }
            }

            bool hasPermission = false;
            if (permissions != null && permissions.Length > 0)
            {
                foreach (var item in permissions)
                {
                    var userPermission = permissionService.GetPermission(user.Guid);
                    if (userPermission != null)
                    {
                        var prm = permissionService.GetPermissionValue(item);
                        hasPermission = prm;
                    }
                }
            }

            return isInRole || hasPermission; // Bu satır değişti
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }


            bool isAuthorized = AuthorizeCore(filterContext.HttpContext);
            if (!isAuthorized)
            {
                // Eğer kullanıcı giriş yapmamışsa, onları giriş sayfasına yönlendir
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                { "controller", "Accounts" },
                { "action", "Login" }
                        });
                }
                else
                {
                    // Kullanıcı giriş yapmış ama yetkisi yoksa, AccessDenied sayfasına yönlendir
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                { "controller", "Error" },
                { "action", "AccessDenied" }
                        });
                }
            }
        }
    }
}