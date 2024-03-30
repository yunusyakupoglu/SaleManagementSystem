using Data.IServices;
using Data.IServices.Integrators;
using Data.Models.api;
using Newtonsoft.Json;
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
        private readonly ITrendyolService _trendyolService;

        public TrendyolController(ITrendyolService trendyolService)
        {
            _trendyolService = trendyolService;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("trendyol/urunler")]
        public async Task<IHttpActionResult> GetTrendyolProducts(string parameter, string size, string svcCredentials, string userAgent, string merchantId)
        {
            var data = await _trendyolService.getProducts(parameter, size, svcCredentials, userAgent, merchantId);
            return Ok(new { page = data.Page, size = data.Size, totalPages = data.TotalPages, totalElements = data.TotalElements, content = data.Content });
        }

        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("trendyol/urun-sil")]
        public async Task<ApiResponseModel> DeleteTrendyolProducts()
        {
            List<string> barcodes = new List<string>();
            var barcode = HttpContext.Current.Request.Headers["Barcode"];
            var SvcCredentials = HttpContext.Current.Request.Headers["SvcCredentials"];
            var UserAgent = HttpContext.Current.Request.Headers["UserAgent"];
            var MerchantId = HttpContext.Current.Request.Headers["MerchantId"];

            if (string.IsNullOrEmpty(barcode))
            {
                return new ApiResponseModel { statusCode = HttpStatusCode.BadRequest, message = "Barkod değeri boş veya null olamaz."};
            }

            barcodes.Add(barcode);

            var data = await _trendyolService.deleteProduct(barcodes, SvcCredentials, UserAgent, MerchantId);
            return data;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("trendyol/urun-ekle")]
        public async Task<ApiResponseModel> CreateTrendyolProducts(PostRequest PostRequest)
        {
            var SvcCredentials = HttpContext.Current.Request.Headers["SvcCredentials"];
            var UserAgent = HttpContext.Current.Request.Headers["UserAgent"];
            var MerchantId = HttpContext.Current.Request.Headers["MerchantId"];
            var data = await _trendyolService.createProduct(PostRequest, SvcCredentials, UserAgent, MerchantId);
            return data;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("trendyol/get-batch-request-result")]
        public async Task<ApiResponseModel> GetBatchRequestResult()
        {
            var batchRequestId = HttpContext.Current.Request.Headers["batchRequestId"];
            var SvcCredentials = HttpContext.Current.Request.Headers["SvcCredentials"];
            var UserAgent = HttpContext.Current.Request.Headers["UserAgent"];
            var MerchantId = HttpContext.Current.Request.Headers["MerchantId"];
            var data = await _trendyolService.getBatchRequestResult(batchRequestId, SvcCredentials, UserAgent, MerchantId);
            return data;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("trendyol/iade-ve-sevkiyat-adres-bilgileri")]
        public async Task<ApiResponseModel> GetSupliersAddresses(string svcCredentials, string userAgent, string merchantId)
        {
            var data = await _trendyolService.getSuppliersAddresses(svcCredentials, userAgent, merchantId);
            return data;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("trendyol/markalar")]
        public async Task<ApiResponseModel> GetBrands(string page, string size, string svcCredentials, string userAgent)
        {
            var data = await _trendyolService.getBrands(page,size,svcCredentials,userAgent);
            return data;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("trendyol/marka-adina-gore-getir")]
        public async Task<ApiResponseModel> GetBrandByName(string brandName, string svcCredentials, string userAgent)
        {
            var data = await _trendyolService.getBrandByName(brandName, svcCredentials, userAgent);
            return data;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("trendyol/urun-kategorileri")]
        public async Task<ApiResponseModel> ProductCategories(string svcCredentials, string userAgent)
        {
            var data = await _trendyolService.getCategoryTree(svcCredentials, userAgent);
            return data;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("trendyol/kategori-ozellik-listesi")]
        public async Task<ApiResponseModel> Attributes(string categoryId, string svcCredentials, string userAgent)
        {
            var data = await _trendyolService.getCategoryAttributes(categoryId, svcCredentials, userAgent);
            return data;
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("trendyol/urun-guncelle")]
        public async Task<ApiResponseModel> UpdateProduct(PutRequest putRequest)
        {
            var SvcCredentials = HttpContext.Current.Request.Headers["SvcCredentials"];
            var UserAgent = HttpContext.Current.Request.Headers["UserAgent"];
            var MerchantId = HttpContext.Current.Request.Headers["MerchantId"];
            var data = await _trendyolService.updateProduct(putRequest, SvcCredentials, UserAgent, MerchantId);
            return data;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("trendyol/stok-ve-fiyat-guncelleme")]
        public async Task<ApiResponseModel> UpdatePriceAndInventory(StockInventoryRequest request, string svcCredentials, string userAgent, string merchantId)
        {
            var data = await _trendyolService.updatePriceAndInventory(request, svcCredentials, userAgent, merchantId);
            return data;
        }
    }
}
