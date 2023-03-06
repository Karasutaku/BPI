using BPIWebApplication.Client.Services.PettyCashServices;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.CashierLogbook;
using BPIWebApplication.Shared.MainModel.Company;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.CashierLogbook;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.CashierLogBookPages
{
    public partial class CashierLogbookDashboard : ComponentBase
    {
        private ActiveUser activeUser = new();

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
            activeUser.token = await sessionStorage.GetItemAsync<string>("token");
            activeUser.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.company = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0];
            activeUser.location = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
            activeUser.sessionId = await sessionStorage.GetItemAsync<string>("SessionId");
            activeUser.appV = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));
            //activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");

            //LoginService.activeUser.userPrivileges = activeUser.userPrivileges;

            string type = "MAIN";
            string status = "";
            string filType = "";
            string filValue = "";
            mainLogPageActive = 1;

            string mainpz = "BrankasLog!_!" + activeUser.location + "!_!LogType!_!MAIN";
            mainLogPageSize = await CashierLogbookService.getModulePageSize(Base64Encode(mainpz));
            string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + filType + "!_!" + filValue + "!_!" + mainLogPageActive.ToString();
            await CashierLogbookService.getLogData(Base64Encode(temp));

            type = "TRANSIT";
            transitLogPageActive = 1;

            string transpz = "BrankasLog!_!" + activeUser.location + "!_!LogType!_!TRANSIT";
            transitLogPageSize = await CashierLogbookService.getModulePageSize(Base64Encode(transpz));
            temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + filType + "!_!" + filValue + "!_!" + mainLogPageActive.ToString();
            await CashierLogbookService.getLogData(Base64Encode(temp));

            string orderby = "AuditActionDate";
            filType = "LogID";
            filValue = "";
            actionLogPageActive = 1;

            string actpz = "BrankasApproveLog!_!" + activeUser.location + "!_!LogID!_!";
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

            if (tp.Equals("MAIN"))
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
                temp = data.LogID + "!_!MAIN";
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

                    activeLog.approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).ConfirmUser = activeUser.userName;
                    activeLog.approvals.FirstOrDefault(x => x.ShiftID.Equals(activeShift.Where(y => y.isActive.Equals(true)).FirstOrDefault().ShiftID)).ConfirmDate = DateTime.Now;
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

        private async Task mainLogPageSelect(int currPage)
        {
            mainLogPageActive = currPage;

            if (isMainLogFilterActive)
            {
                string type = "MAIN";
                string status = "";
                string filType = "";
                string filValue = "";

                string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + filType + "!_!" + filValue + "!_!" + mainLogPageActive.ToString();
                await CashierLogbookService.getLogData(Base64Encode(temp));
            }
            else
            {
                string type = "MAIN";
                string status = "";
                string filType = "";
                string filValue = "";

                string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + filType + "!_!" + filValue + "!_!" + mainLogPageActive.ToString();
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
                string filType = "";
                string filValue = "";

                string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + filType + "!_!" + filValue + "!_!" + mainLogPageActive.ToString();
                await CashierLogbookService.getLogData(Base64Encode(temp));
            }
            else
            {
                string type = "TRANSIT";
                string status = "";
                string filType = "";
                string filValue = "";

                string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + filType + "!_!" + filValue + "!_!" + mainLogPageActive.ToString();
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
