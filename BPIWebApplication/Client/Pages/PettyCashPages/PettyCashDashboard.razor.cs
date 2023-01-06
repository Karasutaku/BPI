using BPIWebApplication.Client.Services.ManagementServices;
using BPIWebApplication.Shared.MainModel.PettyCash;
using BPIWebApplication.Shared.PagesModel.Dashboard;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.PettyCashPages
{
    public partial class PettyCashDashboard : ComponentBase
    {
        private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();

        private Advance advance = new Advance();
        private List<AdvanceLine> advanceLines = new List<AdvanceLine>();
        private Expense expense = new Expense();
        private List<ExpenseLine> ExpenseLines = new List<ExpenseLine>();
        private Reimburse reimburse = new Reimburse();
        private List<ReimburseLine> reimburseLines = new List<ReimburseLine>();

        private bool showModal = false;
        private string transacType = string.Empty;

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

            //PettyCashService.expensesTestData();
            //PettyCashService.advanceTestData();
            //PettyCashService.reimburseTestData();
            // _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/SopPages/Dashboard.razor.js");
        }

        private void modalShow(Advance advData, Expense expData, Reimburse reimData, string type)
        {
            transacType = type;
            showModal = true;

            if (type.Contains("advance"))
            {
                advance = advData;
                
                foreach (var dt in PettyCashService.advanceLines)
                {
                    if (dt.Header.AdvanceID == advance.AdvanceID)
                    {
                        advanceLines.Add(dt);
                    }
                }
            }
            //else if (type.Contains("expense"))
            //{
            //    expense = expData;
            //}
            //else if (type.Contains("reimburse"))
            //{
            //    reimburse = reimData;
            //    expenseReimburse.Clear();

            //    PettyCashService.getExpenseReimburse(reimburse.ReimbursID);

            //    expenseReimburse = PettyCashService.expenseReimburse;
            //}

        }

        private void modalHide() => showModal = false;
    }
}
