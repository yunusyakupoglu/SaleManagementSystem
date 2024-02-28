using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices
{
    public interface IUserService
    {
        IEnumerable<User> Get();
        User GetById(int id);
        User GetByGuid(Guid guid);
    }
}
