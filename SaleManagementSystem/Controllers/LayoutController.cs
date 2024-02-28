using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class LayoutController : Controller
    {
        public ActionResult Menu()
        {
            bool roleButtonEnabled = TempData["RoleButtonEnabled"] != null && (bool)TempData["RoleButtonEnabled"];
            bool userButtonEnabled = TempData["UserButtonEnabled"] != null && (bool)TempData["UserButtonEnabled"];

            ViewBag.RoleButtonEnabled = roleButtonEnabled;
            ViewBag.UserButtonEnabled = userButtonEnabled;
            return PartialView();
        }
    }
}