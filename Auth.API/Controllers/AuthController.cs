using Data.IServices.Integrators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Auth.API.Controllers
{
    [EnableCors(origins: "http://localhost:53097", headers: "*", methods: "*")]
    public class AuthController : ApiController
    {
        private readonly IIntegratorAuthService _authService;

        public AuthController(IIntegratorAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [System.Web.Http.Route("integrator/register")]
        public IHttpActionResult Add(Data.Models.Project.Auth auth)
        {
            _authService.Insert(auth);
            return Json(new { StatusCode = HttpStatusCode.Created });
        }

        [HttpPut]
        [System.Web.Http.Route("integrator/update")]
        public IHttpActionResult Update(Data.Models.Project.Auth auth)
        {
            _authService.Update(auth);
            return Json(new { StatusCode = HttpStatusCode.NoContent });
        }

        [HttpDelete]
        [System.Web.Http.Route("integrator/delete")]
        public IHttpActionResult Delete(Data.Models.Project.Auth auth)
        {
            _authService.Delete(auth);
            return Json(new { StatusCode = HttpStatusCode.NoContent });
        }

        [HttpPatch]
        [System.Web.Http.Route("integrator/update-delete-status")]
        public IHttpActionResult UpdateDeleted(Guid guid)
        {
            _authService.UpdateDeleted(guid);
            return Json(new { StatusCode = HttpStatusCode.NoContent });
        }

        [HttpPatch]
        [System.Web.Http.Route("integrator/update-active-status")]
        public IHttpActionResult UpdateIsActive(Guid guid, bool isActive)
        {
            _authService.UpdateIsActive(guid, isActive);
            return Json(new { StatusCode = HttpStatusCode.NoContent });
        }

        [HttpGet]
        [System.Web.Http.Route("integrator/get")]
        public IHttpActionResult Get()
        {
            var data = _authService.Get();
            return Json(new { Data = data, StatusCode = HttpStatusCode.OK });
        }

        [HttpGet]
        [System.Web.Http.Route("integrator/get-by-filter")]
        public IHttpActionResult Filter(string filter)
        {
            var data = _authService.Filter(filter);
            return Json(new { Data = data, StatusCode = HttpStatusCode.OK });
        }

        [HttpGet]
        [System.Web.Http.Route("integrator/get-by-guid")]
        public IHttpActionResult GetByGuid(Guid guid)
        {
            var data = _authService.GetByGuid(guid);
            return Json(new { Data = data, StatusCode = HttpStatusCode.OK });
        }

        [HttpGet]
        [System.Web.Http.Route("integrator/get-by-user-guid-and-integrator-name")]
        public IHttpActionResult GetByGuidAndIntegratorName(Guid userGuid, string integratorName)
        {
            var data = _authService.GetByUserGuidAndIntegratorName(userGuid, integratorName);
            return Json(new { Data = data, StatusCode = HttpStatusCode.OK });
        }

        [HttpGet]
        [System.Web.Http.Route("integrator/get-by-paginate")]
        public IHttpActionResult GetPage(int page, int dataPerPage)
        {
            var data = _authService.GetPage(page, dataPerPage);
            return Json(new { Data = data, StatusCode = HttpStatusCode.OK });
        }
    }
}
