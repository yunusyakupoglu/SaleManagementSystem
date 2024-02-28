using Data.Models.Dto;
using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices
{
    public interface IStockService
    {
        IEnumerable<Stock> Get();
        IEnumerable<StockDto> GetDto();
        IEnumerable<StockDto> Filter(string filter);
        dynamic GetPage(int page, int dataPerPage);
        Stock GetById(int id);
        Stock GetByGuid(Guid guid);
        StockDto GetDtoByGuid(Guid guid);
        void Insert(Stock stock);
        void Update(Stock stock);
        void UpdateQuantity(Guid Guid, float Quantity);
        void UpdateQuantityRange(IEnumerable<TicketProductResult> ticketProductResults);
        void Delete(Guid guid);
        void UpdateIsActive(Guid guid, bool isActive);
        void UpdateDeleted(Guid guid);

    }
}
