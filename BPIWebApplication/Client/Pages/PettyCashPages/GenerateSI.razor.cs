using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.PettyCash;
using BPIWebApplication.Shared.PagesModel.PettyCash;
using Microsoft.AspNetCore.Components;
using ClosedXML.Excel;
using Microsoft.JSInterop;
using System.Runtime.Intrinsics.X86;

namespace BPIWebApplication.Client.Pages.PettyCashPages
{
    public partial class GenerateSI : ComponentBase
    {
        private ActiveUser activeUser = new();

        List<Reimburse> reimburses = new List<Reimburse>();
        List<Reimburse> selectedReimburse = new List<Reimburse>();

        private int reimbursePageActive = 0;
        private int reimburseNumberofPage = 0;

        private bool showModal = false;
        private bool isLoading = false;
        private bool reimburseCheckAllisChecked = false;

        private IJSObjectReference _jsModule;

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
            activeUser.token = await sessionStorage.GetItemAsync<string>("token");
            activeUser.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.company = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0];
            activeUser.location = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
            activeUser.sessionId = await sessionStorage.GetItemAsync<string>("SessionId");
            activeUser.appV = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));
            activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");

            LoginService.activeUser.userPrivileges = activeUser.userPrivileges;

            reimbursePageActive = 1;

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/PettyCashPages/GenerateSI.razor.js");
        }

        private Stream GetFileStream(byte[] data)
        {
            var fileBinData = data;
            var fileStream = new MemoryStream(fileBinData);

            return fileStream;
        }

        private async void generateSI()
        {

            using (var workbook = new XLWorkbook())
            {
                int headerFontSize = 10;
                workbook.Properties.Author = LoginService.activeUser.userName;
                workbook.Properties.Title = "SI Report " + DateTime.Now.ToString("ddMMyyyy");

                var worksheet = workbook.AddWorksheet("Report");

                worksheet.Cell("A1").Value = "PT. CATUR MITRA SEJATI SENTOSA";
                worksheet.Cell("A1").Style.Font.SetBold(true);
                worksheet.Cell("A1").Style.Font.SetFontSize(headerFontSize);

                worksheet.Cell("A2").Value = "INSTRUKSI PEMOTONGAN SALES";
                worksheet.Cell("A2").Style.Font.SetBold(true);
                worksheet.Cell("A2").Style.Font.SetFontSize(headerFontSize);

                worksheet.Cell("A3").Value = "SETORAN TANGGAL : " + DateTime.Now.ToString("dd") + " " + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.ToString("yyyy");
                worksheet.Cell("A3").Style.Font.SetBold(true);
                worksheet.Cell("A3").Style.Font.SetFontSize(headerFontSize);

                worksheet.Cell("A5").Value = "NO";
                worksheet.Cell("A5").Style.Font.SetBold(true);
                worksheet.Cell("A5").Style.Font.SetFontSize(headerFontSize);
                worksheet.Cell("A5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("A5").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell("A5").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232);
                worksheet.Cell("B5").Value = "REIMBURSE ID";
                worksheet.Cell("B5").Style.Font.SetBold(true);
                worksheet.Cell("B5").Style.Font.SetFontSize(headerFontSize);
                worksheet.Cell("B5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("B5").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell("B5").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232);
                worksheet.Range("B5:C5").Row(1).Merge();
                worksheet.Cell("D5").Value = "LOKASI";
                worksheet.Cell("D5").Style.Font.SetBold(true);
                worksheet.Cell("D5").Style.Font.SetFontSize(headerFontSize);
                worksheet.Cell("D5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("D5").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell("D5").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232);
                worksheet.Cell("E5").Value = "NILAI POTONGAN SETORAN";
                worksheet.Cell("E5").Style.Font.SetBold(true);
                worksheet.Cell("E5").Style.Font.SetFontSize(headerFontSize);
                worksheet.Cell("E5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("E5").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell("E5").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232);

                int rowInsertStart = 6;
                int n = 0;

                foreach (var dt in reimburses)
                {
                    worksheet.Cell($"A{rowInsertStart}").Value = n + 1;
                    worksheet.Cell($"A{rowInsertStart}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell($"A{rowInsertStart}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"B{rowInsertStart}").Value = dt.ReimburseID;
                    worksheet.Cell($"B{rowInsertStart}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell($"B{rowInsertStart}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Range($"B{rowInsertStart}:C{rowInsertStart}").Row(1).Merge();
                    worksheet.Cell($"D{rowInsertStart}").Value = dt.LocationID;
                    worksheet.Cell($"D{rowInsertStart}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell($"D{rowInsertStart}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"E{rowInsertStart}").Value = dt.lines.Sum(x => x.ApprovedAmount).ToString("N0");
                    worksheet.Cell($"E{rowInsertStart}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell($"E{rowInsertStart}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    n++;
                    rowInsertStart++;
                }

                int rowFooterStart = rowInsertStart + selectedReimburse.Count + 3;
                int rowFooterEnd = rowFooterStart;

                worksheet.Cell($"A{rowFooterEnd}").Value = "TOTAL";
                worksheet.Cell($"A{rowFooterEnd}").Style.Font.SetBold(true);
                worksheet.Cell($"A{rowFooterEnd}").Style.Font.SetFontSize(headerFontSize);
                worksheet.Cell($"A{rowFooterEnd}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell($"A{rowFooterEnd}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell($"A{rowFooterEnd}").Style.Fill.BackgroundColor = XLColor.FromArgb(235, 241, 222);
                worksheet.Range($"A{rowFooterEnd}:D{rowFooterEnd}").Row(1).Merge();

                worksheet.Cell($"E{rowFooterEnd}").Value = reimburses.Sum(x => x.lines.Sum(f => f.ApprovedAmount)).ToString("N0");
                worksheet.Cell($"E{rowFooterEnd}").Style.Font.SetBold(true);
                worksheet.Cell($"E{rowFooterEnd}").Style.Font.SetFontSize(headerFontSize);
                worksheet.Cell($"E{rowFooterEnd}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell($"E{rowFooterEnd}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                rowFooterEnd++;

                worksheet.Cell($"A{rowFooterEnd}").Value = "DISETUJUI OLEH";
                worksheet.Cell($"A{rowFooterEnd}").Style.Font.SetBold(true);
                worksheet.Cell($"A{rowFooterEnd}").Style.Font.SetFontSize(headerFontSize);
                worksheet.Cell($"A{rowFooterEnd}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell($"A{rowFooterEnd}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell($"A{rowFooterEnd}").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232);
                worksheet.Range($"A{rowFooterEnd}:C{rowFooterEnd}").Row(1).Merge();

                worksheet.Cell($"D{rowFooterEnd}").Value = "DIAMBIL OLEH";
                worksheet.Cell($"D{rowFooterEnd}").Style.Font.SetBold(true);
                worksheet.Cell($"D{rowFooterEnd}").Style.Font.SetFontSize(headerFontSize);
                worksheet.Cell($"D{rowFooterEnd}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell($"D{rowFooterEnd}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell($"D{rowFooterEnd}").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232);

                worksheet.Cell($"E{rowFooterEnd}").Value = "DIKETAHUI OLEH";
                worksheet.Cell($"E{rowFooterEnd}").Style.Font.SetBold(true);
                worksheet.Cell($"E{rowFooterEnd}").Style.Font.SetFontSize(headerFontSize);
                worksheet.Cell($"E{rowFooterEnd}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell($"E{rowFooterEnd}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell($"E{rowFooterEnd}").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232);

                rowFooterEnd = rowFooterEnd + 4;

                worksheet.Cell($"A{rowFooterEnd}").Value = "FINANCE MGR";
                worksheet.Cell($"A{rowFooterEnd}").Style.Font.SetBold(true);
                worksheet.Cell($"A{rowFooterEnd}").Style.Font.SetFontSize(headerFontSize);
                worksheet.Cell($"A{rowFooterEnd}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell($"A{rowFooterEnd}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell($"A{rowFooterEnd}").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232);
                worksheet.Range($"A{rowFooterEnd}:B{rowFooterStart}").Row(1).Merge();

                worksheet.Cell($"C{rowFooterEnd}").Value = "GM FINANCE";
                worksheet.Cell($"C{rowFooterEnd}").Style.Font.SetBold(true);
                worksheet.Cell($"C{rowFooterEnd}").Style.Font.SetFontSize(headerFontSize);
                worksheet.Cell($"C{rowFooterEnd}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell($"C{rowFooterEnd}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell($"C{rowFooterEnd}").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232);

                worksheet.Cell($"D{rowFooterEnd}").Value = "KADIV / WAKADIV KASIR";
                worksheet.Cell($"D{rowFooterEnd}").Style.Font.SetBold(true);
                worksheet.Cell($"D{rowFooterEnd}").Style.Font.SetFontSize(headerFontSize);
                worksheet.Cell($"D{rowFooterEnd}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell($"D{rowFooterEnd}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell($"D{rowFooterEnd}").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232);
                worksheet.Range($"D{rowFooterEnd}:E{rowFooterEnd}").Row(1).Merge();

                // styling

                worksheet.Range($"A5:E{(rowInsertStart - 1)}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A5:E{(rowInsertStart - 1)}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A5:E{(rowInsertStart - 1)}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A5:E{(rowInsertStart - 1)}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A5:E{(rowInsertStart - 1)}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A5:E{(rowInsertStart - 1)}").Style.Border.TopBorder = XLBorderStyleValues.Thin;

                worksheet.Range($"A{rowFooterStart}:E{(rowFooterStart + 1)}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{rowFooterStart}:E{(rowFooterStart + 1)}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{rowFooterStart}:E{(rowFooterStart + 1)}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{rowFooterStart}:E{(rowFooterStart + 1)}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{rowFooterStart}:E{(rowFooterStart + 1)}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{rowFooterStart}:E{(rowFooterStart + 1)}").Style.Border.TopBorder = XLBorderStyleValues.Thin;

                worksheet.Range($"A{rowFooterEnd}:E{rowFooterEnd}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{rowFooterEnd}:E{rowFooterEnd}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{rowFooterEnd}:E{rowFooterEnd}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{rowFooterEnd}:E{rowFooterEnd}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{rowFooterEnd}:E{rowFooterEnd}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range($"A{rowFooterEnd}:E{rowFooterEnd}").Style.Border.TopBorder = XLBorderStyleValues.Thin;

                worksheet.Cell($"A{(rowFooterStart + 2)}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Cell($"A{(rowFooterStart + 3)}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Cell($"A{(rowFooterStart + 4)}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                worksheet.Cell($"D{(rowFooterStart + 2)}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Cell($"D{(rowFooterStart + 3)}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Cell($"D{(rowFooterStart + 4)}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                worksheet.Cell($"E{(rowFooterStart + 2)}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Cell($"E{(rowFooterStart + 3)}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Cell($"E{(rowFooterStart + 4)}").Style.Border.RightBorder = XLBorderStyleValues.Thin;

                MemoryStream ms = new MemoryStream();
                workbook.SaveAs(ms);

                using var streamRef = new DotNetStreamReference(stream: GetFileStream(ms.ToArray()));

                await _jsModule.InvokeVoidAsync("downloadFileFromStream", $"GenerateSI {DateTime.Now.ToString("ddMMyyyy")}.xlsx", streamRef);
                await _jsModule.InvokeVoidAsync("showAlert", $"File GenerateSI {DateTime.Now.ToString("ddMMyyyy")}.xlsx Downloaded");
            }
        }

        private void appendReimburseSelected(Reimburse data)
        {
            if (selectedReimburse.FirstOrDefault(a => a.ReimburseID == data.ReimburseID) == null)
            {
                selectedReimburse.Add(data);
            }
            else
            {
                var itemRemove1 = selectedReimburse.SingleOrDefault(a => a.ReimburseID == data.ReimburseID);

                if (itemRemove1 != null)
                {
                    selectedReimburse.Remove(itemRemove1);
                }

            }
        }

        private async Task applyReimburse()
        {
            isLoading = true;

            try
            {
                
                if (selectedReimburse.Any())
                {
                    foreach (var dt in selectedReimburse)
                    {
                        reimburses.Add(dt);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            showModal = false;
            isLoading = false;
            selectedReimburse.Clear();

            StateHasChanged();
        }

        private async Task reimbursePageSelect(int currPage)
        {
            reimbursePageActive = currPage;

            string temp = "MASTER!_!" + activeUser.location + "!_!Verified!_!!_!!_!" + reimbursePageActive.ToString();

            await PettyCashService.getReimburseDatabyLocation(Base64Encode(temp));

        }

        private void checkAll()
        {

            if (!selectedReimburse.Any())
            {
                foreach (var line in reimburses)
                {
                    selectedReimburse.Add(line);
                }

                reimburseCheckAllisChecked = true;
            }
            else
            {
                foreach (var line in reimburses)
                {
                    selectedReimburse.Remove(line);
                }

                reimburseCheckAllisChecked = false;
            }

        }

        private void reimburseAction(string action)
        {
            if (selectedReimburse.Any())
            {
                if (action.Equals("remove"))
                {
                    foreach (var line in selectedReimburse)
                    {
                        reimburses.Remove(line);
                    }
                }
            }

            selectedReimburse.Clear();
            reimburseCheckAllisChecked = false;
        }

        private bool checkReimburseDataPresent()
        {
            try
            {
                if (PettyCashService.reimburses.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void modalHide()
        {
            showModal = false;
            selectedReimburse.Clear();
        }

        private async Task modalShow()
        {
            showModal = true;

            PettyCashService.reimburses = new();

            string rbspz = "Reimburse!_!ReimburseID!_!Verified!_!!_!!_!" + activeUser.location;
            reimburseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(rbspz));

            string rbslocPage = "MASTER!_!" + activeUser.location + "!_!Verified!_!!_!!_!" + reimbursePageActive.ToString();
            await PettyCashService.getReimburseDatabyLocation(Base64Encode(rbslocPage));
        }


        //
    }
}
