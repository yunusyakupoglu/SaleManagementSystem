using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Auth.API.Controllers
{
    public class TrendyolController : ApiController
    {
        [HttpPost]
        public IHttpActionResult SetCredentials(TrendyolSettings settings)
        {
            return Ok(new { message = "Kategori başarıyla kaydedildi." });
        }

        [HttpGet]
        public IHttpActionResult GetCredentials(TrendyolSettings settings)
        {
            return Ok(new { data="", message = "Kategori başarıyla kaydedildi." });
        }
    }
}
