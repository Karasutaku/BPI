namespace BPIWebApplication.Client.Services.LoginServices
{
	public interface ILoginService
	{
		Task<ResultModel<ActiveUser<LoginUser>>> GetUserAuthentication(LoginUser data);
	}
}
