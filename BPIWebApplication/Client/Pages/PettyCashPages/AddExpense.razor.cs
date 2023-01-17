using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.PettyCash;
using Microsoft.AspNetCore.Components;

namespace BPIWebApplication.Client.Pages.PettyCashPages
{
    public partial class AddExpense : ComponentBase
    {
        //private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();

        private Expense expense = new Expense();
        private Advance selectedAdvance = new Advance();

        private bool pdfInputFile = false;
        private bool cameraInputFile = true;

        private bool showModal = false;

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

            //activeUser.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            //activeUser.UserLogin = new LoginUser();
            //activeUser.UserLogin.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userEmail"));
            //activeUser.role = Base64Decode(await sessionStorage.GetItemAsync<string>("role"));

            //PettyCashService.advanceTestData();
            // _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/SopPages/Dashboard.razor.js");
        }

        private void modalHide() => showModal = false;

        private void selectAdvance()
        {
            showModal = true;
        }

        private void selectAdvanceItem(Advance data)
        {
            selectedAdvance = data;

            //expense.AdvanceID = selectedAdvance.AdvanceID;
            //expense.ExpenseDetail = selectedAdvance.AdvanceDetail;
            //expense.ExpenseAmount = selectedAdvance.AdvanceAmount;
        }

        private void submitExpense()
        {
            //
        }

        private void triggerCamera()
        {
            pdfInputFile = false;
            cameraInputFile = true;
        }

        private void triggerPDF()
        {
            pdfInputFile = true;
            cameraInputFile = false;
        }

        private void resetInput()
        {
            //expense.ExpenseID = string.Empty;
            //expense.AdvanceID = string.Empty;
            //expense.ExpenseDetail = string.Empty;
            //expense.ExpenseAmount = decimal.Zero;

            selectedAdvance = new();
        }


    }
}
