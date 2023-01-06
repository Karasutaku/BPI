using BPIWebApplication.Shared;
using Microsoft.AspNetCore.Components;
using System.Runtime.InteropServices;
using Microsoft.JSInterop;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.PagesModel.Dashboard;

namespace BPIWebApplication.Client.Pages.SopPages
{
    public partial class Dashboard : ComponentBase
    {
        private string FilterProcedure { get; set; } = string.Empty;
        private string bisnisUnitSelect { get; set; } = string.Empty;
        private string departmentSelect { get; set; } = string.Empty;

        private bool filterActive = false;
        DashboardFilter filterDetails = new DashboardFilter();

        public string bisnisUnitSelected () {
            return bisnisUnitSelect;
        }
        public string departmentSelected () {
            return departmentSelect;
        }

        private async void resetFilter ()
        {
            filterActive = false;
            FilterProcedure = string.Empty;
            bisnisUnitSelect = string.Empty;
            departmentSelect = string.Empty;

            pageActive = 1;

            await ProcedureService.GetDepartmentProcedurewithPaging(pageActive);
            numberofPage = await ProcedureService.getDepartmentProcedureNumberofPage();

            StateHasChanged();
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

        private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();

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
            await ManagementService.GetAllBisnisUnit();
            await ManagementService.GetAllDepartment();
            pageActive = 1;
            await ProcedureService.GetDepartmentProcedurewithPaging(pageActive);
            numberofPage = await ProcedureService.getDepartmentProcedureNumberofPage();

            filterActive = false;
            filterDetails = new DashboardFilter();

            activeUser.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.UserLogin = new LoginUser();
            activeUser.UserLogin.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userEmail"));
            activeUser.role = Base64Decode(await sessionStorage.GetItemAsync<string>("role"));

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/SopPages/Dashboard.razor.js");
        }

        private string? param;

        private Stream GetFileStream(byte[] data)
        {
            var fileBinData = data;
            var fileStream = new MemoryStream(fileBinData);

            return fileStream;
        }

        private async void handleDownload(string path, string procNo, string procName)
        {
            var dt = await ProcedureService.GetFile(path);
            
            if (!dt.isSuccess)
            {
                // alert file download not found
            }
            else
            {
                // download file

                var filestream = GetFileStream(dt.Data.content);
                string filename = procNo + ".pdf";

                using var streamRef = new DotNetStreamReference(stream: filestream);

                await _jsModule.InvokeVoidAsync("downloadFileFromStream", filename, streamRef);
                await _jsModule.InvokeVoidAsync("showAlert", $"File {procNo} Downloaded");

                // insert history data
                try
                {
                    QueryModel<HistoryAccess> historyData = new QueryModel<HistoryAccess>();
                    historyData.Data = new HistoryAccess();

                    historyData.Data.ProcedureNo = procNo;
                    historyData.Data.ProcedureName = procName;
                    historyData.Data.UserEmail = activeUser.UserLogin.userName;
                    historyData.Data.HistoryAccessDate = DateTime.Now;
                    historyData.userEmail = activeUser.UserLogin.userName;
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
                await ProcedureService.GetDepartmentProcedurewithPaging(pageActive);
            }
            else
            {
                filterDetails.pageNo = pageActive;
                await ProcedureService.GetDepartmentProcedurewithFilterbyPaging(filterDetails);
            }

        }


    }
}
