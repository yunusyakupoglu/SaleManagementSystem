using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices
{
    public interface IPermissionService
    {
        IEnumerable<Permission> Get();
        Permission GetById(int id);
        Permission GetPermission(Guid guid);
        bool GetPermissionValue(Common.Permission permissionValue);
        void Insert(Permission permission);
        void Update(Permission permission);
        void Delete(Guid guid);
        void UpdateIsActive(Guid guid, bool isActive);
        void UpdateDeleted(Guid guid);
    }
}
