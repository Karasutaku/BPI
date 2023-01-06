using BPIWebApplication.Shared;
using BPIWebApplication.Shared.DbModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.ManagementPages
{
    public partial class AddEditDepartment : ComponentBase
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

        private Department department = new Department();
        private Department editDepartment = new Department();
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
            // await ProcedureService.GetAllBisnisUnit();
            // await ProcedureService.GetAllDepartment();

            // filePath = config.GetValue<string>("FilePath:FileUploadPath");

            await ManagementService.GetAllBisnisUnit();
            await ManagementService.GetAllDepartment();

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

                if (ManagementService.departments.SingleOrDefault(a => a.DepartmentID == temp) != null)
                {
                    editDepartment = ManagementService.departments.SingleOrDefault(a => a.DepartmentID == temp);
                }
                else
                {
                    alertMessage = "Error Fetch Department Data";
                    alertBody = "Please retry your activity";
                    alertTrigger = true;
                }

            }
        }

        private async void submitDepartment()
        {
            try
            {
                if (!await ManagementService.checkDepartmentExisting(department.DepartmentID.ToUpper()))
                {
                    QueryModel<Department> insertData = new QueryModel<Department>();
                    insertData.Data = new Department();

                    insertData.Data = department;
                    insertData.userEmail = activeUser.UserLogin.userName;
                    insertData.userAction = "I";
                    insertData.userActionDate = DateTime.Now;

                    await ManagementService.createNewDepartment(insertData);

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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        private async void editDepartmentData()
        {
            try
            {
                QueryModel<Department> updateData = new QueryModel<Department>();
                updateData.Data = new Department();

                updateData.Data = editDepartment;
                updateData.userEmail = activeUser.UserLogin.userName;
                updateData.userAction = "U";
                updateData.userActionDate = DateTime.Now;

                await ManagementService.editDepartment(updateData);

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

            department.DepartmentID = "";
            department.DepartmentName = "";
            department.DepartmentLabel = "";
            department.BisnisUnit = ManagementService.bisnisUnits[0];

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

        private void redirectEditDepartment(string deptID)
        {
            param = Base64Encode(deptID);

            navigate.NavigateTo($"management/editdepartment/{param}");
        }

    }
}
