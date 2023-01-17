using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.MainModel.Login
{
    public class ActiveUser
    {
        public string token { get; set; } = string.Empty;
        public string userName { get; set; } = string.Empty;
        public string company { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
        public string sessionId { get; set; } = string.Empty;
        public int appV { get; set; } = 0;
        public List<string> userPrivileges { get; set; } = new List<string>();
    }

   
}
