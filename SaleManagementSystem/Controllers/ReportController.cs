using Data.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class ReportController : Controller
    {
        private readonly IProductService _service;

        public ReportController(IProductService service)
        {
            _service = service;
        }

        // GET: Report
        public ActionResult Index()
        {
            var data = _service.GetProducts();
            return View(data);
        }
    }
}