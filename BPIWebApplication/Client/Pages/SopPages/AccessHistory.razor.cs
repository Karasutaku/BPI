using BPIWebApplication.Shared.PagesModel.AccessHistory;
using BPIWebApplication.Shared.ReportModel;
using Microsoft.AspNetCore.Components;
using ClosedXML.Excel;
using Microsoft.JSInterop;
using System.IO;

namespace BPIWebApplication.Client.Pages.SopPages
{
    public partial class AccessHistory : ComponentBase
    {
        private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();

        private int pageActive, numberofPage;
        private bool filterActive = false;

        private bool showModal = false;

        AccessHistoryFilter filterData = new AccessHistoryFilter();
        AccessHistoryReport reportSubmit = new AccessHistoryReport();

        private IJSObjectReference _jsModule;

        private string historyFilterSelect { get; set; } = string.Empty;
        private string filterDetails { get; set; } = string.Empty;

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private string historyFilterSelected()
        {
            return historyFilterSelect;
        }

        private void modalShow() => showModal = true;
        private void modalHide() => showModal = false;


        protected override async Task OnInitializedAsync()
        {
            activeUser.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.UserLogin = new LoginUser();
            activeUser.UserLogin.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userEmail"));
            activeUser.role = Base64Decode(await sessionStorage.GetItemAsync<string>("role"));

            filterActive = false;
            filterData = new AccessHistoryFilter();

            await ProcedureService.GetAllProcedure();
            pageActive = 1;
            await ProcedureService.GetHistoryAccessbyPaging(pageActive);
            numberofPage = await ProcedureService.getHistoryAccessNumberofPage();

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/SopPages/AccessHistory.razor.js");
        }

        async void resetFilter()
        {
            filterActive = false;
            historyFilterSelect = string.Empty;
            filterDetails = string.Empty;

            pageActive = 1;

            await ProcedureService.GetHistoryAccessbyPaging(pageActive);
            numberofPage = await ProcedureService.getHistoryAccessNumberofPage();

            StateHasChanged();
        }

        async void applyFilter()
        {
            filterActive = true;

            pageActive = 1;

            filterData.filterType = historyFilterSelect;
            filterData.filterDetails = filterDetails;
            filterData.pageNo = pageActive;
            filterData.rowPerPage = 0;

            await ProcedureService.GetHistoryAccessbyFilterwithPaging(filterData);
            numberofPage = await ProcedureService.getAccessHistorywithFilterNumberofPage(filterData);

            StateHasChanged();
        }

        private async Task pageSelect(int currPage)
        {
            pageActive = currPage;

            if (!filterActive)
            {
                await ProcedureService.GetHistoryAccessbyPaging(pageActive);
            }
            else
            {
                filterData.pageNo = pageActive;
                await ProcedureService.GetHistoryAccessbyFilterwithPaging(filterData);
            }
            
        }

        private Stream GetFileStream(byte[] data)
        {
            var fileBinData = data;
            var fileStream = new MemoryStream(fileBinData);

            return fileStream;
        }

        private async void exportAccessHistoryReport()
        {
            // last time instance of end date selected
            DateTime temp = new(reportSubmit.endDate.Year, reportSubmit.endDate.Month, reportSubmit.endDate.Day, 23, 59, 59);

            reportSubmit.endDate = temp;

            await ProcedureService.GetHistoryAccessReportbyFilter(reportSubmit);
            var resReport = ProcedureService.historyAccessReport;

            using (var workbook = new XLWorkbook())
            {
                workbook.Properties.Author = activeUser.UserLogin.userName;
                workbook.Properties.Title = "Access History Report";
                
                var worksheet = workbook.AddWorksheet("Report");

                worksheet.Cell("A1").Value = "Access History Report";
                worksheet.Cell("A1").Style.Font.SetBold(true);
                worksheet.Cell("A1").Style.Font.SetFontSize(15);

                worksheet.Cell("A3").Value = $"Start Date : {reportSubmit.startDate}";
                worksheet.Cell("A4").Value = $"End Date : {reportSubmit.endDate}";

                worksheet.Cell("A6").Value = "Procedure No";
                worksheet.Cell("A6").Style.Font.SetBold(true);
                worksheet.Cell("B6").Value = "Procedure Name";
                worksheet.Cell("B6").Style.Font.SetBold(true);
                worksheet.Cell("C6").Value = "User";
                worksheet.Cell("C6").Style.Font.SetBold(true);
                worksheet.Cell("D6").Value = "Access Date";
                worksheet.Cell("D6").Style.Font.SetBold(true);

                // insert 4 column data
                worksheet.Cell("A7").InsertData(resReport);

                MemoryStream ms = new MemoryStream();
                workbook.SaveAs(ms);

                using var streamRef = new DotNetStreamReference(stream: GetFileStream(ms.ToArray()));

                await _jsModule.InvokeVoidAsync("downloadFileFromStream", "AccessHistoryReport.xlsx", streamRef);
                await _jsModule.InvokeVoidAsync("showAlert", "File AccessHistoryReport.xlsx Downloaded");
            }
            
        }


    }
}
