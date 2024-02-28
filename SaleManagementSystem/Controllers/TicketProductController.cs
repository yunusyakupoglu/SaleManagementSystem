using Data.IServices;
using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class TicketProductController : Controller
    {
        private readonly ITicketProductService _ticketProductService;
        private readonly IStockService _stockService;

        public TicketProductController(ITicketProductService ticketProductService, IStockService stockService)
        {
            _ticketProductService = ticketProductService;
            _stockService = stockService;
        }

        // GET: TicketProduct
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(IEnumerable<TicketProduct> ticketProducts)
        {
            try
            {
                var result = _ticketProductService.InsertRange(ticketProducts);
                return Json(new { success = true, data = result, message = $"Ürünler başarıyla fişe eklendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });

            }
        }

    }
}