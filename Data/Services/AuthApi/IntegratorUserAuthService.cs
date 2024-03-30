using Data.Common;
using Data.IServices.AuthApi;
using Data.Models;
using Data.Models.api;
using Newtonsoft.Json;
using RestSharp;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.AuthApi
{
    public class IntegratorUserAuthService : IIntegratorUserAuthService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;
        //private readonly ITokenService _tokenService;


        public IntegratorUserAuthService()
        {
            this.compiler = new SqlServerCompiler();
        }

        private QueryFactory CreateQueryFactory()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            return new QueryFactory(this.connection, this.compiler);
        }


        public void Register(IntegratorUser user)
        {
            string hashedPassword = PasswordHasher.HashPassword(user.Password);
            string hashedPasswordConfirm = PasswordHasher.HashPassword(user.PasswordConfirm);
            using (var db = CreateQueryFactory())
            {
                db.Query("IntegratorUser").Insert(new
                {
                    IntegratorName = user.IntegratorName,
                    Email = user.Email,
                    Password = hashedPassword,
                    PasswordConfirm = hashedPasswordConfirm,
                    EmailConfirm = user.EmailConfirm,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    Guid = Guid.NewGuid()
                });
            }
        }

        public async Task<ApiResponseModel> Login(LoginModel login)
        {
            string hashedPassword = PasswordHasher.HashPassword(login.Password);
            using (var db = CreateQueryFactory())
            {
                var user = db.Query("IntegratorUser").Where("Email", login.Email).FirstOrDefault<IntegratorUser>();
                if (user != null)
                {
                    if (PasswordHasher.VerifyPassword(login.Password, user.Password))
                    {
                        using (var httpClient = new HttpClient())
                        {
                            string apiUrl = "http://localhost:57183/user/get-token";

                            // JSON içeriğini elle oluştur.
                            var json = JsonConvert.SerializeObject(new { user.Guid });
                            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                            {
                                // POST isteğini gönder.
                                var response = await httpClient.PostAsync(apiUrl, content);

                                if (response.IsSuccessStatusCode)
                                {
                                    // JSON cevabını deserialize et.
                                    var tokenResponse = await response.Content.ReadAsStringAsync();
                                    dynamic tokenData = JsonConvert.DeserializeObject(tokenResponse);
                                    return new ApiResponseModel { message = tokenData.token.data,statusCode = System.Net.HttpStatusCode.OK };
                                }
                                else
                                {
                                    return new ApiResponseModel { message = "Token alınamadı", statusCode = System.Net.HttpStatusCode.BadRequest };
                                }
                            }
                        }
                    }
                    else
                    {
                        return new ApiResponseModel { message = "Kullanıcı girişi başarısız", statusCode = System.Net.HttpStatusCode.Unauthorized };
                    }
                }
                else
                {
                    return new ApiResponseModel { message = "Kullanıcı bulunamadı", statusCode = System.Net.HttpStatusCode.NotFound };
                }
            }
        }
    }
}
