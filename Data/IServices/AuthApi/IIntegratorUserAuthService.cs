using Data.Models;
using Data.Models.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices.AuthApi
{
    public interface IIntegratorUserAuthService
    {
        void Register(IntegratorUser user);
        Task<ApiResponseModel> Login(LoginModel login);
    }
}
