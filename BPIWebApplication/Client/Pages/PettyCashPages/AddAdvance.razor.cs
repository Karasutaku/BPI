using BPIWebApplication.Shared.MainModel.PettyCash;
using Microsoft.AspNetCore.Components;

namespace BPIWebApplication.Client.Pages.PettyCashPages
{
    public partial class AddAdvance : ComponentBase
    {
        private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();

        private Advance advance = new Advance();
        private List<AdvanceLine> advanceLines = new List<AdvanceLine>();

        private int n = 1;

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

            activeUser.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.UserLogin = new LoginUser();
            activeUser.UserLogin.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userEmail"));
            activeUser.role = Base64Decode(await sessionStorage.GetItemAsync<string>("role"));

            // _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/SopPages/Dashboard.razor.js");
        }

        private void submitAdvance()
        {
            advance.AdvanceID = "ADV" + n;
            advance.AdvanceDate = DateTime.Now;
            advance.Applicant = activeUser.UserLogin.userName;
            advance.LocationID = "Store A";
            advance.AdvanceStatus = "Created";

            PettyCashService.addAdvanceTestData(advance, advanceLines);

            n++;
        }

        private void addLine()
        {
            advanceLines.Add(new AdvanceLine());
        }

        private void deleteLine(AdvanceLine data)
        {
            advanceLines.Remove(data);
        }

    }
}
