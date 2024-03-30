using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.api
{
    public class Cargo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string TaxNumber { get; set; }
        public int CargoId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
