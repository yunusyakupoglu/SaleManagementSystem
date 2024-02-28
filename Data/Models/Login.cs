using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Login
    {
        public Guid Role { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string NameSurname { get; set; }
    }
}
