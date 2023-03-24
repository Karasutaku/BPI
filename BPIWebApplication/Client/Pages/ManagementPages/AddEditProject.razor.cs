using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.AddEditProject;
using BPIWebApplication.Shared.PagesModel.AddEditUser;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;

namespace BPIWebApplication.Client.Pages.ManagementPages
{
    public partial class AddEditProject : ComponentBase
    {
        [Parameter]
        public string? param { get; set; }

        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        // message trigger flag
        private bool alertTrigger = false;
        private string alertMessage = string.Empty;
        private string alertBody = string.Empty;

        // upload valid submit flag
        private bool uploadTrigger = false;
        private bool successAlert = false;

        private Project project = new Project();
        private Project editProject = new Project();
        //private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();

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
            if (!LoginService.activeUser.userPrivileges.IsNullOrEmpty())
                LoginService.activeUser.userPrivileges.Clear();

            if (syncSessionStorage.ContainKey("PagePrivileges"))
                syncSessionStorage.RemoveItem("PagePrivileges");

            string tkn = syncSessionStorage.GetItem<string>("token");

            if (syncSessionStorage.ContainKey("userName"))
            {
                privilegeDataParam.moduleId = Convert.ToInt32(Base64Decode(syncSessionStorage.GetItem<string>("ModuleId")));
                privilegeDataParam.UserName = Base64Decode(syncSessionStorage.GetItem<string>("userName"));
                privilegeDataParam.userLocationParam = new();
                privilegeDataParam.userLocationParam.SessionId = syncSessionStorage.GetItem<string>("SessionId");
                privilegeDataParam.userLocationParam.MacAddress = "";
                privilegeDataParam.userLocationParam.IpClient = "";
                privilegeDataParam.userLocationParam.ApplicationId = Convert.ToInt32(Base64Decode(syncSessionStorage.GetItem<string>("AppV")));
                privilegeDataParam.userLocationParam.LocationId = Base64Decode(syncSessionStorage.GetItem<string>("CompLoc")).Split("_")[1];
                privilegeDataParam.userLocationParam.Name = Base64Decode(syncSessionStorage.GetItem<string>("userName"));
                privilegeDataParam.userLocationParam.CompanyId = Convert.ToInt32(Base64Decode(syncSessionStorage.GetItem<string>("CompLoc")).Split("_")[0]);
                privilegeDataParam.userLocationParam.PageIndex = 1;
                privilegeDataParam.userLocationParam.PageSize = 100;
                privilegeDataParam.privileges = new();
            }

            var res = await LoginService.frameworkApiFacadePrivilege(privilegeDataParam, tkn);

            userPriv.Clear();

            if (res.isSuccess)
            {
                if (res.Data.privileges.Any())
                {
                    foreach (var priv in res.Data.privileges)
                    {
                        userPriv.Add(priv.privilegeId);
                    }
                }

                syncSessionStorage.SetItem("PagePrivileges", userPriv);

                LoginService.activeUser.userPrivileges = userPriv;

            }
            //

            activeUser.token = await sessionStorage.GetItemAsync<string>("token");
            activeUser.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.company = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0];
            activeUser.location = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
            activeUser.sessionId = await sessionStorage.GetItemAsync<string>("SessionId");
            activeUser.appV = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));
            activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");

            LoginService.activeUser.userPrivileges = activeUser.userPrivileges;

            //activeUser.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            //activeUser.UserLogin = new LoginUser();
            //activeUser.UserLogin.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userEmail"));
            //activeUser.role = Base64Decode(await sessionStorage.GetItemAsync<string>("role"));

            // _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/SopPages/Dashboard.razor.js");

        }

        protected override async Task OnParametersSetAsync()
        {
            if (param != null)
            {
                string temp = Base64Decode(param);

                if (ManagementService.projects.SingleOrDefault(a => a.ProjectName == temp) != null)
                {
                    editProject = new();
                    editProject = ManagementService.projects.SingleOrDefault(a => a.ProjectName == temp);
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
                    insertData.userEmail = activeUser.userName;
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

        private async void editProjectData()
        {
            try
            {
                QueryModel<Project> updateData = new QueryModel<Project>();
                updateData.Data = new Project();

                updateData.Data = project;
                updateData.userEmail = activeUser.userName;
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
