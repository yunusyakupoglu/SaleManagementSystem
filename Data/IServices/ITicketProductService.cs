using Data.Models.Dto;
using Data.Models.Project;
using System;
using System.Collections.Generic;

namespace Data.IServices
{
    public interface ITicketProductService
    {
        IEnumerable<TicketProduct> Get();
        TicketProduct GetById(int id);
        TicketProduct GetByGuid(Guid guid);
        void Insert(TicketProduct ticketProduct);
        IEnumerable<TicketProductResult> InsertRange(IEnumerable<TicketProduct> ticketProducts);
        void Update(TicketProduct ticketProduct);
        void Delete(Guid guid);
        void UpdateIsActive(Guid guid, bool isActive);
        void UpdateDeleted(Guid guid);

    }
}
