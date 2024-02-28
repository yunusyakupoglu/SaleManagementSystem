using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices
{
    public interface ICategoryService
    {
        IEnumerable<Category> Get();
        IEnumerable<Category> Filter(string filter);
        dynamic GetPage(int page, int dataPerPage);
        Category GetById(int id);
        Category GetByGuid(Guid guid);
        void Insert(Category category);
        void Update(Category category);
        void Delete(Category category);
        void UpdateIsActive(Guid guid, bool isActive);
        void UpdateDeleted(Guid guid);
    }
}
