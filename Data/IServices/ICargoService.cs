using Data.Models.api;
using System;
using System.Collections.Generic;

namespace Data.IServices
{
    public interface ICargoService
    {
        IEnumerable<Cargo> Get();
        IEnumerable<Cargo> Filter(string filter);
        dynamic GetPage(int page, int dataPerPage);
        Cargo GetById(int id);
        Cargo GetByGuid(Guid guid);
        void Insert(Cargo cargo);
        void Update(Cargo cargo);
        void Delete(Cargo cargo);
        void UpdateIsActive(Guid guid, bool isActive);
        void UpdateDeleted(Guid guid);
    }
}
