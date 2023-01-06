using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.PagesModel.AddEditUser;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;

namespace BPIWebApplication.Client.Pages.ManagementPages
{
    public partial class AddEditUser
    {
        [Parameter]
        public string? param { get; set; }

        // message trigger flag
        private bool alertTrigger = false;
        private string alertMessage = string.Empty;
        private string alertBody = string.Empty;

        // upload valid submit flag
        private bool uploadTrigger = false;
        private bool successAlert = false;

        private UserAdmin userAdmin = new UserAdmin();
        private UserAdmin editUserAdmin = new UserAdmin();
        private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        protected override async Task OnInitializedAsync()
        {
            //await ManagementService.GetAllBisnisUnit();
            //await ManagementService.GetAllDepartment();

            await ManagementService.GetAllUserAdmin();

            activeUser.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.UserLogin = new LoginUser();
            activeUser.UserLogin.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userEmail"));
            activeUser.role = Base64Decode(await sessionStorage.GetItemAsync<string>("role"));

            // _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/SopPages/Dashboard.razor.js");

        }

        protected override async Task OnParametersSetAsync()
        {
            if (param != null)
            {
                string temp = Base64Decode(param);

                if (ManagementService.users.SingleOrDefault(a => a.UserEmail == temp) != null)
                {
                    editUserAdmin = ManagementService.users.SingleOrDefault(a => a.UserEmail == temp);
                }
                else
                {
                    alertMessage = "Error Fetch User Data";
                    alertBody = "Please retry your activity";
                    alertTrigger = true;
                }

            }
        }

        private async void submitUser()
        {
            try
            {
                if (!await ManagementService.checkUserAdminExisting(userAdmin.UserEmail))
                {
                    QueryModel<UserAdmin> insertData = new QueryModel<UserAdmin>();
                    insertData.Data = new UserAdmin();

                    insertData.Data = userAdmin;
                    insertData.userEmail = activeUser.UserLogin.userName;
                    insertData.userAction = "I";
                    insertData.userActionDate = DateTime.Now;

                    await ManagementService.createNewUserAdmin(insertData);

                    alertMessage = "Add Department Success !";
                    alertBody = "";
                    successAlert = true;

                    ClearInput();
                }
                else
                {
                    alertMessage = "Department Already Exist !";
                    alertBody = "Please check your Department ID";
                    alertTrigger = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private async void editUser()
        {
            try
            {
                QueryModel<UserAdmin> updateData = new QueryModel<UserAdmin>();
                updateData.Data = new UserAdmin();

                updateData.Data = editUserAdmin;
                updateData.userEmail = activeUser.UserLogin.userName;
                updateData.userAction = "U";
                updateData.userActionDate = DateTime.Now;

                await ManagementService.editUser(updateData);

                alertMessage = "Edit Department Success !";
                alertBody = "";
                successAlert = true;

                ClearInput();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ClearInput()
        {

            userAdmin.UserID = "";
            userAdmin.UserEmail = "";
            userAdmin.UserRole = "";

            this.StateHasChanged();
        }

        private void resetTrigger()
        {
            alertTrigger = false;
            this.StateHasChanged();
        }

        private void resetSuccessAlert()
        {
            successAlert = false;
            this.StateHasChanged();
        }

        private void redirectEditUser(string userEmail)
        {
            param = Base64Encode(userEmail);

            navigate.NavigateTo($"management/edituser/{param}");
        }

    }
}
