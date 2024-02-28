using Data.IServices;
using Data.Models;
using SaleManagementSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SaleManagementSystem.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;

        public AccountsController(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string Username, string Password)
        {
            var login = _accountService.Login(Username, Password);
            if (login != null)
            {
                FormsAuthentication.SetAuthCookie(login.Username, false);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Accounts");
        }

    }
}