using BPIWebApplication.Shared;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BPIWebApplication.Client.Services.LoginServices
{
	public class LoginService : ILoginService
	{
        private readonly HttpClient _http;
        public LoginService(HttpClient http)
        {
            _http = http;
        }
        
        public async Task<ResultModel<ActiveUser<LoginUser>>> GetUserAuthentication(LoginUser data)
        {
            ResultModel<ActiveUser<LoginUser>> resData = new ResultModel<ActiveUser<LoginUser>>();
            
            try
            {
                // check login api user Windows
                var result1 = await _http.PostAsJsonAsync<LoginUser>("api/Login/Authenticate", data);

                // kalau ada, check admin user
                if (result1.IsSuccessStatusCode)
                {
                    var result2 = await _http.PostAsJsonAsync<LoginUser>("api/Login/IsAdmin", data);

                    var respBody = await result2.Content.ReadFromJsonAsync<ResultModel<ActiveUser<LoginUser>>>();

                    if (respBody.isSuccess) {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }
            
            return resData;
        }
    }
}
