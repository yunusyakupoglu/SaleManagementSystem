using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string ImgName { get; set; }
        public string ProductName { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Vat { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid Guid { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
