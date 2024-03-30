using Data.IServices;
using Data.Models.api;
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
    public class CargoService : ICargoService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;

        public CargoService()
        {
            this.compiler = new SqlServerCompiler();
        }

        private QueryFactory CreateQueryFactory()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            return new QueryFactory(this.connection, this.compiler);
        }


        public void Delete(Cargo cargo)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Cargo").Where("Guid", cargo.Guid).Delete();
            }
        }

        public IEnumerable<Cargo> Filter(string filter)
        {
            using (var db = CreateQueryFactory())
            {
                var cargos = db.Query("Cargo").Where("Name", "LIKE", $"{filter}%")
                    .Get<Cargo>();
                return cargos;
            }
        }

        public IEnumerable<Cargo> Get()
        {
            using (var db = CreateQueryFactory())
            {
                var cargos = db.Query("Cargo").Where("IsDeleted", false).Get<Cargo>();
                return cargos;
            }
        }

        public Cargo GetByGuid(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                var cargo = db.Query("Cargo").Where("Guid", guid).FirstOrDefault<Cargo>();
                return cargo;
            }
        }

        public Cargo GetById(int id)
        {
            using (var db = CreateQueryFactory())
            {
                var cargo = db.Query("Cargo").Where("Id", id).First<Cargo>();
                return cargo;
            }
        }

        public dynamic GetPage(int page, int dataPerPage)
        {
            using (var db = CreateQueryFactory())
            {
                var cargos = db.Query("Cargo").Where(new
                {
                    IsDeleted = false
                }).Paginate<Cargo>(page, dataPerPage);

                return cargos;
            }
        }

        public void Insert(Cargo cargo)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Cargo").Insert(new
                {
                    Name = cargo.Name,
                    Code = cargo.Code,
                    TaxNumber = cargo.TaxNumber,
                    CargoId = cargo.CargoId,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Guid = Guid.NewGuid(),
                });
            }
        }

        public void Update(Cargo cargo)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Cargo").Where("Guid", cargo.Guid).Update(new
                {
                    Name = cargo.Name,
                    Code = cargo.Code,
                    TaxNumber = cargo.TaxNumber,
                    CargoId = cargo.CargoId
                });
            }
        }

        public void UpdateDeleted(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Cargo").Where("Guid", guid).Update(new
                {
                    IsDeleted = true
                });
            }
        }

        public void UpdateIsActive(Guid guid, bool isActive)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Cargo").Where("Guid", guid).Update(new
                {
                    IsActive = isActive
                });
            }
        }
    }
}
