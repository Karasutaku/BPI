using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.MainModel.Login
{
    // login

    public class FacadeLogin
    {
        public string userName { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public int companyId { get; set; } = 0;
        public string locationId { get; set; } = string.Empty;
        public int fromApplicationId { get; set; } = 0;
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
        public string emailId { get; set; } = string.Empty;
        public Guid guidId { get; set; } = Guid.NewGuid();
        public DateTime expiredTime { get; set; } = DateTime.Now;
        public string sessionId { get; set; } = string.Empty;
        public int? applicationId { get; set; } = 0;
        public int? companyId { get; set; } = 0;
        public string locationId { get; set; } = string.Empty;
        public string ip { get; set; } = string.Empty;
        public string macAddress { get; set; } = string.Empty;
    }

    // user module

    public class SoapHeader
    {
        public string sessionid { get; set; } = string.Empty;
        public string macaddress { get; set; } = string.Empty;
        public string ipclient { get; set; } = string.Empty;
        public int? applicationid { get; set; } = 0;
        public string locationid { get; set; } = string.Empty;
        public int companyid { get; set; } = 0;

        public bool isactive(DateTime lastact)
        {
            TimeSpan ts = DateTime.Now - lastact;
            if (ts.TotalMinutes > 300)
                return false;
            else
            {
                return true;
            }
        }
    }

    public class FacadeUserModule
    {
        public SoapHeader SoapHeader { get; set; } = new SoapHeader();
        public int? CompanyId { get; set; } = 0;
        public string LocationId { get; set; } = string.Empty;
        public string ModuleTypeId { get; set; } = string.Empty;
        public int ApplicationId { get; set; } = 0;
        public string UserName { get; set; } = string.Empty;
    }

    public class FacadeUserModuleResp
    {
        public int? applicationId { get; set; } = 0;
        public string applicationName { get; set; } = string.Empty;
        public int? childApplicationId { get; set; } = 0;
        public string childApplicationName { get; set; } = string.Empty;
        public int moduleCategoryId { get; set; } = 0;
        public string moduleCategoryName { get; set; } = string.Empty;
        public int? moduleId { get; set; } = 0;
        public string moduleName { get; set; } = string.Empty;
        public string moduleTypeId { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string icon { get; set; } = string.Empty;
    }

    public class ModuleCategory
    {
        public int moduleCategoryId { get; set; } = 0;
        public string moduleCategoryName { get; set; } = string.Empty;
    }

    public class userLocationParam
    {
        public string SessionId { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public string IpClient { get; set; } = string.Empty;
        public int ApplicationId { get; set; } = 0;
        public string LocationId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CompanyId { get; set; } = 0;
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 0;
    }

    public class Privileges
    {
        public string? privilegeId { get; set; } = string.Empty;
        public string? privilegeName { get; set; } = string.Empty;
        public string? orderNo { get; set; } = string.Empty;
    }

    public class UserPrivileges
    {
        public int? moduleId { get; set; } = 0;
        public string UserName { get; set; } = string.Empty;
        public userLocationParam userLocationParam { get; set; } = new();
        public List<Privileges> privileges { get; set; } = new();

    }

    public class UserPrivilegesResp
    {
        public List<Privileges> privileges { get; set; } = new();
        public string? errorMessage { get; set; } = string.Empty;
    }

}
