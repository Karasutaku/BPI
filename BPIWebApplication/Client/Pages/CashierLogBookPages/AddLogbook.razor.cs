using BPIWebApplication.Client.Services.PettyCashServices;
using BPIWebApplication.Shared.MainModel.CashierLogbook;
using BPIWebApplication.Shared.MainModel.Company;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.CashierLogbook;
using BPIWebApplication.Shared.MainModel.Company;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BPIWebApplication.Shared.MainModel.PettyCash;
using BPIWebApplication.Shared.DbModel;

namespace BPIWebApplication.Client.Pages.CashierLogBookPages
{
    public partial class AddLogbook : ComponentBase
    {
        [Parameter]
        public string? param { get; set; } = null;

        private ActiveUser activeUser = new();

        //private Shift activeShift { get; set; } = new();
        //private AmountCategories activeCategories { get; set; } = new();

        private CashierLogData logbook { get; set; } = new();
        private LocationBalanceDetails? balanceData { get; set; } = null;

        private int selectedShiftID { get; set; } = 0;
        private string selectedCategoryID { get; set; } = string.Empty;
        private string previewActualAmount { get; set; } = string.Empty;
        //private string testtext { get; set; } = string.Empty;

        private bool alertTrigger = false;
        private bool successAlert = false;
        private string alertBody = string.Empty;
        private string alertMessage = string.Empty;

        private bool isFetchBalanceActive = false;
        private bool isLoading = false;
        private bool isSuccessUpload = false;

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
            //activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");

            //LoginService.activeUser.userPrivileges = activeUser.userPrivileges;

            await CashierLogbookService.getShiftData("CashierLogbook");
            await CashierLogbookService.getLogbookCategories();

            logbook.LogType = "MAIN";
            logbook.Applicant = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            logbook.LocationID = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1].Equals("") ? "HO" : Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
            logbook.LogStatus = "Create";
            logbook.LogStatusDate = DateTime.Now;

