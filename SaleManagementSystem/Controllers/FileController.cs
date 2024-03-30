  using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        [HttpPost]
        public ActionResult TrendyolFileUpload(List<HttpPostedFileBase> files)
        {
            List<Data.Models.api.Image> fileUrls = new List<Data.Models.api.Image>();
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    // Dosya adındaki boşlukları "-" ile değiştir
                    var fileName = Path.GetFileName(file.FileName).Replace(" ", "-");

                    // Güvenli dosya adı oluştur (opsiyonel olarak daha fazla düzenleme yapılabilir)
                    fileName = Regex.Replace(fileName, "[^a-zA-Z0-9.-]", "-");

                    var relativePath = Path.Combine("~/Files/TrendyolImages", fileName);
                    var absolutePath = Server.MapPath(relativePath);
                    file.SaveAs(absolutePath);

                    // Temel URL oluşturuluyor
                    var request = HttpContext.Request;
                    var baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}{Url.Content("~")}";

                    // Dosyanın tam URL'si oluşturuluyor ve URL'ler güvenli bir şekilde birleştiriliyor
                    var fileUrl = new Uri(new Uri(baseUrl), $"Files/TrendyolImages/{fileName}").ToString();
                    fileUrls.Add(new Data.Models.api.Image { url = fileUrl });
                }
            }

            return Json(new { success = true, data = fileUrls});
        }
    }
}