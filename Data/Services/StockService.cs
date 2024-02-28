using Data.IServices;
using Data.Models.Dto;
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
    public class StockService : IStockService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;

        public StockService()
        {
            this.compiler = new SqlServerCompiler();
        }

        private QueryFactory CreateQueryFactory()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            return new QueryFactory(this.connection, this.compiler);
        }

        public IEnumerable<Stock> Get()
        {
            using (var db = CreateQueryFactory())
            {
                var stocks = db.Query("Stock").Where("IsDeleted", false).Get<Stock>();
                return stocks;
            }
        }

        public IEnumerable<StockDto> Filter(string filter)
        {
            using (var db = CreateQueryFactory())
            {
                var stocks = db.Query("StockView").Where("ProductName", "LIKE", $"{filter}%")
                    .OrWhere("BrandName", "LIKE", $"{filter}%")
                    .OrWhere("CategoryName", "LIKE", $"{filter}%")
                    .Get<StockDto>();
                return stocks;
            }
        }

        public dynamic GetPage(int page, int dataPerPage)
        {
            using (var db = CreateQueryFactory())
            {
                var stocks = db.Query("StockView").Where("IsDeleted", false).Paginate<StockDto>(page, dataPerPage);
                return stocks;
            }
        }

        public IEnumerable<StockDto> GetDto()
        {
            using (var db = CreateQueryFactory())
            {
                var query = db.Query("Stock")
    .Where("Stock.IsDeleted", false)
    .Select("Stock.Id", "Stock.Quantity", "Stock.IsDeleted", "Stock.IsActive", "Stock.Guid", "Unit.UnitName as Unit", "Product.ProductName as Product", "Product.ImgName as ImgName", "Brand.BrandName as Brand", "Brand.Guid as BrandGuid", "Unit.Guid as UnitGuid", "Product.Guid as ProductGuid", "Product.SellPrice as SellPrice", "Product.Vat as Vat")
    .Join("Unit", "Stock.Unit", "Unit.Guid")
    .Join("Product", "Stock.Product", "Product.Guid")
.Join("Brand", "Product.Brand", "Brand.Guid");

                var stocks = query.Get<StockDto>();
                return stocks;
            }
        }

        public Stock GetById(int id)
        {
            using (var db = CreateQueryFactory())
            {
                var stock = db.Query("Stock").Where("Id", id).First<Stock>();
                return stock;
            }
        }

        public Stock GetByGuid(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                var stock = db.Query("Stock").Where("Guid", guid).FirstOrDefault<Stock>();
                return stock;
            }
        }

        public StockDto GetDtoByGuid(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                var query = db.Query("Stock")
    .Where("Stock.IsDeleted", false)
    .Where("Stock.Guid", guid)
    .Select("Stock.Id", "Stock.Quantity", "Stock.IsDeleted", "Stock.IsActive", "Stock.Guid", "Unit.UnitName as Unit", "Product.ProductName as Product", "Product.ImgName as ImgName", "Brand.BrandName as Brand", "Brand.Guid as BrandGuid", "Unit.Guid as UnitGuid", "Product.Guid as ProductGuid", "Product.SellPrice as SellPrice", "Product.Vat as Vat")
    .Join("Unit", "Stock.Unit", "Unit.Guid")
    .Join("Product", "Stock.Product", "Product.Guid")
.Join("Brand", "Product.Brand", "Brand.Guid");

                var stock = query.Get<StockDto>().FirstOrDefault();
                return stock;
            }
        }

        public void Insert(Stock stock)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Stock").Insert(new
                {
                    Unit = stock.Unit,
                    Product = stock.Product,
                    Quantity = stock.Quantity,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Guid = Guid.NewGuid(),
                });
            }
        }

        public void Update(Stock stock)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Stock").Where("Guid", stock.Guid).Update(new
                {
                    Unit = stock.Unit,
                    Product = stock.Product,
                    Quantity = stock.Quantity,
                });
            }
        }

        public void UpdateQuantity(Guid Guid, float Quantity)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Stock").Where("Guid", Guid).Update(new
                {
                    Quantity = Quantity,
                });
            }
        }

        public void UpdateQuantityRange(IEnumerable<TicketProductResult> ticketProductResults)
        {
            using (var db = CreateQueryFactory())
            {
                foreach (var item in ticketProductResults)
                {
                    var stock = db.Query("Stock").Where("Guid", item.Stock).FirstOrDefault<Stock>(); ;
                    db.Query("Stock").Where("Guid", item.Stock).Update(new
                    {
                        Quantity = stock.Quantity - item.Quantity
                    });
                }
            }
        }

        public void Delete(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Stock").Where("Guid", guid).Delete();
            }
        }

        public void UpdateIsActive(Guid guid, bool isActive)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Stock").Where("Guid", guid).Update(new
                {
                    IsActive = isActive
                });
            }
        }

        public void UpdateDeleted(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Stock").Where("Guid", guid).Update(new
                {
                    IsDeleted = true
                });
            }
        }
    }
}
