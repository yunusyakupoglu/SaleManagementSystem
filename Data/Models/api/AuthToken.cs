using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.api
{
    public class AuthToken
    {
        public int Id { get; set; }
        public Guid UserGuid { get; set; }
        public string Token { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid Guid { get; set; }
    }
}
