using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Project
{
    public class Auth
    {
        public int Id { get; set; }
        public string IntegratorName { get; set; }
        public string MerchantId { get; set; }
        public string IntegratorCompany { get; set; }
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string SvcCredentials
        {
            get
            {
                return

                   System.Convert.ToBase64String(Encoding.GetEncoding("UTF-8")
                               .GetBytes(ApiKey + ":" + SecretKey));
            }
        }

        public string UserAgent
        {
            get
            {
                return

                    MerchantId + " - " + IntegratorCompany;
            }
        }
        public DateTime CreatedDate { get; set; }
        public Guid Guid { get; set; }
        public Guid UserGuid { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
