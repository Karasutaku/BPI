using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.PettyCash;
using BPIWebApplication.Shared.DbModel;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop.Implementation;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.PettyCashPages
{
    public partial class AddAdvance : ComponentBase
    {
        //private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();
        private ActiveUser activeUser = new();

        private Advance advance = new Advance();
        private List<AdvanceLine> advanceLines = new List<AdvanceLine>();

        private bool isTypeTransfer = false;
        private bool isUserHaventSettled = false;
        private bool isLoading = false;

        private bool alertTrigger = false;
        private bool successAlert = false;
        private string alertBody = string.Empty;
        private string alertMessage = string.Empty;

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

            //activeUser.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            //activeUser.UserLogin = new LoginUser();
            //activeUser.UserLogin.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userEmail"));
            //activeUser.role = Base64Decode(await sessionStorage.GetItemAsync<string>("role"));

            await InitPage();
            await ManagementService.GetAllDepartment();

            string temp = activeUser.userName + "!_!MASTER";

            var res = await PettyCashService.getAdvanceDatabyUser(Base64Encode(temp));

            if (res.isSuccess)
            {
                if (res.ErrorCode.Contains("01"))
                {
                    isUserHaventSettled = false;
                }
                else
                {
                    isUserHaventSettled = true;
                }

            }
            else
            {
                isUserHaventSettled = true;
            }

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/PettyCashPages/AddAdvance.razor.js");
        }

        //private bool checkUserPrivilegeViewable()
        //{
        //    try
        //    {
        //        if (LoginService.activeUser.userPrivileges.Contains("VW"))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        private async Task InitPage()
        {
            advance = new Advance();
            advance.AdvanceType = "CH";

            //advance.Applicant = LoginService.activeUser.userName;
            advance.Applicant = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            advance.LocationID = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1].Equals("") ? "HO" : Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];

            //if (ManagementService.locations.FirstOrDefault(loc => loc.locationId.Equals(LoginService.activeUser.location)) != null)
            //{
            //    advance.LocationID = ManagementService.locations.SingleOrDefault(loc => loc.locationId.Equals(LoginService.activeUser.location)).locationName;
            //}
            //else
            //{
            //    if (LoginService.activeUser.location.Equals("") && !LoginService.activeUser.company.IsNullOrEmpty())
            //    {
            //        advance.LocationID = "HO";
            //    }
            //    else
            //    {
            //        advance.LocationID = "Err : Location Not Found";
            //    }
            //}
        }

        private async void submitAdvance()
        {
            try
            {
                if (!validateInput())
                {
                    successAlert = false;
                    alertTrigger = true;
                    alertMessage = "Blank Input Field !";
                    alertBody = "Please Fill the blank Field";

                    StateHasChanged();
                }
                else
                {
                    if (LoginService.activeUser.userPrivileges.Contains("CR"))
                    {
                        isLoading = true;

                        var advanceId = await PettyCashService.createDocumentID("Advance");

                        advance.AdvanceID = advanceId.Data;
                        advance.AdvanceDate = DateTime.Now;
                        advance.Approver = advance.Approver.ToLower();

                        QueryModel<Advance> inputData = new();
                        inputData.Data = new();
                        inputData.Data.lines = new();

                        inputData.Data = advance;

                        inputData.Data.AdvanceID = advanceId.Data;
                        inputData.Data.AdvanceStatus = "Open";
                        inputData.userEmail = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
                        inputData.userAction = "I";
                        inputData.userActionDate = DateTime.Now;

                        int nLine = 0;

                        foreach (var line in advanceLines)
                        {
                            nLine++;

                            AdvanceLine temp = new AdvanceLine
                            {
                                AdvanceID = advanceId.Data,
                                LineNo = nLine,
                                Details = line.Details,
                                Amount = line.Amount,
                                Status = "AP"
                            };

                            inputData.Data.lines.Add(temp);
                        }

                        var res = await PettyCashService.createAdvanceData(inputData);

                        if (res.isSuccess)
                        {
                            string temp = "PettyCash!_!AddDocument!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + advanceId.Data;
                            var res2 = await PettyCashService.autoEmail(Base64Encode(temp));

                            if (res2.isSuccess)
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success");
                            }
                            else
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed");
                            }

                            alertTrigger = false;
                            successAlert = true;
                            alertMessage = "Create Advance Success !";
                            alertBody = $"Your Advance ID is {advanceId.Data}";

                            isUserHaventSettled = true;
                            isLoading = false;
                            StateHasChanged();
                        }
                        else
                        {
                            alertTrigger = false;
                            successAlert = true;
                            alertMessage = "Create Advance Failed !";
                            alertBody = $"Please Check Your Connection";

                            isUserHaventSettled = false;
                            isLoading = false;
                            StateHasChanged();
                        }
                    }
                    else
                    {
                        successAlert = false;
                        alertTrigger = true;
                        alertMessage = "You Have no Authority to Create Document !";
                        alertBody = "Please try again or Contact the Administrator";

                        StateHasChanged();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void addLine()
        {
            advanceLines.Add(new AdvanceLine());
        }

        private void deleteLine(AdvanceLine data)
        {
            advanceLines.Remove(data);
        }

        private void setAdvanceType(string value)
        {
            advance.TypeAccount = string.Empty;
            advance.AdvanceType = value;

            if (value.Equals("TF"))
            {
                isTypeTransfer = true;
            }
            else
            {
                isTypeTransfer = false;
            }

            StateHasChanged();
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

        private void resetForm()
        {
            isTypeTransfer = false;
            isLoading = false;

            advance.AdvanceID = "";
            advance.AdvanceDate = DateTime.Now;
            advance.AdvanceStatus = "";
            advance.AdvanceNIK = "";
            advance.AdvanceNote = "";
            advance.AdvanceType = "CH";
            advance.TypeAccount = "";
            advance.DepartmentID = "";
            advance.Approver = "";
            advance.lines = new();

            advanceLines.Clear();
        }

        private bool validateInput()
        {
            if (advance.Applicant.IsNullOrEmpty())
                return false;

            if (advance.AdvanceNote.IsNullOrEmpty())
                return false;

            if (advance.LocationID.IsNullOrEmpty())
                return false;

            if (advance.DepartmentID.IsNullOrEmpty())
                return false;

            if (advance.Applicant.IsNullOrEmpty())
                return false;

            if (activeUser.location.Equals(""))
            {
                if (advance.Approver.IsNullOrEmpty() || !advance.Approver.Contains('@'))
                    return false;
            }
            
            if (advance.AdvanceType.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                if (advance.AdvanceType.Equals("TF"))
                {
                    if (advance.TypeAccount.IsNullOrEmpty())
                        return false;
                }

            }

            if (!advanceLines.Any())
                return false;

            if (advanceLines.Any(x => x.Details.IsNullOrEmpty()) || advanceLines.Any(x => x.Amount.Equals(Decimal.Zero)))
                return false;

            return true;
        }

    }
}
