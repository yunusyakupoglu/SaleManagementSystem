using Data.Models.Dto;
using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices
{
    public interface IProductService
    {
        IEnumerable<Product> Get();
        IEnumerable<ProductWithTags> Filter(string filter);
        dynamic GetPage(int page, int dataPerPage);
        IEnumerable<ProductWithTags> GetProcedure();
        IEnumerable<ProductDto> GetDto();
        IEnumerable<Product> GetProducts();
        Product GetById(int id);
        Product GetByGuid(Guid guid);
        void Insert(Product product);
        void Update(Product product);
        void Delete(Guid guid);
        void UpdateIsActive(Guid guid, bool isActive);
        void UpdateDeleted(Guid guid);
    }
}
