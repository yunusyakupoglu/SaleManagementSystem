using Data.Models.Dto;
using Data.Models.Project;
using System;
using System.Collections.Generic;

namespace Data.IServices
{
    public interface ITicketService
    {
        IEnumerable<Ticket> Get();
        IEnumerable<TicketView> Filter(string filter);
        dynamic GetPage(int page, int dataPerPage);
        Ticket GetById(int id);
        Ticket GetByGuid(Guid guid);
        Guid Insert(Ticket ticket);
        void Update(Ticket ticket);
        void Delete(Guid guid);
        void UpdateIsActive(Guid guid, bool isActive);
        void UpdateDeleted(Guid guid);

    }
}
