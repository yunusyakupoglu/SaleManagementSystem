using Data.IServices;
using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class UnitsController : Controller
    {
        private readonly IUnitService _unitService;

        public UnitsController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        // GET: Units
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Paginate(int page, int dataCount)
        {
            try
            {

                var units = _unitService.GetPage(page, dataCount);
                return Json(new { data = units.List, pageCount = units.TotalPages, totalCount = units.Count, page = units.Page, perPage = units.PerPage }, JsonRequestBehavior.AllowGet);
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

                var units = _unitService.Filter(filter);
                return Json(new { data = units }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Insert(string unitName, string formule)
        {
            try
            {

                var unit = new Unit
                {
                    UnitName = unitName,
                    Formule = formule,
                };

                _unitService.Insert(unit);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"{unitName} birimi başarıyla eklendi." });
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

                _unitService.UpdateIsActive(guid, isActive);

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
                _unitService.UpdateDeleted(guid);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Birim silme işlemi başarılı." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetUnit(Guid guid)
        {
            try
            {
                var unit = _unitService.GetByGuid(guid);
                if (unit == null)
                {
                    return Json(new { success = false, message = "Birim bulunamadı." });
                }

                return Json(new { success = true, data = unit }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Update(Unit unit)
        {
            try
            {
                _unitService.Update(unit);
                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Birim başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetUnits()
        {
            try
            {
                var units = _unitService.Get();
                if (units == null)
                {
                    return Json(new { success = false, message = "Birimler bulunamadı." });
                }

                return Json(new { success = true, data = units }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }
    }
}