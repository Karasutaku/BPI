using BPIWebApplication.Shared.MainModel.CashierLogbook;
using BPIWebApplication.Shared.MainModel.Company;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.CashierLogbook;
using Microsoft.AspNetCore.Components;

namespace BPIWebApplication.Client.Pages.CashierLogBook
{
    public partial class AddLogbook : ComponentBase
    {
        private ActiveUser activeUser = new();

        //private Shift activeShift { get; set; } = new();
        //private AmountCategories activeCategories { get; set; } = new();

        private CashierLogData logbook { get; set; } = new();

        private int selectedShiftID { get; set; } = 0;
        private string selectedCategoryID { get; set; } = string.Empty;

        private bool alertTrigger = false;
        private bool successAlert = false;
        private string alertBody = string.Empty;
        private string alertMessage = string.Empty;

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
        }

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
                    AmountSubCategoryID = "",
                    AmountSubCategoryName = "",
                    AmountType = "",
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
                    AmountSubCategoryID = "",
                    AmountSubCategoryName = "",
                    AmountType = "",
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
