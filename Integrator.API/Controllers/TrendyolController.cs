using Data.IServices;
using Data.IServices.Integrators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Integrator.API.Controllers
{
    [EnableCors(origins: "http://localhost:53097", headers: "*", methods: "*")]
    public class TrendyolController : ApiController
    {
        private readonly IIntegratorService _integratorService;
        private readonly ITrendyolService _trendyolService;

        public TrendyolController(IIntegratorService integratorService, ITrendyolService trendyolService)
        {
            _integratorService = integratorService;
            _trendyolService = trendyolService;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("trendyol/kategoriler")]
        public string GetTrendyolCategories()
        {
            var data = _integratorService.GetTrendyolCategories();
            return data;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("trendyol/urunler")]
        public async Task<IHttpActionResult> GetTrendyolProducts()
        {
            var data = await _trendyolService.getProducts();
            return Ok(new { page = data.Page, size = data.Size, totalPages = data.TotalPages, totalElements = data.TotalElements, content = data.Content });
        }

        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("trendyol/urun-sil")]
        public async Task<IHttpActionResult> DeleteTrendyolProducts()
        {
            List<string> barcodes = new List<string>();
            var barcode = HttpContext.Current.Request.Headers["Barcode"];
            if (string.IsNullOrEmpty(barcode))
            {
                return BadRequest("Barcode header is missing.");
            }

            barcodes.Add(barcode);

            var data = await _trendyolService.deleteProduct(barcodes);
            return Ok(new { data = data });
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("trendyol/markalar")]
        public string GetTrendyolBrands(int page)
        {
            var data = _integratorService.GetTrendyolBrands(page);
            return data;
        }
    }
}
