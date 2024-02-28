using Data.IServices;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Integrator.API.Controllers
{
    [EnableCors(origins: "http://localhost:53097", headers: "*", methods: "*")]
    public class ProductsController : ApiController
    {
        private readonly IIntegratorService _ıntegratorService;

        public ProductsController(IIntegratorService ıntegratorService)
        {
            _ıntegratorService = ıntegratorService;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("kategoriler")]
        public string GetTrendyolCategories()
        {
            var data = _ıntegratorService.GetTrendyolCategories();
            return data;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("urunler")]
        public string GetTrendyolProducts()
        {
            var data = _ıntegratorService.GetTrendyolProducts();
            return data;
        }

        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("urun-sil")]
        public string DeleteTrendyolProduct(string barcode)
        {
            var data = _ıntegratorService.GetTrendyolProducts();
            return data;
        }
    }
}
