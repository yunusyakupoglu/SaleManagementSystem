using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Dto
{
    public class StockDto
    {
        public int Id { get; set; }
        public string ImgName { get; set; }
        public string ProductName { get; set; }
        public decimal PurchasePrice { get; set; }
        public string UnitName { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryGuid { get; set; }
        public string Formule { get; set; }
        public Guid UnitGuid { get; set; }
        public string BrandName { get; set; }
        public Guid BrandGuid { get; set; }
        public Guid ProductGuid { get; set; }
        public float Quantity { get; set; }
        public decimal SellPrice { get; set; }
        public int Vat { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid Guid { get; set; }
    }
}
