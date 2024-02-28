using Data.IServices;
using Data.Models;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class RoleService : IRoleService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;
        QueryFactory db;

        public RoleService()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            this.compiler = new SqlServerCompiler();
            this.db = new QueryFactory(connection, compiler);
            connection.Open();
        }

        public IEnumerable<Role> Get()
        {
            var roles = db.Query("Role").Where("IsDeleted", false).Get<Role>();
            return roles;
        }

        public bool CreateRoles()
        {
            var admin = db.Query("Role").Insert(new
            {
                RoleName = "Admin",
                RoleDescription = "Bütün kullanıcı yetkilerini içerir.",
                CanEdit = true,
                CanInsert = true,
                CanView = true,
                CanDelete = true,
                IsActive = true,
                IsDeleted = false,
                Guid = Guid.NewGuid(),
                CreatedDate = DateTime.Now
            });

            var editor = db.Query("Role").Insert(new
            {
                RoleName = "Editor",
                RoleDescription = "Düzenleme ve görüntüleme yetkilerini içerir.",
                CanEdit = true,
                CanInsert = false,
                CanView = true,
                CanDelete = false,
                IsActive = true,
                IsDeleted = false,
                Guid = Guid.NewGuid(),
                CreatedDate = DateTime.Now
            });

            var viewer = db.Query("Role").Insert(new
            {
                RoleName = "Viewer",
                RoleDescription = "Sadece görüntüleme yetkisini içerir.",
                CanEdit = false,
                CanInsert = false,
                CanView = true,
                CanDelete = false,
                IsActive = true,
                IsDeleted = false,
                Guid = Guid.NewGuid(),
                CreatedDate = DateTime.Now
            });

            if (admin != 0 && editor != 0 && viewer != 0)
            {
                return true;
            }
            else return false;
        }

        public Role GetRole(Guid guid)
        {
            var role = db.Query("Role").Where("Guid", guid)
                .FirstOrDefault<Role>();
            return role;
        }
    }
}
