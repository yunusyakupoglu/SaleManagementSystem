using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices
{
    public interface IUnitService
    {
        IEnumerable<Unit> Get();
        IEnumerable<Unit> Filter(string filter);
        dynamic GetPage(int page, int dataPerPage);
        Unit GetById(int id);
        Unit GetByGuid(Guid guid);
        void Insert(Unit unit);
        void Update(Unit unit);
        void Delete(Guid guid);
        void UpdateIsActive(Guid guid, bool isActive);
        void UpdateDeleted(Guid guid);
    }
}
