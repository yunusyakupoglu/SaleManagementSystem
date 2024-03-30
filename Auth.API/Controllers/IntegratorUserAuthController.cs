using Auth.API.IServices;
using Data.IServices.AuthApi;
using Data.Models;
using Data.Models.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Auth.API.Controllers
{
    [EnableCors(origins: "http://localhost:53097", headers: "*", methods: "*")]
    public class IntegratorUserAuthController : ApiController
    {
        private readonly IIntegratorUserAuthService _service;
        private readonly ITokenService _tokenService;

        public IntegratorUserAuthController(IIntegratorUserAuthService service, ITokenService tokenService)
        {
            _service = service;
            _tokenService = tokenService;
        }

        [HttpPost]
        [System.Web.Http.Route("user/register")]
        public IHttpActionResult Register(IntegratorUser user)
        {
            _service.Register(user);
            return Json(new { StatusCode = HttpStatusCode.OK });
        }

        [HttpPost]
        [System.Web.Http.Route("user/login")]
        public async Task<IHttpActionResult> Login(LoginModel login)
        {
            var data = await _service.Login(login);
            return Json(new { Data = data.message, StatusCode = data.statusCode });
        }

        [HttpPost]
        [System.Web.Http.Route("user/get-token")]
        public async Task<IHttpActionResult> Token(IntegratorUser user)
        {
            if (user != null)
            {
                var token = await Task.Run(() => _tokenService.GetToken(user.Guid));
                return Ok(new { token = token });
            }
            return BadRequest("Invalid request");
        }
    }
}
