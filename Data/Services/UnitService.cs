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
    public class UnitService : IUnitService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;

        public UnitService()
        {
            this.compiler = new SqlServerCompiler();
        }

        private QueryFactory CreateQueryFactory()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            return new QueryFactory(this.connection, this.compiler);
        }

        public IEnumerable<Unit> Get()
        {
            using (var db = CreateQueryFactory())
            {
                var units = db.Query("Unit").Where("IsDeleted", false).Get<Unit>();
                return units;
            }
        }

        public IEnumerable<Unit> Filter(string filter)
        {
            using (var db = CreateQueryFactory())
            {
                var units = db.Query("Unit").Where("UnitName", "LIKE", $"{filter}%")
                    .Get<Unit>();
                return units;
            }
        }

        public dynamic GetPage(int page, int dataPerPage)
        {
            using (var db = CreateQueryFactory())
            {
                var units = db.Query("Unit").Where(new
                {
                    IsDeleted = false
                }).Paginate<Unit>(page, dataPerPage);

                return units;
            }
        }

        public Unit GetById(int id)
        {
            using (var db = CreateQueryFactory())
            {
                var unit = db.Query("Unit").Where("Id", id).First<Unit>();
                return unit;
            }

        }

        public Unit GetByGuid(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                var unit = db.Query("Unit").Where("Guid", guid).FirstOrDefault<Unit>();
                return unit;
            }

        }

        public void Insert(Unit unit)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Unit").Insert(new
                {
                    UnitName = unit.UnitName,
                    Formule = unit.Formule,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Guid = Guid.NewGuid(),
                });
            }

        }

        public void Update(Unit unit)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Unit").Where("Guid", unit.Guid).Update(new
                {
                    UnitName = unit.UnitName,
                    Formule = unit.Formule,
                });
            }

        }

        public void Delete(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Unit").Where("Guid", guid).Delete();

            }
        }

        public void UpdateIsActive(Guid guid, bool isActive)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Unit").Where("Guid", guid).Update(new
                {
                    IsActive = isActive
                });

            }

        }

        public void UpdateDeleted(Guid guid)
        {
            using (var db = CreateQueryFactory())
            {
                db.Query("Unit").Where("Guid", guid).Update(new
                {
                    IsDeleted = true
                });
            }
        }
    }
}
