using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool CanEdit { get; set; }
        public bool CanInsert { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Guid { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
