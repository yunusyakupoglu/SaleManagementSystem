using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Dto
{
    public class TicketView
    {
        public Guid Guid { get; set; }
        public decimal NetTotalPrice { get; set; }
        public decimal SumDiscount { get; set; }
        public string UniqueTicketCode { get; set; }
        public Guid Company { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public string IsDeleted { get; set; }
    }
}
