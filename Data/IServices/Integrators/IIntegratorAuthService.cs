using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices.Integrators
{
    public interface IIntegratorAuthService
    {
        IEnumerable<Auth> Get();
        IEnumerable<Auth> Filter(string filter);
        dynamic GetPage(int page, int dataPerPage);
        Auth GetById(int id);
        Auth GetByGuid(Guid guid);
        Auth GetByUserGuidAndIntegratorName(Guid userGuid, string integratorName);
        void Insert(Auth auth);
        void Update(Auth auth);
        void Delete(Auth auth);
        void UpdateIsActive(Guid guid, bool isActive);
        void UpdateDeleted(Guid guid);
    }
}
