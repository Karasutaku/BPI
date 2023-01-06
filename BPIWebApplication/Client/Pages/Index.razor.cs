using BPIWebApplication.Shared;

namespace BPIWebApplication.Client.Pages
{
    public partial class Index
    {
        ActiveUser<LoginUser> activeUser = new();

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        protected override async Task OnInitializedAsync()
        {
            await ManagementService.GetAllProject();

            activeUser.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.UserLogin = new LoginUser();
            activeUser.UserLogin.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userEmail"));
            activeUser.role = Base64Decode(await sessionStorage.GetItemAsync<string>("role"));
        }

        private void redirectEditProject(string projectName)
        {
            if (activeUser.role.Contains("admin"))
            {
                string temp = Base64Encode(projectName);

                navigate.NavigateTo($"management/editproject/{temp}");
            }
            else
            {
                // do nothing
            }
            
        }


    }
}
