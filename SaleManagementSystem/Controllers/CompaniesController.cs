using Data.IServices;
using Data.Models.Project;
using System;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET: Companies
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Paginate(int page, int dataCount)
        {
            try
            {

                var companies = _companyService.GetPage(page, dataCount);
                return Json(new { data = companies.List, pageCount = companies.TotalPages, totalCount = companies.Count, page = companies.Page, perPage = companies.PerPage }, JsonRequestBehavior.AllowGet);
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

                var companies = _companyService.Filter(filter);
                return Json(new { data = companies }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Insert(Company company)
        {
            try
            {
                var random = new Random();
                int randomNumber = random.Next(1000, 10000);
                string companyCode = "A-" + randomNumber.ToString();
                company.CompanyCode = companyCode;
                _companyService.Insert(company);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"{company.CompanyName} firması başarıyla eklendi." });
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

                _companyService.UpdateIsActive(guid, isActive);

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
                _companyService.UpdateDeleted(guid);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Firma silme işlemi başarılı." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        public ActionResult GetByGuid(Guid guid)
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

    }
}