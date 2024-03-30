using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.api
{
    public class StockInventoryRequest
    {
        public decimal Price { get; set; }
        public float Inventory { get; set; }
    }
}
