using Data.IServices;
using Data.Models.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class CargoController : Controller
    {
        private readonly ICargoService _cargoService;

        public CargoController(ICargoService cargoService)
        {
            _cargoService = cargoService;
        }


        // GET: Cargo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Paginate(int page, int dataCount)
        {
            try
            {

                var cargos = _cargoService.GetPage(page, dataCount);
                return Json(new { data = cargos.List, pageCount = cargos.TotalPages, totalCount = cargos.Count, page = cargos.Page, perPage = cargos.PerPage }, JsonRequestBehavior.AllowGet);
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

                var cargos = _cargoService.Filter(filter);
                return Json(new { data = cargos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Insert(Cargo cargo)
        {
            try
            {
                _cargoService.Insert(cargo);
                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"{cargo.Name} kargo başarıyla eklendi." });
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
                _cargoService.UpdateIsActive(guid, isActive);

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
                _cargoService.UpdateDeleted(guid);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Kargo silme işlemi başarılı." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetCargo(Guid guid)
        {
            try
            {
                var cargo = _cargoService.GetByGuid(guid);
                if (cargo == null)
                {
                    return Json(new { success = false, message = "Kargo bulunamadı." });
                }

                return Json(new { success = true, data = cargo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetCargos()
        {
            try
            {
                var cargos = _cargoService.Get();
                if (cargos == null)
                {
                    return Json(new { success = false, message = "Kargo bulunamadı." });
                }

                return Json(new { success = true, data = cargos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Update(Cargo cargo)
        {
            try
            {
                _cargoService.Update(cargo);
                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Kargo başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }
    }
}