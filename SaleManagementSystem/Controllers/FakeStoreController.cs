using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class FakeStoreController : Controller
    {
        // GET: FakeStore
        public ActionResult Index()
        {
            return View();
        }
    }
}