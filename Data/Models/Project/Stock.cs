using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Project
{
    public class Stock
    {
        public int Id { get; set; }
        public Guid Unit { get; set; }
        public Guid Product { get; set; }
        public float Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Guid { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
