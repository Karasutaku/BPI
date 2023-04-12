using BPIWebApplication.Client.Services.PettyCashServices;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.CashierLogbook;
using BPIWebApplication.Shared.MainModel.Company;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.CashierLogbook;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.CashierLogBookPages
{
    public partial class CashierLogbookDashboard : ComponentBase
    {
        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        CashierLogData activeLog = new();
        List<AmountCategories> activeCategories = new();
        List<AmountSubCategories> activeSubCategories = new();
        List<AmountTypes> activeAmountType = new();
        List<Shift> activeShift = new();

        private int mainLogPageActive = 0;
        private int transitLogPageActive = 0;
        private int actionLogPageActive = 0;
        private int mainLogPageSize = 0;
        private int transitLogPageSize = 0;
        private int actionLogPageSize = 0;
        private int confirmShift = 0;

        private bool isMainLogActive = false;
        private bool isTransitLogActive = false;
        private bool isMainLogFilterActive = false;
        private bool isTransitLogFilterActive = false;
        private bool isActionLogFilterActive = false;
        private bool showModal = false;
        private bool confirmModal = false;

        private string mainLogFilterType { get; set; } = string.Empty;
        private string transitLogFilterType { get; set; } = string.Empty;
        private string mainLogFilterValue { get; set; } = string.Empty;
        private string transitLogFilterValue { get; set; } = string.Empty;
        private DateTime mainLogFilterDateValue { get; set; } = DateTime.Now;
        private DateTime transitLogFilterDateValue { get; set; } = DateTime.Now;

        private string confirmNote { get; set; } = string.Empty;

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

        IJSObjectReference _jsModule;

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
            //activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");

            //LoginService.activeUser.userPrivileges = activeUser.userPrivileges;

            string type = "UTAMA";
            string status = "";
            string filType = "LogID";
            string filValue = "";
            mainLogPageActive = 1;

            string mainpz = "BrankasLog!_!" + activeUser.location + "!_!LogType = \'UTAMA\'";
            mainLogPageSize = await CashierLogbookService.getModulePageSize(Base64Encode(mainpz));
            string temp = type + "!_!" + activeUser.location + "!_!" + status + $"!_!{filType} LIKE \'%%\'!_!" + mainLogPageActive.ToString();
            await CashierLogbookService.getLogData(Base64Encode(temp));

            type = "TRANSIT";
            transitLogPageActive = 1;

            string transpz = "BrankasLog!_!" + activeUser.location + "!_!LogType = \'TRANSIT\'";
            transitLogPageSize = await CashierLogbookService.getModulePageSize(Base64Encode(transpz));
            temp = type + "!_!" + activeUser.location + "!_!" + status + $"!_!{filType} LIKE \'%%\'!_!" + mainLogPageActive.ToString();
            await CashierLogbookService.getLogData(Base64Encode(temp));

            string orderby = "a.AuditActionDate";
            filType = "a.LogID";
            filValue = "";
            actionLogPageActive = 1;

            string actpz = "BrankasApproveLog!_!" + activeUser.location + "!_!LogID LIKE \'%%\'";
            actionLogPageSize = await CashierLogbookService.getModulePageSize(Base64Encode(actpz));
            temp = activeUser.location + "!_!" + orderby + "!_!" + filType + "!_!" + filValue + "!_!" + actionLogPageActive.ToString();
            await CashierLogbookService.getBrankasActionLogData(Base64Encode(temp));

            await CashierLogbookService.getShiftData("CashierLogbook");

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/CashierLogBookPages/CashierLogbookDashboard.razor.js");
        }

        private void hideModal() => showModal = false;

        private void showMainModal(CashierLogData data, string tp)
        {
            isMainLogActive = false;
            isTransitLogActive = false;

            if (tp.Equals("UTAMA"))
            {
                isMainLogActive = true;
            }
            else if (tp.Equals("TRANSIT"))
            {
                isTransitLogActive = true;
            }

            showModal = true;
            activeCategories.Clear();
            activeSubCategories.Clear();
            activeAmountType.Clear();
            activeShift.Clear();

            activeLog = data;

            foreach (var header in activeLog.header)
            {
                if (activeCategories.FirstOrDefault(x => x.AmountCategoryID.Equals(header.AmountCategoryID)) == null)
                {
                    activeCategories.Add(new AmountCategories
                    {
                        AmountCategoryID = header.AmountCategoryID,
                        AmountCategoryName = header.AmountCategoryName
                    });
                }

                foreach (var line in header.lines)
                {
                    if (activeSubCategories.FirstOrDefault(x => x.AmountSubCategoryID.Equals(line.AmountSubCategoryID)) == null)
                    {
                        activeSubCategories.Add(new AmountSubCategories
                        {
                            AmountSubCategoryID = line.AmountSubCategoryID,
                            AmountSubCategoryName = line.AmountSubCategoryName,
                            AmountType = line.AmountType
                        });
                    }

                    if (activeAmountType.FirstOrDefault(x => x.AmountType.Equals(line.AmountType)) == null)
                    {
                        activeAmountType.Add(new AmountTypes
                        {
                            AmountType = line.AmountType,
                            AmountDesc = line.AmountDesc
                        });
                    }

                    if (activeShift.FirstOrDefault(x => x.ShiftID.Equals(line.ShiftID)) == null)
                    {
                        activeShift.Add(new Shift
                        {
                            ShiftID = line.ShiftID,
                            ShiftDesc = line.ShiftDesc,
                            isActive = false
                        });
                    }
                }
            }

            activeShift.First().isActive = true;
            //
        }

        private void editDocument(CashierLogData data)
        {
            string temp = string.Empty;

            if (isMainLogActive)
            {
                temp = data.LogID + "!_!UTAMA";
            }
            else if (isTransitLogActive)
            {
                temp = data.LogID + "!_!TRANSIT";
            }

            string param = Base64Encode(temp);

            navigate.NavigateTo($"cashierlogbook/editlogbook/{param}");
        }

        private async void confirmLog()
        {
            try
            {
                QueryModel<CashierLogApproval> editData = new();
                editData.Data = new();

                editData.Data.LogID = activeLog.LogID;
                editData.Data.LocationID = activeLog.LocationID;
                editData.Data.ShiftID = activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID;
                editData.Data.CreateUser = activeLog.approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).CreateUser;
                editData.Data.CreateDate = activeLog.approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).CreateDate;
                editData.Data.ConfirmUser = activeUser.userName;
                editData.Data.ConfirmDate = DateTime.Now;
                editData.Data.ApproveNote = confirmNote;
                editData.userEmail = activeUser.userName;
                editData.userAction = "U";
                editData.userActionDate = DateTime.Now;

                var res = await CashierLogbookService.editBrankasApproveLogOnConfirm(editData);

                if (res.isSuccess)
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Log Confirm Handover Success !");

                    activeLog.LogStatus = "Partial";
                    activeLog.approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).ApproveNote = confirmNote;
                    activeLog.approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).ConfirmUser = activeUser.userName;
                    activeLog.approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).ConfirmDate = DateTime.Now;

                    if (activeLog.LogType.Equals("UTAMA"))
                    {
                        CashierLogbookService.mainLogs.SingleOrDefault(z => z.LogID.Equals(activeLog.LogID)).LogStatus = "Partial";
                        CashierLogbookService.mainLogs.SingleOrDefault(z => z.LogID.Equals(activeLog.LogID)).approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).ApproveNote = confirmNote;
                        CashierLogbookService.mainLogs.SingleOrDefault(z => z.LogID.Equals(activeLog.LogID)).approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).ConfirmUser = activeUser.userName;
                        CashierLogbookService.mainLogs.SingleOrDefault(z => z.LogID.Equals(activeLog.LogID)).approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).ConfirmDate = DateTime.Now;
                    }
                    else if (activeLog.LogType.Equals("TRANSIT"))
                    {
                        CashierLogbookService.transitLogs.SingleOrDefault(z => z.LogID.Equals(activeLog.LogID)).LogStatus = "Partial";
                        CashierLogbookService.transitLogs.SingleOrDefault(z => z.LogID.Equals(activeLog.LogID)).approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).ApproveNote = confirmNote;
                        CashierLogbookService.transitLogs.SingleOrDefault(z => z.LogID.Equals(activeLog.LogID)).approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).ConfirmUser = activeUser.userName;
                        CashierLogbookService.transitLogs.SingleOrDefault(z => z.LogID.Equals(activeLog.LogID)).approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).ConfirmDate = DateTime.Now;
                    }
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Log Confirm Handover Failed, Please Check Your Connection and Try Again !");
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Log Confirm Handover Failed, {ex.Message} !");
            }
        }

        private async void updateBrankasStatus(string stat, string type)
        {
            try
            {
                QueryModel<string> editData = new();

                string temp = activeLog.LogID + "!_!" + activeUser.location + "!_!" + stat;

                editData.Data = Base64Encode(temp);
                editData.userEmail = activeUser.userName;
                editData.userAction = "U";
                editData.userActionDate = DateTime.Now;

                var res = await CashierLogbookService.updateBrankasDocumentStatusData(editData);

                if (res.isSuccess)
                {
                    activeLog.LogStatus = stat;
                    activeLog.LogStatusDate = DateTime.Now;

                    if (type.Equals("UTAMA"))
                    {
                        CashierLogbookService.mainLogs.SingleOrDefault(z => z.LogID.Equals(activeLog.LogID)).LogStatus = stat;
                        CashierLogbookService.mainLogs.SingleOrDefault(z => z.LogID.Equals(activeLog.LogID)).LogStatusDate = DateTime.Now;
                    }
                    else if (type.Equals("TRANSIT"))
                    {
                        CashierLogbookService.transitLogs.SingleOrDefault(z => z.LogID.Equals(activeLog.LogID)).LogStatus = stat;
                        CashierLogbookService.transitLogs.SingleOrDefault(z => z.LogID.Equals(activeLog.LogID)).LogStatusDate = DateTime.Now;
                    }

                    await _jsModule.InvokeVoidAsync("showAlert", "Log Data Status Success Updated !");
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Log Data Status Fail Updated, Please Check Your Connection and Try Again !");
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Log Data Status Fail Updated, {ex.Message} !");
            }
        }

        private async Task mainLogPageSelect(int currPage)
        {
            mainLogPageActive = currPage;

            if (isMainLogFilterActive)
            {
                string type = "UTAMA";
                string status = "";
                string cond = $"{mainLogFilterType} LIKE \'%{mainLogFilterValue}%\'";

                string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + cond + "!_!" + mainLogPageActive.ToString();
                await CashierLogbookService.getLogData(Base64Encode(temp));
            }
            else
            {
                string type = "UTAMA";
                string status = "";
                string cond = "LogID LIKE \'%%\'";

                string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + cond + "!_!" + mainLogPageActive.ToString();
                await CashierLogbookService.getLogData(Base64Encode(temp));
            }
        }

        private async Task transitLogPageSelect(int currPage)
        {
            transitLogPageActive = currPage;

            if (isTransitLogFilterActive)
            {
                string type = "TRANSIT";
                string status = "";
                string cond = $"{transitLogFilterType} LIKE \'%{transitLogFilterValue}%\'";

                string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + cond + "!_!" + mainLogPageActive.ToString();
                await CashierLogbookService.getLogData(Base64Encode(temp));
            }
            else
            {
                string type = "TRANSIT";
                string status = "";
                string cond = "LogID LIKE \'%%\'";

                string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + cond + "!_!" + mainLogPageActive.ToString();
                await CashierLogbookService.getLogData(Base64Encode(temp));
            }
        }

        private async Task actionLogPageSelect(int currPage)
        {
            actionLogPageActive = currPage;

            if (isActionLogFilterActive)
            {
                string orderby = "AuditActionDate";
                string filType = "LogID";
                string filValue = "";

                string temp = activeUser.location + "!_!" + orderby + "!_!" + filType + "!_!" + filValue + "!_!" + actionLogPageActive.ToString();
                await CashierLogbookService.getBrankasActionLogData(Base64Encode(temp));
            }
            else
            {
                string orderby = "AuditActionDate";
                string filType = "LogID";
                string filValue = "";

                string temp = activeUser.location + "!_!" + orderby + "!_!" + filType + "!_!" + filValue + "!_!" + actionLogPageActive.ToString();
                await CashierLogbookService.getBrankasActionLogData(Base64Encode(temp));
            }
        }

        private async Task mainLogFilter()
        {
            if (mainLogFilterType.Length > 0)
            {
                mainLogPageActive = 1;
                isMainLogFilterActive = true;

                string type = "UTAMA";
                string status = "";
                string filType = mainLogFilterType;
                string filValue = mainLogFilterValue;

                CashierLogbookService.mainLogs.Clear();

                if (mainLogFilterType.Equals("LogDate"))
                {
                    string mainpz = "BrankasLog!_!" + activeUser.location + $"!_!LogType = \'UTAMA\' AND {filType} LIKE \'%{filValue}%\'";
                    mainLogPageSize = await CashierLogbookService.getModulePageSize(Base64Encode(mainpz));

                    string cond = $"{mainLogFilterType} BETWEEN \'{mainLogFilterDateValue.ToString("yyyyMMdd")}\' AND \'{mainLogFilterDateValue.ToString("yyyyMMdd")}\'";

                    string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + cond + "!_!" + mainLogPageActive.ToString();
                    await CashierLogbookService.getLogData(Base64Encode(temp));
                }
                else
                {
                    string mainpz = "BrankasLog!_!" + activeUser.location + $"!_!LogType = \'UTAMA\' AND {filType} LIKE \'%{filValue}%\'";
                    mainLogPageSize = await CashierLogbookService.getModulePageSize(Base64Encode(mainpz));

                    string cond = $"LogID LIKE \'%%\' AND {filType} LIKE \'%{filValue}%\'";

                    string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + cond + "!_!" + mainLogPageActive.ToString();
                    await CashierLogbookService.getLogData(Base64Encode(temp));
                }
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }
        }

        private async Task mainLogFilterReset()
        {
            mainLogPageActive = 1;
            isMainLogFilterActive = false;
            mainLogFilterType = "";
            mainLogFilterValue = "";

            string type = "UTAMA";
            string status = "";

            string mainpz = "BrankasLog!_!" + activeUser.location + "!_!LogType = \'UTAMA\'";
            mainLogPageSize = await CashierLogbookService.getModulePageSize(Base64Encode(mainpz));

            string cond = "LogID LIKE \'%%\'";

            string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + cond + "!_!" + mainLogPageActive.ToString();
            await CashierLogbookService.getLogData(Base64Encode(temp));
        }

        private async Task transitLogFilter()
        {
            if (transitLogFilterType.Length > 0)
            {
                transitLogPageActive = 1;
                isTransitLogFilterActive = true;

                string type = "TRANSIT";
                string status = "";
                string filType = transitLogFilterType;
                string filValue = transitLogFilterValue;

                CashierLogbookService.transitLogs.Clear();

                if (transitLogFilterType.Equals("LogDate"))
                {
                    string mainpz = "BrankasLog!_!" + activeUser.location + $"!_!LogType = \'TRANSIT\' AND {filType} LIKE \'%{filValue}%\'";
                    mainLogPageSize = await CashierLogbookService.getModulePageSize(Base64Encode(mainpz));

                    string cond = $"{mainLogFilterType} BETWEEN \'{mainLogFilterDateValue.ToString("yyyyMMdd")}\' AND \'{mainLogFilterDateValue.ToString("yyyyMMdd")}\'";

                    string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + cond + "!_!" + mainLogPageActive.ToString();
                    await CashierLogbookService.getLogData(Base64Encode(temp));
                }
                else
                {
                    string mainpz = "BrankasLog!_!" + activeUser.location + $"!_!LogType = \'TRANSIT\' AND {filType} LIKE \'%{filValue}%\'";
                    mainLogPageSize = await CashierLogbookService.getModulePageSize(Base64Encode(mainpz));

                    string cond = $"LogID LIKE \'%%\' AND {filType} LIKE \'%{filValue}%\'";

                    string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + cond + "!_!" + mainLogPageActive.ToString();
                    await CashierLogbookService.getLogData(Base64Encode(temp));
                }
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }
        }

        private async Task transitLogFilterReset()
        {
            mainLogPageActive = 1;
            isMainLogFilterActive = false;
            mainLogFilterType = "";
            mainLogFilterValue = "";

            string type = "TRANSIT";
            string status = "";

            string mainpz = "BrankasLog!_!" + activeUser.location + "!_!LogType = \'TRANSIT\'";
            mainLogPageSize = await CashierLogbookService.getModulePageSize(Base64Encode(mainpz));

            string cond = "LogID LIKE \'%%\'";

            string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + cond + "!_!" + mainLogPageActive.ToString();
            await CashierLogbookService.getLogData(Base64Encode(temp));
        }

        private void shiftSelect(Shift data)
        {
            foreach (var sh in activeShift)
            {
                sh.isActive = false;
            }

            activeShift.FirstOrDefault(x => x.ShiftID.Equals(data.ShiftID)).isActive = true;
        }

        private bool checkMainLogPresent()
        {
            try
            {
                if (CashierLogbookService.mainLogs.Any())
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

        private bool checkActionLogPresent()
        {
            try
            {
                if (CashierLogbookService.actionLogs.Any())
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

        private bool checkTransitLogPresent()
        {
            try
            {
                if (CashierLogbookService.transitLogs.Any())
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


        //
    }
}
