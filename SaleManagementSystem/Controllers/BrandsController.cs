using Data.IServices;
using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class BrandsController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // GET: Brands
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Paginate(int page, int dataCount)
        {
            try
            {

                var brands = _brandService.GetPage(page, dataCount);
                return Json(new { data = brands.List, pageCount = brands.TotalPages, totalCount = brands.Count, page = brands.Page, perPage = brands.PerPage }, JsonRequestBehavior.AllowGet);
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

                var brands = _brandService.Filter(filter);
                return Json(new { data = brands }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Insert(HttpPostedFileBase file, string brandName)
        {
            string path = null;
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/Files/Brands"), fileName);
                    file.SaveAs(path);
                }

                var brand = new Brand
                {
                    BrandName = brandName,
                    ImgName = file.FileName,
                    ImgUrl = path
                };

                _brandService.Insert(brand);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = "Marka ve dosya başarıyla eklendi." });
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

                _brandService.UpdateIsActive(guid, isActive);

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
                _brandService.UpdateDeleted(guid);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Marka silme işlemi başarılı." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetBrand(Guid guid)
        {
            try
            {
                var category = _brandService.GetByGuid(guid);
                if (category == null)
                {
                    return Json(new { success = false, message = "Firma bulunamadı." });
                }

                return Json(new { success = true, data = category }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetBrands()
        {
            try
            {
                var brands = _brandService.Get();
                if (brands == null)
                {
                    return Json(new { success = false, message = "Firma bulunamadı." });
                }

                return Json(new { success = true, data = brands }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Update(Guid guid, HttpPostedFileBase file, string brandName, string imgUrl, string imgName)
        {
            string path = null;

            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/Files/Brands"), fileName);
                    file.SaveAs(path);
                }

                Brand brand = null;
                if (file != null)
                {
                    brand = new Brand
                    {
                        BrandName = brandName,
                        Guid = guid,
                        ImgName = file.FileName,
                        ImgUrl = path
                    };
                } else
                {
                    brand = new Brand
                    {
                        BrandName = brandName,
                        Guid = guid,
                        ImgName = imgName,
                        ImgUrl = imgUrl
                    };
                }

                _brandService.Update(brand);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"Firma başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }
    }
}