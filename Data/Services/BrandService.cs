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
    public class BrandService : IBrandService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;

        public BrandService()
        {
            this.compiler = new SqlServerCompiler();
        }

        private QueryFactory CreateQueryFactory()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            return new QueryFactory(this.connection, this.compiler);
        }

        public IEnumerable<Brand> Get()
        {
            using (var db = CreateQueryFactory())
            {
                var brands = db.Query("Brand").Where("IsDeleted", false).Get<Brand>();
                return brands;
            }
        }

        public IEnumerable<Brand> Filter(string filter)
        {
            using (var db = CreateQueryFactory())
            {
                var brands = db.Query("Brand").Where("BrandName", "LIKE", $"{filter}%")
                    .Get<Brand>();
                return brands;
            }
        }

        public dynamic GetPage(int page, int dataPerPage)
        {
            using (var db = CreateQueryFactory())
            {
                var brands = db.Query("Brand").Where(new
                {
                    IsDeleted = false
                }).Paginate<Brand>(page, dataPerPage);

                return brands;
            }
        }

        public Brand GetById(int id)
        {
            using (var db = CreateQueryFactory())
            {
                var brand = db.Query("Brand").Where("Id", id).First<Brand>();
                return brand;
            }

        }

        public Brand GetByGuid(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                var brand = db.Query("Brand").Where("Guid", guid).FirstOrDefault<Brand>();
                return brand;
            }

        }

        public void Insert(Brand brand)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Brand").Insert(new
                {
                    ImgUrl = brand.ImgUrl,
                    ImgName = brand.ImgName,
                    BrandName = brand.BrandName,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Guid = Guid.NewGuid(),
                });
            }

        }

        public void Update(Brand brand)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Brand").Where("Guid", brand.Guid).Update(new
                {
                    ImgUrl = brand.ImgUrl,
                    ImgName = brand.ImgName,
                    BrandName = brand.BrandName,
                });
            }

        }

        public void Delete(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Brand").Where("Guid", guid).Delete();

            }
        }

        public void UpdateIsActive(Guid guid, bool isActive)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Brand").Where("Guid", guid).Update(new
                {
                    IsActive = isActive
                });
            }

        }

        public void UpdateDeleted(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Brand").Where("Guid", guid).Update(new
                {
                    IsDeleted = true
                });
            }
        }
    }
}
