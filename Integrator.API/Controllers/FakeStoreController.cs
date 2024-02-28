using Data.IServices.Integrators;
using Data.Models.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Integrator.API.Controllers
{
    [EnableCors(origins: "http://localhost:53097", headers: "*", methods: "*")]
    public class FakeStoreController : ApiController
    {
        private readonly IFakeStoreService _fakeStoreService;

        public FakeStoreController(IFakeStoreService fakeStoreService)
        {
            _fakeStoreService = fakeStoreService;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("fake-store/urunler")]
        public string GetFakeStoreProducts()
        {
            var data = _fakeStoreService.GetProducts();
            return data;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("fake-store/urunler")]
        public string AddFakeStoreProducts(ApiProduct apiProduct)
        {

            var data = _fakeStoreService.AddProduct(apiProduct);
            return data;
        }
    }
}
