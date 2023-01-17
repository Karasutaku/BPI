using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.PettyCash;
using BPIWebApplication.Shared.PagesModel.ApplyProcedure;
using Microsoft.AspNetCore.Components;

namespace BPIWebApplication.Client.Pages.PettyCashPages
{
    public partial class AddReimbursment : ComponentBase
    {
        //private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();

        private Reimburse reimbursment = new Reimburse();
        private List<Expense> expense = new List<Expense>();

        private bool triggerModal = false;

        private List<Expense> selectedExpense = new List<Expense>();

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

            // _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/SopPages/Dashboard.razor.js");
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        PettyCashService.expensesTestData();
        //    }
        //}

        private void newExpense()
        {
            expense.Add(new Expense());
        }

        private void submitReimbursment()
        {
            
        }

        private void removeExpense(Expense data)
        {
            expense.Remove(data);

            var itemRemove = selectedExpense.SingleOrDefault(a => a.ExpenseID == data.ExpenseID);

            if (itemRemove != null)
            {
                selectedExpense.Remove(itemRemove);
            }
        }

        private void hideModal()
        {
            selectedExpense.Clear();
            triggerModal = false;
        }

        private void showModal()
        {
            triggerModal = true;
        }

        private void appendExpenseSelected(Expense data)
        {
            if (selectedExpense.FirstOrDefault(a => a.ExpenseID == data.ExpenseID) == null)
            {
                selectedExpense.Add(data);
            }
            else
            {
                var itemRemove1 = selectedExpense.SingleOrDefault(a => a.ExpenseID == data.ExpenseID);
                //var itemRemove2 = expense.SingleOrDefault(a => a.ExpenseID == data.ExpenseID);

                if (itemRemove1 != null)
                {
                    selectedExpense.Remove(itemRemove1);
                }

                //if (itemRemove2 != null)
                //{
                //    expense.Remove(itemRemove2);
                //}

            }
        }

        private void applySelectedExpense()
        {
            foreach (var a in selectedExpense)
            {
                if (expense.FirstOrDefault(x => x.ExpenseID == a.ExpenseID) == null)
                {
                    expense.Add(a);
                }
            }
            triggerModal = false;
        }


    }
}
