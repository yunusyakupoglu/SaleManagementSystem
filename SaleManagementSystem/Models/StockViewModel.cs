using Data.Models.Dto;
using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManagementSystem.Models
{
    public class StockViewModel
    {
        public IEnumerable<StockDto> stocks { get; set; }
        public IEnumerable<Unit> units { get; set; }
        public IEnumerable<Product> products { get; set; }
    }
}