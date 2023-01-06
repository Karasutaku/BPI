using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.Serialization;

namespace BPIWebApplication.Client.Shared
{
    public partial class NavMenu : ComponentBase
    {
        private bool collapseNavMenu = true;
        private bool showSopMenu = true;
        private bool showDigitalizationMenu = false;
        private bool showPettyCashMenu = false;
        private bool showManagementMenu = false;
        
        //private bool showModalTrigger = false;

        private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        //private void showModal() => showModalTrigger = true;
        //private void hideModal() => showModalTrigger = false;

        protected override async Task OnInitializedAsync()
        {
            activeUser.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.role = Base64Decode(await sessionStorage.GetItemAsync<string>("role"));
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        private void toggleSopMenu()
        {
            showSopMenu = !showSopMenu;
        }

        private void toggleDigitalizationMenu()
        {
            showDigitalizationMenu = !showDigitalizationMenu;
        }

        private void togglePettyCashMenu()
        {
            showPettyCashMenu = !showPettyCashMenu;
        }

        private void toggleManagementMenu()
        {
            showManagementMenu = !showManagementMenu;
        }

        private async void confirmLogout()
        {
            await sessionStorage.ClearAsync();

            navigate.NavigateTo("/");
        }
    }
}
