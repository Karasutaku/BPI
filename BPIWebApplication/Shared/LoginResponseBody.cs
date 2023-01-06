using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared
{
    public class LoginResponseBody
    {
        public string token { get; set; } = string.Empty;
        public string createDateTime { get; set; } = string.Empty;
        public string statusCode { get; set; } = string.Empty;
    }
}
