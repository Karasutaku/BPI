using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.PagesModel.AddEditProject;
using BPIWebApplication.Shared.PagesModel.AddEditUser;
using Microsoft.AspNetCore.Components;

namespace BPIWebApplication.Client.Pages.ManagementPages
{
    public partial class AddEditProject
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

        private Project project = new Project();
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

                if (ManagementService.projects.SingleOrDefault(a => a.ProjectName == temp) != null)
                {
                    project = ManagementService.projects.SingleOrDefault(a => a.ProjectName == temp);
                }
                else
                {
                    alertMessage = "Error Fetch User Data";
                    alertBody = "Please retry your activity";
                    alertTrigger = true;
                }

            }
        }

        private async void submitProject()
        {
            try
            {
                await ManagementService.GetAllProject();

                if (!await ManagementService.checkProjectExisting(project.ProjectName))
                {
                    QueryModel<Project> insertData = new QueryModel<Project>();
                    insertData.Data = new Project();

                    insertData.Data = project;
                    insertData.userEmail = activeUser.UserLogin.userName;
                    insertData.userAction = "I";
                    insertData.userActionDate = DateTime.Now;

                    await ManagementService.createNewProject(insertData);

                    alertMessage = "Add Project Success !";
                    alertBody = "";
                    successAlert = true;

                    ClearInput();
                }
                else
                {
                    alertMessage = "Project Already Exist !";
                    alertBody = "Please check your Project Name";
                    alertTrigger = true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private async void editProject()
        {
            try
            {
                QueryModel<Project> updateData = new QueryModel<Project>();
                updateData.Data = new Project();

                updateData.Data = project;
                updateData.userEmail = activeUser.UserLogin.userName;
                updateData.userAction = "U";
                updateData.userActionDate = DateTime.Now;

                await ManagementService.editProject(updateData);

                alertMessage = "Edit Project Success !";
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

            project.ProjectName = "";
            project.ProjectStatus = "";
            project.ProjectNote = "";

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

    }
}
