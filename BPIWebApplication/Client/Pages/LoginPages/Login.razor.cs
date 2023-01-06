using BPIWebApplication.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.LoginPages
{
	public partial class Login : ComponentBase
	{
		private LoginUser user { get; set; } = new LoginUser();

		private ResultModel<ActiveUser<LoginUser>> data = new ResultModel<ActiveUser<LoginUser>>();

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

            data = await loginService.GetUserAuthentication(user);

			// assign session data if success
			if (data.isSuccess)
			{
                sessionStorage.SetItem("userName", Base64Encode(data.Data.Name));
                sessionStorage.SetItem("userEmail", Base64Encode(data.Data.UserLogin.userName));
				sessionStorage.SetItem("role", Base64Encode(data.Data.role));

                navigate.NavigateTo("/Index");

                isLoginProgress = false;
                loginMessage = "Login Success";
            }
            else
            {
                isLoginProgress = false;
                StateHasChanged();

                data = null;
                user.userName = "";
                user.password = "";

                await _jsModule.InvokeAsync<IJSObjectReference>("showAlert", "Login Failed ! Please relogin");

			}

        }
	}
}
