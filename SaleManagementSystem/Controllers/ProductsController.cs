using Data.IServices;
using Data.Models.Project;
using SaleManagementSystem.Models;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public ProductsController(IProductService productService, IBrandService brandService, ICategoryService categoryService, ITagService tagService)
        {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
            _tagService = tagService;
        }


        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetProducts()
        {
            var products = _productService.GetDto();
            return Json(new { data = products }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProduct(Guid guid)
        {
            var product = _productService.GetByGuid(guid);
            return Json(new { data = product }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Paginate(int page, int dataCount)
        {
            try
            {

                var products = _productService.GetPage(page, dataCount);
                return Json(new { data = products.List, pageCount = products.TotalPages, totalCount = products.Count, page = products.Page, perPage = products.PerPage }, JsonRequestBehavior.AllowGet);
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

                var products = _productService.Filter(filter);
                return Json(new { data = products }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Insert(HttpPostedFileBase file, Guid brand, Guid category, string productName, string purchasePrice, string sellPrice, int vat)
        {
            string path = null;
            try
            {
                // Ondalık sayıları C# formatına dönüştür
                decimal.TryParse(purchasePrice.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal purchasePriceDecimal);
                decimal.TryParse(sellPrice.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal sellPriceDecimal);

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/Files/Products"), fileName);
                    file.SaveAs(path);
                }

                var product = new Product
                {
                    Brand = brand,
                    Category = category,
                    ImgName = file.FileName,
                    ImgUrl = path,
                    ProductName = productName,
                    PurchasePrice = purchasePriceDecimal,
                    SellPrice = sellPriceDecimal,
                    Vat = vat,
                };

                _productService.Insert(product);

                // Başarılı işlem sonucu
                return Json(new { success = true, message = $"{productName} ürünü başarıyla eklendi." });
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

                _productService.UpdateIsActive(guid, isActive);

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
                _productService.UpdateDeleted(guid);

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