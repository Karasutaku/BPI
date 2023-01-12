using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.MainModel.Login
{
    public class FacadeLogin
    {
        public string userName { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public int companyId { get; set; } = 0;
        public string locationId { get; set; } = string.Empty;
        public string fromApplicationId { get; set; } = string.Empty;
        public string fromApplicationSession { get; set; } = string.Empty;
        public string ipClient { get; set; } = string.Empty;
        public string macAddress { get; set; } = string.Empty;
    }

    public class FacadeLoginResponse
    {
        public string token { get; set; } = string.Empty;
        public string userName { get; set; } = string.Empty;
        public TimeSpan validaty { get; set; } = System.TimeSpan.Zero;
        public string fefreshToken { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public string guidId { get; set; } = string.Empty;
        public DateTime expiredTime { get; set; } = DateTime.Now;
        public string sessionId { get; set; } = string.Empty;
        public int applicationId { get; set; } = 0;
        public int companyId { get; set; } = 0;
        public string locationId { get; set; } = string.Empty;
        public string ip { get; set; } = string.Empty;
        public string macAddress { get; set; } = string.Empty;
    }
}
