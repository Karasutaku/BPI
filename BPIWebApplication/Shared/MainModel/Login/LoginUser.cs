namespace BPIWebApplication.Shared.MainModel.Login
{
    public class LoginUser
    {
        public string userName { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public int companyId { get; set; } = 0;
        public string locationId { get; set; } = string.Empty;
    }
}
