using Data.IServices;
using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class TicketController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IStockService _stockService;
        private readonly IUnitService _unitService;
        private readonly ITicketService _ticketService;

        public TicketController(ICompanyService companyService, IStockService stockService, IUnitService unitService, ITicketService ticketService)
        {
            _companyService = companyService;
            _stockService = stockService;
            _unitService = unitService;
            _ticketService = ticketService;
        }

        // GET: Ticket
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Paginate(int page, int dataCount)
        {
            try
            {

                var tickets = _ticketService.GetPage(page, dataCount);
                return Json(new { data = tickets.List, pageCount = tickets.TotalPages, totalCount = tickets.Count, page = tickets.Page, perPage = tickets.PerPage }, JsonRequestBehavior.AllowGet);
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

                var tickets = _ticketService.Filter(filter);
                return Json(new { data = tickets }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetCompany(Guid guid)
        {
            try
            {
                var company = _companyService.GetByGuid(guid);
                return Json(new { data = company }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

   
        public ActionResult GetUnit()
        {
            try
            {
                var units = _unitService.Get();
                return Json(new { data = units }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: TicketProduct
        public ActionResult Insert()
        {
            var companies = _companyService.Get();
            ViewBag.Companies = companies;
            return View();
        }

        [HttpPost]
        public ActionResult Insert(Ticket ticket)
        {
            try
            {
                var ticketdata = _ticketService.Insert(ticket);
                return Json(new { success = true, data = ticketdata, message = $"{ticket.Guid} fişi başarıyla kaydedildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });

            }
        }

        [HttpPost]
        public ActionResult UpdateStatus(Guid guid, bool isActive)
        {
            try
            {

                _ticketService.UpdateIsActive(guid, isActive);

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
                _ticketService.UpdateDeleted(guid);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Ürün silme işlemi başarılı." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }
    }
}