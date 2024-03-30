using Data.IServices.Integrators;
using Data.Models.Project;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaleManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SaleManagementSystem.Controllers
{
    public class TrendyolController : Controller
    {
        private readonly ITrendyolService _service;

        public TrendyolController(ITrendyolService service)
        {
            _service = service;
        }

        // GET: Trendyol
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult CreateProducts()
        {
            return View();
        }

        public ActionResult SupliersAddresses()
        {
            return View();
        }

        public ActionResult Brands()
        {
            return View();
        }

        public ActionResult ProductCategories()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveCategory(string jsonData)
        {
            string json = JsonConvert.SerializeObject(jsonData);

            // JSON dosyasına kaydetme işlemi
            System.IO.File.WriteAllText(Server.MapPath("~/App_Data/categories.json"), jsonData);

            return Json(new { success = true, message = "Kategori başarıyla kaydedildi." });
        }

        [HttpGet]
        public JsonResult GetCategories()
        {
            // JSON dosyasından veri okuma işlemi
            var filePath = Server.MapPath("~/App_Data/categories.json");
            var json = System.IO.File.ReadAllText(filePath);

            var data = JsonConvert.DeserializeObject(json);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult GetCategory(string categoryName)
        //{
        //    categoryName = "Aksesuar";
        //    var filePath = Server.MapPath("~/App_Data/categories.json");
        //    var json = System.IO.File.ReadAllText(filePath);
        //    // JSON dosyasını parse et
        //    var jsonObj = JObject.Parse(json);

        //    // 'categories' anahtarını kullanarak kategoriler listesine ulaş
        //    var categories = (JArray)jsonObj["categories"];

        //    // categoryName ile eşleşen ilk kategoriyi bul
        //    var matchingCategory = categories.FirstOrDefault(c => c["name"].ToString().Equals(categoryName, StringComparison.OrdinalIgnoreCase));

        //    // Eşleşen kategori varsa, bu kategoriyi döndür
        //    if (matchingCategory != null)
        //    {
        //        return Content(matchingCategory.ToString(), "application/json");
        //    }
        //    else
        //    {
        //        // Eşleşen kategori yoksa, hata mesajı döndür
        //        return HttpNotFound($"Category '{categoryName}' not found.");
        //    }
        //}

        [HttpGet]
        public ActionResult GetCategory(string categoryName)
        {
            var filePath = Server.MapPath("~/App_Data/categories.json");
            var json = System.IO.File.ReadAllText(filePath);
            var jsonObj = JObject.Parse(json);

            var categories = (JArray)jsonObj["categories"];
            var matchingCategory = FindCategory(categories, categoryName);

            if (matchingCategory != null)
            {
                return Content(matchingCategory.ToString(), "application/json");
            }
            else
            {
                return HttpNotFound($"Category '{categoryName}' not found.");
            }
        }

        private JToken FindCategory(JArray categories, string categoryName)
        {
            foreach (var category in categories)
            {
                // Şu anki kategori ile eşleşme kontrolü
                if (category["name"].ToString().Equals(categoryName, StringComparison.OrdinalIgnoreCase))
                {
                    return category;
                }

                // Alt kategorilerde rekürsif arama
                if (category["subCategories"] != null)
                {
                    var subCategories = (JArray)category["subCategories"];
                    var subCategoryMatch = FindCategory(subCategories, categoryName);
                    if (subCategoryMatch != null)
                    {
                        return subCategoryMatch;
                    }
                }
            }

            // Eşleşme bulunamadı
            return null;
        }
    }


}


