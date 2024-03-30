using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.api
{
    public class Error
    {
        public string Key { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public string[] Args { get; set; }
    }

    public class ApiResponse
    {
        public long Timestamp { get; set; }
        public string Exception { get; set; }
        public Error[] Errors { get; set; }
        public string BatchRequestId { get; set; }
        public string Message { get; set; }
    }
}