            selectedCategoryID = CashierLogbookService.categories.OrderBy(x => x.AmountCategoryName).First().AmountCategoryID;
            selectedShiftID = CashierLogbookService.Shifts.OrderBy(x => x.ShiftID).First().ShiftID;

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/CashierLogbookPages/AddLogbook.razor.js");
        }

        protected override async Task OnParametersSetAsync()
        {
            if (param != null)
            {
                isSuccessUpload = false;
                string temp = Base64Decode(param);

                if (temp.Split("!_!")[1].Equals("MAIN"))
                {
                    if (CashierLogbookService.mainLogs.SingleOrDefault(a => a.LogID.Equals(temp.Split("!_!")[0])) != null)
                    {
                        logbook = CashierLogbookService.mainLogs.SingleOrDefault(a => a.LogID.Equals(temp.Split("!_!")[0]));
                        logbook.LogType = "MAIN";
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Data Not Found !");
                    }
                }
                else if (temp.Split("!_!")[1].Equals("TRANSIT"))
                {
                    if (CashierLogbookService.transitLogs.SingleOrDefault(a => a.LogID.Equals(temp.Split("!_!")[0])) != null)
                    {
                        logbook = CashierLogbookService.transitLogs.SingleOrDefault(a => a.LogID.Equals(temp.Split("!_!")[0]));
                        logbook.LogType = "TRANSIT";
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Data Not Found !");
                    }
                }

                StateHasChanged();
            }
        }

        private async void createLogData()
        {
            try
            {
                if (!validateInput())
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Blank Input Field, Please Fill the Blank Field !");
                    isLoading = false;
                    isSuccessUpload = false;
                }
                else
                {
                    isLoading = true;

                    //logbook.header.SelectMany(x => x.lines).Select(y => y.ShiftID).Distinct();

                    QueryModel<CashierLogData> uploadData = new();
                    uploadData.Data = new();

                    uploadData.Data = logbook;

                    uploadData.Data.approvals = new() { new CashierLogApproval
                    {
                        LocationID = activeUser.location,
                        LogID = "",
                        ShiftID = selectedShiftID,
                        CreateUser = activeUser.userName,
                        CreateDate = DateTime.Now,
                        ConfirmUser = "",
                        ConfirmDate = DateTime.MinValue,
                        ApproveNote = ""
                    }};

                    uploadData.userEmail = activeUser.userName;
                    uploadData.userAction = "I";
                    uploadData.userActionDate = DateTime.Now;

                    var res = await CashierLogbookService.createLogData(uploadData);

                    if (res.isSuccess)
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Create Log Success !");

                        logbook.LogID = res.Data.Data.LogID;

                        isLoading = false;
                        isSuccessUpload = true;

                        StateHasChanged();
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Create Log Failed, Please Check Your Connection !");
                        isLoading = false;
                        isSuccessUpload = false;
                    }
                }
            }
            catch (Exception e)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error {e.Message} !");
                isFetchBalanceActive = false;
            }
        }

        private async void editLogData()
        {
            try
            {
                if (!validateInput())
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Blank Input Field, Please Fill the Blank Field !");
                    isLoading = false;
                    isSuccessUpload = false;
                }
                else
                {
                    isLoading = true;

                    QueryModel<CashierLogData> uploadData = new();
                    uploadData.Data = new();

                    uploadData.Data = logbook;
                    uploadData.userEmail = activeUser.userName;
                    uploadData.userAction = "U";
                    uploadData.userActionDate = DateTime.Now;

                    var res = await CashierLogbookService.editLogData(uploadData);

                    if (res.isSuccess)
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Edit Log Success !");

                        //logbook.LogID = res.Data.Data.LogID;

                        isLoading = false;
                        isSuccessUpload = true;

                        StateHasChanged();
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Edit Log Failed, Please Check Your Connection !");
                        isLoading = false;
                        isSuccessUpload = false;
                    }
                }
            }
            catch (Exception e)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error {e.Message} !");
                isFetchBalanceActive = false;
            }
        }

        private async void checkSubCategoryAutoFill(ChangeEventArgs e, CashierLogLineDetail line)
        {
            line.AmountSubCategoryID = e.Value.ToString();

            try
            {
                if (e.Value.Equals("AD"))
                {
                    if (balanceData == null)
                    {
                        isFetchBalanceActive = true;

                        string temp = activeUser.location.Equals("") ? "HO" : activeUser.location;

                        var res = await PettyCashService.getPettyCashOutstandingAmount(temp);

                        if (res.isSuccess)
                        {
                            balanceData = res.Data;

                            line.LineAmount = res.Data.outstandingBalance.advanceApprovedAmount;

                            isFetchBalanceActive = false;
                        }
                        else
                        {
                            line.LineAmount = decimal.Zero;

                            await _jsModule.InvokeVoidAsync("showAlert", "Fetch Data Failed, Please Check Your Connection !");
                            isFetchBalanceActive = false;
                        }
                    }
                    else
                    {
                        line.LineAmount = balanceData.outstandingBalance.advanceApprovedAmount;
                    }
                }
                else if (e.Value.Equals("EX"))
                {
                    if (balanceData == null)
                    {
                        isFetchBalanceActive = true;

                        string temp = activeUser.location.Equals("") ? "HO" : activeUser.location;

                        var res = await PettyCashService.getPettyCashOutstandingAmount(temp);

                        if (res.isSuccess)
                        {
                            balanceData = res.Data;

                            line.LineAmount = res.Data.outstandingBalance.expenseApprovedAmount;

                            isFetchBalanceActive = false;
                        }
                        else
                        {
                            line.LineAmount = decimal.Zero;

                            await _jsModule.InvokeVoidAsync("showAlert", "Fetch Data Failed, Please Check Your Connection !");
                            isFetchBalanceActive = false;
                        }
                    }
                    else
                    {
                        line.LineAmount = balanceData.outstandingBalance.expenseApprovedAmount;
                    }
                }
                else if (e.Value.Equals("RB"))
                {
                    if (balanceData == null)
                    {
                        isFetchBalanceActive = true;

                        string temp = activeUser.location.Equals("") ? "HO" : activeUser.location;

                        var res = await PettyCashService.getPettyCashOutstandingAmount(temp);

                        if (res.isSuccess)
                        {
                            line.LineAmount = res.Data.outstandingBalance.reimbursementApvOutstandingAmount;

                            isFetchBalanceActive = false;
                        }
                        else
                        {
                            line.LineAmount = decimal.Zero;

                            await _jsModule.InvokeVoidAsync("showAlert", "Fetch Data Failed, Please Check Your Connection !");
                            isFetchBalanceActive = false;
                        }
                    }
                    else
                    {
                        line.LineAmount = balanceData.outstandingBalance.reimbursementApvOutstandingAmount;
                    }
                }

                StateHasChanged();
            }
            catch (Exception exc)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error ! {exc.Message}");
                throw new Exception(exc.Message);
            }
        }

        //private void previewUpdate(string catId)
        //{
        //    previewActualAmount = logbook.header.FirstOrDefault(x => x.AmountCategoryID.Equals(catId)).ActualAmount.ToString("N0");
        //    StateHasChanged();  
        //}

        private void addLine(int shiftId, string categoryId)
        {
            if (!logbook.header.Where(x => x.AmountCategoryID.Equals(categoryId)).Where(y => y.lines.All(z => z.ShiftID.Equals(shiftId))).SelectMany(ln => ln.lines).Any())
            {
                logbook.header.Add(new CashierLogCategoryDetail
                {
                    LogID = "",
                    BrankasCategoryID = "",
                    AmountCategoryID = CashierLogbookService.categories.FirstOrDefault(x => x.AmountCategoryID.Equals(categoryId)).AmountCategoryID,
                    AmountCategoryName = CashierLogbookService.categories.FirstOrDefault(x => x.AmountCategoryID.Equals(categoryId)).AmountCategoryName,
                    HeaderAmount = decimal.Zero,
                    ActualAmount = decimal.Zero,
                    isLineDeleted = false,
                    lines = new()
                });

                logbook.header.Where(x => x.AmountCategoryID.Equals(categoryId)).Where(y => y.lines.All(z => z.ShiftID.Equals(shiftId))).FirstOrDefault(x => x.AmountCategoryID.Equals(categoryId)).lines.Add(new CashierLogLineDetail
                {
                    BrankasCategoryID = "",
                    LineNo = 0,
                    AmountSubCategoryID = "BLANK",
                    AmountSubCategoryName = "",
                    AmountType = "BLANK",
                    AmountDesc = "",
                    ShiftID = CashierLogbookService.Shifts.FirstOrDefault(x => x.ShiftID.Equals(shiftId)).ShiftID,
                    ShiftDesc = CashierLogbookService.Shifts.FirstOrDefault(x => x.ShiftID.Equals(shiftId)).ShiftDesc,
                    LineAmount = decimal.Zero
                });
            }
            else
            {
                logbook.header.Where(x => x.AmountCategoryID.Equals(categoryId)).Where(y => y.lines.All(z => z.ShiftID.Equals(shiftId))).FirstOrDefault(x => x.AmountCategoryID.Equals(categoryId)).lines.Add(new CashierLogLineDetail
                {
                    BrankasCategoryID = "",
                    LineNo = 0,
                    AmountSubCategoryID = "BLANK",
                    AmountSubCategoryName = "",
                    AmountType = "BLANK",
                    AmountDesc = "",
                    ShiftID = CashierLogbookService.Shifts.FirstOrDefault(x => x.ShiftID.Equals(shiftId)).ShiftID,
                    ShiftDesc = CashierLogbookService.Shifts.FirstOrDefault(x => x.ShiftID.Equals(shiftId)).ShiftDesc,
                    LineAmount = decimal.Zero
                });
            }
        }

        private void deleteLine(CashierLogLineDetail data, string categoryId)
        {
            logbook.header.Where(x => x.AmountCategoryID.Equals(categoryId)).Where(y => y.lines.All(z => z.ShiftID.Equals(selectedShiftID))).FirstOrDefault().lines.Remove(data);

            if (!logbook.header.Where(x => x.AmountCategoryID.Equals(categoryId)).Where(y => y.lines.All(z => z.ShiftID.Equals(selectedShiftID))).FirstOrDefault().lines.Any())
            {
                logbook.header.Remove(logbook.header.Where(x => x.AmountCategoryID.Equals(categoryId)).Where(y => y.lines.All(z => z.ShiftID.Equals(selectedShiftID))).FirstOrDefault());
            }
        }

        private bool validateInput()
        {
            if (!logbook.header.Any())
                return false;

            if (logbook.header.Any(x => x.ActualAmount < 1))
                return false;

            if (logbook.header.Any(x => x.lines.Any(y => y.AmountSubCategoryID.Equals("BLANK"))))
                return false;

            if (logbook.header.Any(x => x.lines.Any(y => y.AmountType.Equals("BLANK"))))
                return false;

            if (logbook.header.Any(x => x.lines.Any(y => y.LineAmount < 1)))
                return false;

            return true;
        }

        private bool checkCategoriesPresent()
        {
            try
            {
                if (CashierLogbookService.categories.Any())
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

        private bool checkSubCategoriesPresent()
        {
            try
            {
                if (CashierLogbookService.subCategories.Any())
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

        private bool checkTypePresent()
        {
            try
            {
                if (CashierLogbookService.types.Any())
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
