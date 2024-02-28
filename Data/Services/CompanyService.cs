using Data.IServices;
using Data.Models.Project;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class CompanyService : ICompanyService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;

        public CompanyService()
        {
            this.compiler = new SqlServerCompiler();
        }

        private QueryFactory CreateQueryFactory()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            return new QueryFactory(this.connection, this.compiler);
        }

        public IEnumerable<Company> Get()
        {
            using (var db = CreateQueryFactory())
            {
                var companies = db.Query("Company").Where("IsDeleted", false).Get<Company>();
                return companies;
            }
        }

        public IEnumerable<Company> Filter(string filter)
        {
            using (var db = CreateQueryFactory())
            {
                var companies = db.Query("Company").WhereTrue("IsActive").WhereFalse("IsDeleted").Where("CompanyName", "LIKE", $"{filter}%")
                    .Get<Company>();
                return companies;
            }
        }

        public dynamic GetPage(int page, int dataPerPage)
        {
            using (var db = CreateQueryFactory())
            {
                var companies = db.Query("Company").Where(new
                {
                    IsDeleted = false
                }).Paginate<Company>(page, dataPerPage);

                return companies;
            }
        }

        public Company GetById(int id)
        {
            using (var db = CreateQueryFactory())
            {
                var brand = db.Query("Company").Where("Id", id).First<Company>();
                return brand;
            }

        }

        public Company GetByGuid(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                var company = db.Query("Company").Where("Guid", guid).FirstOrDefault<Company>();
                return company;
            }

        }

        public void Insert(Company company)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Company").Insert(new
                {
                    CompanyName = company.CompanyName,
                    TaxNumber = company.TaxNumber,
                    TaxOffice = company.TaxOffice,
                    Email = company.Email,
                    Phone = company.Phone,
                    Address = company.Address,
                    CompanyCode = company.CompanyCode,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Guid = Guid.NewGuid(),
                });
            }

        }

        public void Update(Company company)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Company").Where("Guid", company.Guid).Update(new
                {
                    CompanyName = company.CompanyName,
                    TaxNumber = company.TaxNumber,
                    TaxOffice = company.TaxOffice,
                    Email = company.Email,
                    Phone = company.Phone,
                    Address = company.Address,
                    CompanyCode = company.CompanyCode,
                });
            }

        }

        public void Delete(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Company").Where("Guid", guid).Delete();

            }
        }

        public void UpdateIsActive(Guid guid, bool isActive)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Company").Where("Guid", guid).Update(new
                {
                    IsActive = isActive
                });
            }

        }

        public void UpdateDeleted(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Company").Where("Guid", guid).Update(new
                {
                    IsDeleted = true
                });
            }
        }
    }
}
