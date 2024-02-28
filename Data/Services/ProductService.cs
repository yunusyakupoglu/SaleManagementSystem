using Data.IServices;
using Data.Models.Dto;
using Data.Models.Project;
using SqlKata;
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
    public class ProductService : IProductService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;

        public ProductService()
        {
            this.compiler = new SqlServerCompiler();
        }

        private QueryFactory CreateQueryFactory()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            return new QueryFactory(this.connection, this.compiler);
        }

        public IEnumerable<Product> Get()
        {
            using (var db = CreateQueryFactory())
            {
                var products = db.Query("Product").Where("IsDeleted", false).Get<Product>();
                return products;
            }
        }

        public IEnumerable<ProductWithTags> GetProcedure()
        {


            using (var db = CreateQueryFactory())
            {
                var products = db.Select<ProductWithTags>("ProductWithTags");
                return products;
            }
        }

        public IEnumerable<ProductWithTags> Filter(string filter)
        {
            using (var db = CreateQueryFactory())
            {
                var products = db.Query("ProductView").Where("ProductName", "LIKE", $"{filter}%")
                    .Get<ProductWithTags>();
                return products;
            }
        }

        public dynamic GetPage(int page, int dataPerPage)
        {
            using (var db = CreateQueryFactory())
            {
                var products = db.Query("ProductView").Where("IsDeleted",false).Paginate<ProductWithTags>(page, dataPerPage);
                return products;
            }
        }

        public IEnumerable<ProductDto> GetDto()
        {
            using (var db = CreateQueryFactory())
            {
                var query = db.Query("Product")
.Where("Product.IsDeleted", false)
.Select("Product.Id", "Product.ImgUrl", "Product.ImgName", "Product.ProductName", "Product.PurchasePrice", "Product.SellPrice", "Product.Vat", "Product.IsDeleted", "Product.IsActive", "Product.Guid", "Brand.BrandName as Brand", "Category.CategoryName as Category")
.Join("Brand", "Product.Brand", "Brand.Guid")
.Join("Category", "Product.Category", "Category.Guid");

                var products = query.Get<ProductDto>();
                return products;
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            using (var db = CreateQueryFactory())
            {
                var query = db.Query("Product").Where("SellPrice", ">", 205).OrderBy("ProductName").Get<Product>();
                return query;
            }
        }

        public Product GetById(int id)
        {
            using (var db = CreateQueryFactory())
            {
                var product = db.Query("Product").Where("Id", id).First<Product>();
                return product;
            }
        }

        public Product GetByGuid(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                var product = db.Query("Product").Where("Guid", guid).FirstOrDefault<Product>();
                return product;
            }
        }

        public void Insert(Product product)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Product").Insert(new
                {
                    Brand = product.Brand,
                    Category = product.Category,
                    ProductName = product.ProductName,
                    PurchasePrice = product.PurchasePrice,
                    SellPrice = product.SellPrice,
                    Vat = product.Vat,
                    ImgName = product.ImgName,
                    ImgUrl = product.ImgUrl,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Guid = Guid.NewGuid(),
                });
            }
        }

        public void Update(Product product)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Product").Where("Guid", product.Guid).Update(new
                {
                    Brand = product.Brand,
                    Category = product.Category,
                    ProductName = product.ProductName,
                    PurchasePrice = product.PurchasePrice,
                    SellPrice = product.SellPrice,
                    Vat = product.Vat,
                    ImgName = product.ImgName,
                    ImgUrl = product.ImgUrl,
                });
            }
        }

        public void Delete(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Product").Where("Guid", guid).Delete();
            }
        }

        public void UpdateIsActive(Guid guid, bool isActive)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Product").Where("Guid", guid).Update(new
                {
                    IsActive = isActive
                });
            }
        }

        public void UpdateDeleted(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Product").Where("Guid", guid).Update(new
                {
                    IsDeleted = true
                });
            }
        }
    }
}
