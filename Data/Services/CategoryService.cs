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
    public class CategoryService : ICategoryService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;

        public CategoryService()
        {
            this.compiler = new SqlServerCompiler();
        }

        private QueryFactory CreateQueryFactory()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            return new QueryFactory(this.connection, this.compiler);
        }


        public IEnumerable<Category> Get()
        {
            using (var db = CreateQueryFactory())
            {
                var categories = db.Query("Category").Where("IsDeleted", false).Get<Category>();
                return categories;
            }
        }

        public IEnumerable<Category> Filter(string filter)
        {
            using (var db = CreateQueryFactory())
            {
                var units = db.Query("Category").Where("CategoryName", "LIKE", $"{filter}%")
                    .Get<Category>();
                return units;
            }
        }

        public dynamic GetPage(int page, int dataPerPage)
        {
            using (var db = CreateQueryFactory())
            {
                var units = db.Query("Category").Where(new
                {
                    IsDeleted = false
                }).Paginate<Category>(page, dataPerPage);

                return units;
            }
        }

        public Category GetById(int id)
        {
            using (var db = CreateQueryFactory())
            {
                var category = db.Query("Category").Where("Id", id).First<Category>();
                return category;
            }
        }

        public Category GetByGuid(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                var category = db.Query("Category").Where("Guid", guid).FirstOrDefault<Category>();
                return category;
            }
        }

        public void Insert(Category category)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Category").Insert(new
                {
                    CategoryName = category.CategoryName,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Guid = Guid.NewGuid(),
                });
            }
        }

        public void Update(Category category)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Category").Where("Guid", category.Guid).Update(new
                {
                    CategoryName = category.CategoryName,
                });
            }
        }

        public void Delete(Category category)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Category").Where("Guid", category.Guid).Delete();
            }
        }

        public void UpdateIsActive(Guid guid, bool isActive)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Category").Where("Guid", guid).Update(new
                {
                    IsActive = isActive
                });
            }
        }

        public void UpdateDeleted(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Category").Where("Guid", guid).Update(new
                {
                    IsDeleted = true
                });
            }
        }
    }
}
