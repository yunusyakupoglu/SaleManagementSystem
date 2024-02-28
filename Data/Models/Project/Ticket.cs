using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Project
{
    public class Ticket
    {
        public int Id { get; set; }
        public Guid Company { get; set; }
        public decimal SumSellPrice { get; set; }
        public decimal SumVat { get; set; }
        public decimal SumDiscount { get; set; }
        public decimal SubSellPrice { get; set; }
        public decimal NetTotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Guid { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
