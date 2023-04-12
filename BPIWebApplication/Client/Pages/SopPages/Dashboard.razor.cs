using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.PagesModel.Dashboard;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.Procedure;
using BPIWebApplication.Client.Services.LoginServices;
using Microsoft.IdentityModel.Tokens;

namespace BPIWebApplication.Client.Pages.SopPages
{
    public partial class Dashboard : ComponentBase
    {
        private string FilterProcedure { get; set; } = string.Empty;
        private string bisnisUnitSelect { get; set; } = string.Empty;
        private string departmentSelect { get; set; } = string.Empty;

        private bool filterActive = false;
        DashboardFilter filterDetails = new DashboardFilter();

        private Byte[] streamdata = new Byte[0];

        private bool showModal = false;

        public string bisnisUnitSelected () {
            return bisnisUnitSelect;
        }
        public string departmentSelected () {
            return departmentSelect;
        }

        //public bool isVisible (DepartmentProcedure departmentProcedure)
        //{
        //    if (string.IsNullOrEmpty(FilterProcedure) && string.IsNullOrEmpty(departmentSelected()))
        //        return true;

        //    if (departmentProcedure.DepartmentID.Contains(departmentSelected(), StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(FilterProcedure))
        //    {
        //        return true;
        //    } else if (departmentProcedure.DepartmentID.Contains(departmentSelected(), StringComparison.OrdinalIgnoreCase) && (departmentProcedure.ProcedureNo.Contains(FilterProcedure, StringComparison.OrdinalIgnoreCase) || departmentProcedure.Procedure.ProcedureName.Contains(FilterProcedure, StringComparison.OrdinalIgnoreCase)))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        private IJSObjectReference _jsModule;
        private int pageActive, numberofPage;

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
                syncSessionStorage.RemoveItem("ModuleId");
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

            string loc = activeUser.location.Equals("") ? "HO" : activeUser.location;

            await ManagementService.GetAllBisnisUnit(Base64Encode(loc));
            await ManagementService.GetAllDepartment(Base64Encode(loc));
            pageActive = 1;
            string temp = activeUser.location + "!_!" + pageActive.ToString();
            await ProcedureService.GetDepartmentProcedurewithPaging(Base64Encode(temp));

            numberofPage = await ProcedureService.getDepartmentProcedureNumberofPage(Base64Encode(loc));

            filterActive = false;
            filterDetails = new DashboardFilter();

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/SopPages/Dashboard.razor.js");

        }

        private string? param;

        //private void modalShow() => showModal = true;
        private void modalHide() => showModal = false;

        private Stream GetFileStream(byte[] data)
        {
            var fileBinData = data;
            var fileStream = new MemoryStream(fileBinData);

            return fileStream;
        }

        private async Task handleDownload(string path, string procNo, string procName)
        {
            var temp = path + "!_!" + procNo;

            var dt = await ProcedureService.GetFile(temp);

            streamdata = new Byte[0];

            if (!dt.isSuccess)
            {
                // alert file download not found
            }
            else
            {
                // download file

                //var filestream = GetFileStream(dt.Data.content);
                //string filename = procNo + ".pdf";

                streamdata = dt.Data.content;

                showModal = true;

                StateHasChanged();

                //using var streamRef = new DotNetStreamReference(stream: filestream);

                //await _jsModule.InvokeVoidAsync("downloadFileFromStream", filename, streamRef);
                //await _jsModule.InvokeVoidAsync("showAlert", $"File {procNo} Downloaded");


                // insert history data
                try
                {
                    QueryModel<HistoryAccess> historyData = new QueryModel<HistoryAccess>();
                    historyData.Data = new HistoryAccess();

                    historyData.Data.ProcedureNo = procNo;
                    historyData.Data.ProcedureName = procName;
                    historyData.Data.UserEmail = activeUser.userName;
                    historyData.Data.HistoryAccessDate = DateTime.Now;
                    historyData.userEmail = activeUser.userName;
                    historyData.userAction = "I";
                    historyData.userActionDate = DateTime.Now;

                    await ProcedureService.createHistoryAccess(historyData);
                
                }
                catch (Exception ex)
                {
                    await _jsModule.InvokeVoidAsync("showAlert", ex.Message);
                    throw new Exception(ex.Message);
                }

            }
        }
        
        private async void applyFilter()
        {
            filterActive = true;

            pageActive = 1;

            filterDetails.locationId = activeUser.location.Equals("") ? "HO" : activeUser.location;
            filterDetails.filterNo = FilterProcedure;
            filterDetails.filterName = FilterProcedure;
            filterDetails.filterDept = departmentSelect;
            filterDetails.filterBU = bisnisUnitSelect;
            filterDetails.pageNo = pageActive;
            filterDetails.rowPerPage = 0;

            await ProcedureService.GetDepartmentProcedurewithFilterbyPaging(filterDetails);
            numberofPage = await ProcedureService.getDepartmentProcedurewithFilterNumberofPage(filterDetails);

            StateHasChanged();
        }

        private async void resetFilter()
        {
            filterActive = false;
            FilterProcedure = string.Empty;
            bisnisUnitSelect = string.Empty;
            departmentSelect = string.Empty;

            pageActive = 1;

            string temp = activeUser.location + "!_!" + pageActive.ToString();

            await ProcedureService.GetDepartmentProcedurewithPaging(Base64Encode(temp));

            string loc = activeUser.location.Equals("") ? "HO" : activeUser.location;
            numberofPage = await ProcedureService.getDepartmentProcedureNumberofPage(Base64Encode(loc));

            StateHasChanged();
        }

        void redirectProcedure(string procNo)
        {
            param = Base64Encode(procNo);

            navigate.NavigateTo($"procedure/editsop/{param}");
        }

        private async Task pageSelect(int currPage)
        {
            pageActive = currPage;

            if (!filterActive)
            {
                string temp = activeUser.location + "!_!" + pageActive.ToString();
                await ProcedureService.GetDepartmentProcedurewithPaging(Base64Encode(temp));
            }
            else
            {
                filterDetails.locationId = activeUser.location.Equals("") ? "HO" : activeUser.location;
                filterDetails.filterNo = FilterProcedure;
                filterDetails.filterName = FilterProcedure;
                filterDetails.filterDept = departmentSelect;
                filterDetails.filterBU = bisnisUnitSelect;
                filterDetails.pageNo = pageActive;
                filterDetails.rowPerPage = 0;

                await ProcedureService.GetDepartmentProcedurewithFilterbyPaging(filterDetails);
            }

        }


    }
}
