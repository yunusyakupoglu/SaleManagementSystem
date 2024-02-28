using Data.Common;
using Data.IServices;
using SaleManagementSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    [CheckUserAndRoleFilter]
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        [AuthorizeUser(role: "Editor", permissions: new Permission[] { Permission.View, Permission.Insert }, requireAuthenticated: true)]
        public ActionResult Index()
        {
            return View();
        }
    }
}