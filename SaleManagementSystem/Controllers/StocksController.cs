using Data.IServices;
using Data.Models.Dto;
using Data.Models.Project;
using SaleManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class StocksController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IUnitService _unitService;
        private readonly IProductService _productService;

        public StocksController(IStockService stockService, IUnitService unitService, IProductService productService)
        {
            _stockService = stockService;
            _unitService = unitService;
            _productService = productService;
        }


        // GET: Stocks
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Paginate(int page, int dataCount)
        {
            try
            {

                var stocks = _stockService.GetPage(page, dataCount);
                return Json(new { data = stocks.List, pageCount = stocks.TotalPages, totalCount = stocks.Count, page = stocks.Page, perPage = stocks.PerPage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Filter(string filter)
        {
            try
            {

                var stocks = _stockService.Filter(filter);
                return Json(new { data = stocks }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Insert(Guid product, Guid unit, string quantity)
        {
            try
            {
                bool isConverted = float.TryParse(quantity.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out float quantityFloat);
                if (isConverted)
                {
                    quantityFloat = (float)Math.Round(quantityFloat, 2);
                }
                else
                {
                    return Json(new { success = false, message = "Hata: Float dönüşümü yapılamadı."});
                }
                var stock = new Stock
                {
                    Product = product,
                    Unit = unit,
                    Quantity = quantityFloat
                };

                _stockService.Insert(stock);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Stok ekleme işlemi başarılı." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult UpdateRange(IEnumerable<TicketProductResult> ticketProducts)
        {
            try
            {
                _stockService.UpdateQuantityRange(ticketProducts);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Toplu stok güncelleme işlemi başarılı." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult UpdateStatus(Guid guid, bool isActive)
        {
            try
            {

                _stockService.UpdateIsActive(guid, isActive);

                // Başarılı işlem sonucu
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid guid)
        {
            try
            {
                _stockService.UpdateDeleted(guid);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Stok silme işlemi başarılı." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetStock(Guid guid)
        {
            try
            {
                var stock = _stockService.GetByGuid(guid);
                if (stock == null)
                {
                    return Json(new { success = false, message = "Stok bulunamadı." });
                }

                return Json(new { success = true, data = stock }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Update(Stock stock)
        {
            try
            {
                _stockService.Update(stock);
                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Stok başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }
    }
}