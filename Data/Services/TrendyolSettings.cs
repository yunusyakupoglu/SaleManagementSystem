using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class TrendyolSettings
    {
        //string ApiKey = "X0WCclNWvHYRHLA4LdaX";
        //string SecretKey = "ragp7O70nsTXngSDBKC0";
        //public static string ApiKey { get { return "X0WCclNWvHYRHLA4LdaX"; } }
        //public static string SecretKey { get { return "ragp7O70nsTXngSDBKC0"; } }
        public static string MerchantId { get; set; }
        public static string IntegratorCompany { get; set; }
        public static string ApiKey { get; set; }
        public static string SecretKey { get; set; }
        public static string svcCredentials
        {
            get
            {
                return

                   System.Convert.ToBase64String(Encoding.GetEncoding("UTF-8")
                               .GetBytes(ApiKey + ":" + SecretKey));
            }
        }

        public static string UserAgent
        {
            get
            {
                return

                    MerchantId + " - " + IntegratorCompany;
            }
        }
    }
}
