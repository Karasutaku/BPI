using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Login;

namespace BPIWebApplication.Client.Services.LoginServices
{
    public interface ILoginService
	{
		ActiveUser activeUser { get; set; }

		//Task<ResultModel<ActiveUser<LoginUser>>> GetUserAuthentication(LoginUser data);

		Task<ResultModel<FacadeLoginResponse>> smsApiFacadeLogin(FacadeLogin data);
		Task<ResultModel<List<FacadeUserModuleResp>>> frameworkApiFacadeModule(FacadeUserModule data, string token);
        Task<ResultModel<UserPrivilegesResp>> frameworkApiFacadePrivilege(UserPrivileges data, string token);
    }
}
