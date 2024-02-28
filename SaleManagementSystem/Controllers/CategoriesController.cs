using Data.IServices;
using Data.Models.Project;
using System;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Categories
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Paginate(int page, int dataCount)
        {
            try
            {

                var categories = _categoryService.GetPage(page, dataCount);
                return Json(new { data = categories.List, pageCount = categories.TotalPages, totalCount = categories.Count, page = categories.Page, perPage = categories.PerPage }, JsonRequestBehavior.AllowGet);
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

                var categories = _categoryService.Filter(filter);
                return Json(new { data = categories }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Insert(Category category)
        {
            try
            {
                _categoryService.Insert(category);
                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"{category.CategoryName} kategorisi başarıyla eklendi." });
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
                _categoryService.UpdateIsActive(guid, isActive);

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
                _categoryService.UpdateDeleted(guid);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Kategori silme işlemi başarılı." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetCategory(Guid guid)
        {
            try
            {
                var category = _categoryService.GetByGuid(guid);
                if (category == null)
                {
                    return Json(new { success = false, message = "Kategori bulunamadı." });
                }

                return Json(new { success = true, data = category }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetCategories()
        {
            try
            {
                var categories = _categoryService.Get();
                if (categories == null)
                {
                    return Json(new { success = false, message = "Firma bulunamadı." });
                }

                return Json(new { success = true, data = categories }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Update(Category category)
        {
            try
            {
                _categoryService.Update(category);
                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Kategori başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }
    }
}