using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices
{
    public interface ICompanyService
    {
        IEnumerable<Company> Get();
        IEnumerable<Company> Filter(string filter);
        dynamic GetPage(int page, int dataPerPage);
        Company GetById(int id);
        Company GetByGuid(Guid guid);
        void Insert(Company company);
        void Update(Company company);
        void Delete(Guid guid);
        void UpdateIsActive(Guid guid, bool isActive);
        void UpdateDeleted(Guid guid);

    }
}
