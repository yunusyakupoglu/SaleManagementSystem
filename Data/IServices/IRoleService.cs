using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices
{
    public interface IRoleService
    {
        IEnumerable<Role> Get();
        Role GetRole(Guid guid);
        bool CreateRoles();
    }
}
