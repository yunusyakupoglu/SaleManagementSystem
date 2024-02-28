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
    public class PermissionService : IPermissionService
    {
        SqlConnection connection;
        SqlServerCompiler compiler;
        QueryFactory db;

        public PermissionService()
        {
            this.connection = new SqlConnection("Server=DESKTOP-U1O9HJN;Database=SaleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=true;");
            this.compiler = new SqlServerCompiler();
            this.db = new QueryFactory(connection, compiler);
            connection.Open();
        }

        public IEnumerable<Permission> Get()
        {
            var permissions = db.Query("Permission").Where("IsDeleted", false).Get<Permission>();
            return permissions;
        }

        public Permission GetById(int id)
        {
            var permission = db.Query("Permission").Where("Id", id).First<Permission>();
            return permission;
        }

        public Permission GetPermission(Guid guid)
        {
            var permission = db.Query("Permission").Where("UserGuid", guid).FirstOrDefault<Permission>();
            return permission;
        }

        public bool GetPermissionValue(Common.Permission permissionValue)
        {
                Common.PermissionObject prm = permissionValue;
                var permission = db.Query("Permission").Where($"{prm.Description}", true)
                .FirstOrDefault<Permission>();
                return permission != null;
        }


        public void Insert(Permission permission)
        {
            db.Query("Permission").Insert(new
            {
                CanEdit = permission.CanEdit,
                CanInsert = permission.CanInsert,
                CanView = permission.CanView,
                CanDelete = permission.CanDelete,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                IsActive = true,
                Guid = Guid.NewGuid(),
                UserGuid = permission.UserGuid
            });
        }

        public void Update(Permission permission)
        {
            db.Query("Permission").Where("Guid", permission.Guid).Update(new
            {
                CanEdit = permission.CanEdit,
                CanInsert = permission.CanInsert,
                CanView = permission.CanView,
                CanDelete = permission.CanDelete,
            });
        }

        public void Delete(Guid guid)
        {
            db.Query("Permission").Where("Guid", guid).Delete();
        }

        public void UpdateIsActive(Guid guid, bool isActive)
        {
            db.Query("Permission").Where("Guid", guid).Update(new
            {
                IsActive = isActive
            });
        }

        public void UpdateDeleted(Guid guid)
        {
            db.Query("Permission").Where("Guid", guid).Update(new
            {
                IsDeleted = true
            });
        }
    }
}
