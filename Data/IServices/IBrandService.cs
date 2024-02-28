using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices
{
    public interface IBrandService
    {
        IEnumerable<Brand> Get();
        dynamic GetPage(int page, int dataPerPage);
        IEnumerable<Brand> Filter(string filter);
        Brand GetById(int id);
        Brand GetByGuid(Guid guid);
        void Insert(Brand brand);
        void Update(Brand brand);
        void Delete(Guid guid);
        void UpdateIsActive(Guid guid, bool isActive);
        void UpdateDeleted(Guid guid);
    }
}
