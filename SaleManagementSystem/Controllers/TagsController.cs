using Data.IServices;
using Data.Models.Project;
using System;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: Tags
        public ActionResult Index()
        {
            try
            {
                var tags = _tagService.Get();
                return Json(new { data = tags }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Refresh()
        {
            try
            {
                var tags = _tagService.Get();
                return Json(new { data = tags }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Insert(string tagName, string tagColor, Guid product)
        {
            try
            {
                var tag = new Tag
                {
                    TagName = tagName,
                    TagColor = tagColor,
                    Product = product
                };
                _tagService.Insert(tag);
                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"{tagName} etiketi başarıyla eklendi." });
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

                _tagService.UpdateIsActive(guid, isActive);

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
                _tagService.UpdateDeleted(guid);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Etiket silme işlemi başarılı." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }
    }
}