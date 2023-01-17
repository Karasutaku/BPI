using BPIWebApplication.Shared.MainModel.Login;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net;
using Microsoft.IdentityModel.Tokens;

namespace BPIWebApplication.Client.Pages.LoginPages
{
    public partial class Login : ComponentBase
	{
		private LoginUser user { get; set; } = new LoginUser();

		//private ResultModel<ActiveUser<LoginUser>> data = new ResultModel<ActiveUser<LoginUser>>();

        private FacadeLogin loginData = new FacadeLogin();
        //private FacadeUserModule moduleData = new();

        private bool isLoginProgress = false;
        private string loginMessage = string.Empty;

        private IJSObjectReference _jsModule;

        protected override async Task OnInitializedAsync()
        {
            isLoginProgress = false;

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/LoginPages/Login.razor.js");
        }
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        async Task checkLogin()
		{
            isLoginProgress = true;

            //data = await loginService.GetUserAuthentication(user);

            loginData.userName = user.userName;
            loginData.password = user.password;
            loginData.companyId = user.companyId;
            loginData.locationId = user.locationId;
            loginData.fromApplicationId = 38;
            loginData.fromApplicationSession = "";
            loginData.ipClient = "";
            loginData.macAddress = "";

            facadeLogin(loginData);

			// assign session data if success
			//if (data.isSuccess)
			//{
   //             sessionStorage.SetItem("userName", Base64Encode(data.Data.Name));
   //             sessionStorage.SetItem("userEmail", Base64Encode(data.Data.UserLogin.userName));
   //             sessionStorage.SetItem("role", Base64Encode(data.Data.role));

   //             navigate.NavigateTo("/Index");

   //             isLoginProgress = false;
   //             loginMessage = "Login Success";
   //         }
   //         else
   //         {
   //             isLoginProgress = false;
   //             StateHasChanged();

   //             data = null;
   //             user.userName = "";
   //             user.password = "";

   //             await _jsModule.InvokeAsync<IJSObjectReference>("showAlert", "Login Failed ! Please relogin");

			//}

        }

        async void facadeLogin(FacadeLogin data)
        {
            try
            {
                var dt = await loginService.smsApiFacadeLogin(data);

                if (dt.isSuccess)
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(dt.Data.token);
                    var tokenz = jsonToken as JwtSecurityToken;

                    // claims

                    string email = tokenz.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                    string compLoc = tokenz.Claims.FirstOrDefault(c => c.Type.Equals("CompLoc")).Value;
                    string sessionId = tokenz.Claims.FirstOrDefault(c => c.Type.Equals("SessionId")).Value;
                    string appV = tokenz.Claims.FirstOrDefault(c => c.Type.Equals("AppV")).Value;

                    loginService.activeUser.token = dt.Data.token;
                    loginService.activeUser.userName = email;
                    loginService.activeUser.company = compLoc.Split("_")[0];
                    loginService.activeUser.location = compLoc.Split("_")[1];
                    loginService.activeUser.sessionId = sessionId;
                    loginService.activeUser.appV = Convert.ToInt32(appV);

                    sessionStorage.SetItem("token", dt.Data.token);
                    sessionStorage.SetItem("userName", Base64Encode(email));
                    sessionStorage.SetItem("CompLoc", Base64Encode(compLoc));
                    sessionStorage.SetItem("SessionId", sessionId);
                    sessionStorage.SetItem("AppV", Base64Encode(appV));

                    // module

                    //moduleData.SoapHeader.sessionid = sessionId;
                    //moduleData.SoapHeader.macaddress = ""; //
                    //moduleData.SoapHeader.ipclient = ""; //
                    //moduleData.SoapHeader.applicationId = Convert.ToInt32(appV); //
                    //moduleData.SoapHeader.locationid = compLoc.Split("_")[1];
                    //moduleData.SoapHeader.companyid = Convert.ToInt32(compLoc.Split("_")[0]);
                    //moduleData.CompanyId = Convert.ToInt32(compLoc.Split("_")[0]);
                    //moduleData.LocationId = compLoc.Split("_")[1];

                    //if (user.locationId.IsNullOrEmpty())
                    //{
                    //    moduleData.ModuleTypeId = "CMP";
                    //}
                    //else
                    //{
                    //    moduleData.ModuleTypeId = "LOC";
                    //}

                    //moduleData.ApplicationId = Convert.ToInt32(appV); //
                    //moduleData.UserName = email;

                    navigate.NavigateTo("/Index");

                    isLoginProgress = false;
                    loginMessage = "Login Success";

                }
                else
                {
                    isLoginProgress = false;

                    user.userName = "";
                    user.password = "";
                    user.companyId = 0;
                    user.locationId = "";

                    StateHasChanged();

                    await _jsModule.InvokeAsync<IJSObjectReference>("showAlert", "Login Failed ! Please relogin");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        private void formClear()
        {
            user.userName = "";
            user.password = "";
            user.companyId = 0;
            user.locationId = "";
        }

	}
}
