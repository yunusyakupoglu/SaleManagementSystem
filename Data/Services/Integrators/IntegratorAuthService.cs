using Data.IServices.Integrators;
using Data.Models.Project;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data.Services.Integrators
{
    public class IntegratorAuthService : IIntegratorAuthService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;

        public IntegratorAuthService()
        {
            this.compiler = new SqlServerCompiler();
        }

        private QueryFactory CreateQueryFactory()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            return new QueryFactory(this.connection, this.compiler);
        }


        public void Delete(Auth auth)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Auth").Where("Guid", auth.Guid).Delete();
            }
        }//

        public IEnumerable<Auth> Filter(string filter)
        {
            using (var db = CreateQueryFactory())
            {
                var auths = db.Query("Auth").Where("IntegratorName", "LIKE", $"{filter}%")
                    .Get<Auth>();
                return auths;
            }
        }//

        public IEnumerable<Auth> Get()
        {
            using (var db = CreateQueryFactory())
            {
                var auths = db.Query("Auth").Where("IsDeleted", false).Get<Auth>();
                return auths;
            }
        }//

        public Auth GetByGuid(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                var auth = db.Query("Auth").Where("Guid", guid).FirstOrDefault<Auth>();
                return auth;
            }
        }//

        public Auth GetById(int id)
        {
            using (var db = CreateQueryFactory())
            {
                var auth = db.Query("Auth").Where("Id", id).First<Auth>();
                return auth;
            }
        }

        public dynamic GetPage(int page, int dataPerPage)
        {
            using (var db = CreateQueryFactory())
            {
                var auths = db.Query("Auth").Where(new
                {
                    IsDeleted = false
                }).Paginate<Auth>(page, dataPerPage);

                return auths;
            }
        }//

        public void Insert(Auth auth)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Auth").Insert(new
                {
                    IntegratorName = auth.IntegratorName,
                    MerchantId = auth.MerchantId,
                    IntegratorCompany = auth.IntegratorCompany,
                    ApiKey = auth.ApiKey,
                    SecretKey = auth.SecretKey,
                    SvcCredentials = auth.SvcCredentials,
                    UserAgent = auth.UserAgent,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Guid = Guid.NewGuid(),
                    UserGuid = auth.UserGuid
                });
            }
        }//

        public void Update(Auth auth)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Auth").Where("Guid", auth.Guid).Update(new
                {
                    IntegratorName = auth.IntegratorName,
                    MerchantId = auth.MerchantId,
                    IntegratorCompany = auth.IntegratorCompany,
                    ApiKey = auth.ApiKey,
                    SecretKey = auth.SecretKey,
                    SvcCredentials = auth.SvcCredentials,
                    UserAgent = auth.UserAgent,
                });
            }
        }//

        public void UpdateDeleted(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Auth").Where("Guid", guid).Update(new
                {
                    IsDeleted = true
                });
            }
        }//

        public void UpdateIsActive(Guid guid, bool isActive)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Auth").Where("Guid", guid).Update(new
                {
                    IsActive = isActive
                });
            }
        }//

        public Auth GetByUserGuidAndIntegratorName(Guid userGuid, string integratorName)
        {
            using (var db = CreateQueryFactory())
            {
                var auth = db.Query("Auth").Where(new {
                    UserGuid = userGuid,
                    IntegratorName = integratorName
                }).FirstOrDefault<Auth>();
                return auth;
            }
        }
    }
}
