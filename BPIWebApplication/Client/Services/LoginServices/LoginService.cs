using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Login;
using Irony.Parsing;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BPIWebApplication.Client.Services.LoginServices
{
    public class LoginService : ILoginService
	{
        private readonly HttpClient _http;

        public ActiveUser activeUser { get; set; } = new();

        public LoginService(HttpClient http)
        {
            _http = http;
        }

        //public async Task<ResultModel<ActiveUser<LoginUser>>> GetUserAuthentication(LoginUser data)
        //{
        //    ResultModel<ActiveUser<LoginUser>> resData = new ResultModel<ActiveUser<LoginUser>>();
            
        //    try
        //    {
        //        // check login api user Windows
        //        var result1 = await _http.PostAsJsonAsync<LoginUser>("api/Login/Authenticate", data);

        //        // kalau ada, check admin user
        //        if (result1.IsSuccessStatusCode)
        //        {
        //            var result2 = await _http.PostAsJsonAsync<LoginUser>("api/Login/IsAdmin", data);

        //            var respBody = await result2.Content.ReadFromJsonAsync<ResultModel<ActiveUser<LoginUser>>>();

        //            if (respBody.isSuccess) {
        //                resData.Data = respBody.Data;
        //                resData.isSuccess = respBody.isSuccess;
        //                resData.ErrorCode = respBody.ErrorCode;
        //                resData.ErrorMessage = respBody.ErrorMessage;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resData.Data = null;
        //        resData.isSuccess = false;
        //        resData.ErrorCode = "99";
        //        resData.ErrorMessage = ex.Message;
        //    }
            
        //    return resData;
        //}

        public async Task<ResultModel<FacadeLoginResponse>> smsApiFacadeLogin(FacadeLogin data)
        {
            ResultModel<FacadeLoginResponse> resData = new ResultModel<FacadeLoginResponse>();

            try
            {
                var result = await _http.PostAsJsonAsync<FacadeLogin>($"api/endUser/Login/Authenticate", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<FacadeLoginResponse>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                    else
                    {
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

        public async Task<ResultModel<List<FacadeUserModuleResp>>> frameworkApiFacadeModule(FacadeUserModule data, string token)
        {
            ResultModel<List<FacadeUserModuleResp>> resData = new ResultModel<List<FacadeUserModuleResp>>();

            try
            {
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await _http.PostAsJsonAsync<FacadeUserModule>("api/endUser/Login/getUserModule", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<FacadeUserModuleResp>>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                    else
                    {
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

        public async Task<ResultModel<UserPrivilegesResp>> frameworkApiFacadePrivilege(UserPrivileges data, string token)
        {
            ResultModel<UserPrivilegesResp> resData = new ResultModel<UserPrivilegesResp>();

            try
            {
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var result = await _http.PostAsJsonAsync<UserPrivileges>($"api/endUser/Login/getUserPrivileges", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<UserPrivilegesResp>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                    else
                    {
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
