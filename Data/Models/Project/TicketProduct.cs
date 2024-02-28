using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Project
{
    public class TicketProduct
    {
        //public int Id { get; set; }
        //public Guid Stock { get; set; }
        //public float Quantity { get; set; }
        //public decimal SellPrice { get; set; }
        //public decimal Vat { get; set; }
        //public decimal Discount { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public Guid Ticket { get; set; }
        //public Guid Guid { get; set; }
        //public bool IsDeleted { get; set; }
        //public bool IsActive { get; set; }

        public int Id { get; set; }
        public Guid Guid { get; set; }
        public Guid Stock { get; set; }
        public string BrandName { get; set; }
        public string ProductName { get; set; }
        public string ImgName { get; set; }
        public string UnitName { get; set; }
        public float Quantity { get; set; }
        public decimal SellPrice { get; set; }
        public decimal Vat { get; set; }
        public decimal Discount { get; set; }
        public Guid Ticket { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
