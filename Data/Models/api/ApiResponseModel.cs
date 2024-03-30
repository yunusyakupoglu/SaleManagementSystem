using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.api
{
    public class ApiResponseModel
    {
        public HttpStatusCode statusCode { get; set; }
        public string message { get; set; }
    }
}
