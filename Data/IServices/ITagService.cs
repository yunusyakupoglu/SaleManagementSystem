using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices
{
    public interface ITagService
    {
        IEnumerable<Tag> Get();
        IEnumerable<Tag> GetbyProductGuid(Guid guid);
        Tag GetById(int id);
        Tag GetByGuid(Guid guid);
        void Insert(Tag product);
        void Update(Tag product);
        void Delete(Guid guid);
        void UpdateIsActive(Guid guid, bool isActive);
        void UpdateDeleted(Guid guid);
    }
}
