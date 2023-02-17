using BPIWebApplication.Client.Services.PettyCashServices;
using BPIWebApplication.Shared.MainModel.CashierLogbook;
using BPIWebApplication.Shared.MainModel.Company;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.CashierLogbook;
using Microsoft.AspNetCore.Components;

namespace BPIWebApplication.Client.Pages.CashierLogBook
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

        private bool showModal = false;

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

            string type = "MAIN";
            string status = "";
            string filType = "";
            string filValue = "";
            mainLogPageActive = 1;

            string temp = type + "!_!" + activeUser.location + "!_!" + status + "!_!" + filType + "!_!" + filValue + "!_!" + mainLogPageActive.ToString();

            await CashierLogbookService.getLogData(Base64Encode(temp));
            await CashierLogbookService.getShiftData("CashierLogbook");

            //_jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/PettyCashPages/PettyCashDashboard.razor.js");
        }

        private void hideModal() => showModal = false;

        private void showMainModal(CashierLogData data)
        {
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
