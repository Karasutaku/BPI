using BPIWebApplication.Shared.PagesModel.PettyCash;
using BPIWebApplication.Shared.MainModel.PettyCash;
using Microsoft.AspNetCore.Components;
using DocumentFormat.OpenXml.Office.Word;
using Microsoft.JSInterop;
using DocumentFormat.OpenXml.Office.CustomUI;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.DbModel;
using System;
using Microsoft.IdentityModel.Tokens;

namespace BPIWebApplication.Client.Pages.PettyCashPages
{
    public partial class AddReimbursment : ComponentBase
    {
        //private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();
        private ActiveUser activeUser = new();

        private Reimburse reimbursement = new Reimburse();
        private List<ReimbursementExpense> expenses = new List<ReimbursementExpense>();

        private bool triggerModal = false;
        private bool isLoading = false;
        private bool successUpload = false;

        private bool alertTrigger = false;
        private bool successAlert = false;
        private string alertBody = string.Empty;
        private string alertMessage = string.Empty;

        private List<Expense> selectedExpense = new List<Expense>();
        List<string> settlingExpense = new();

        private int expensePageActive = 0;
        private int expenseNumberofPage = 0;

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
            activeUser.userPrivileges = new();
            activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");

            LoginService.activeUser.userPrivileges = activeUser.userPrivileges;

            reimbursement = new Reimburse();
            resetForm();

            reimbursement.Applicant = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            reimbursement.LocationID = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1].Equals("") ? "HO" : Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];

            expensePageActive = 1;

            await PettyCashService.getCoabyModule("PettyCash");

            StateHasChanged();

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/PettyCashPages/AddReimbursment.razor.js");
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        activeUser.token = await sessionStorage.GetItemAsync<string>("token");
        //        activeUser.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
        //        activeUser.company = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0];
        //        activeUser.location = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
        //        activeUser.sessionId = await sessionStorage.GetItemAsync<string>("SessionId");
        //        activeUser.appV = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));
        //        activeUser.userPrivileges = new();
        //        activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");

        //        LoginService.activeUser.userPrivileges = activeUser.userPrivileges;
        //    }
        //}

        private bool checkUserPrivilegeViewable()
        {
            try
            {
                if (LoginService.activeUser.userPrivileges.Contains("VW"))
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

        private async void submitReimbursement()
        {
            isLoading = true;

            try
            {
                if (!reimbursement.lines.Any(x => x.AccountNo.IsNullOrEmpty()))
                {
                    if (LoginService.activeUser.userPrivileges.Contains("CR"))
                    {
                        ReimburseStream uploadData = new ReimburseStream();

                        uploadData.reimburseDetails = new();
                        uploadData.reimburseDetails.Data = new();
                        uploadData.files = new();

                        var reimburseId = await PettyCashService.createDocumentID("Reimburse");

                        reimbursement.ReimburseID = reimburseId.Data;
                        reimbursement.ReimburseDate = DateTime.Now;
                        reimbursement.ReimburseStatus = "Open";
                        reimbursement.ReimburseNote = "";

                        int nLine = 0;

                        foreach (var lines in reimbursement.lines)
                        {
                            nLine++;

                            lines.ReimburseID = reimburseId.Data;
                            lines.LineNo = nLine;
                        }

                        uploadData.reimburseDetails.Data = reimbursement;
                        uploadData.reimburseDetails.userEmail = activeUser.userName;
                        uploadData.reimburseDetails.userAction = "I";
                        uploadData.reimburseDetails.userActionDate = DateTime.Now;

                        foreach (var f in PettyCashService.fileStreams)
                        {
                            f.type = f.type + "!_!" + reimburseId.Data;
                            f.content = Array.Empty<byte>();
                        }

                        uploadData.files = PettyCashService.fileStreams;

                        QueryModel<List<string>> settleExpenseData = new();
                        settleExpenseData.Data = new();

                        settleExpenseData.Data = settlingExpense;
                        settleExpenseData.userEmail = activeUser.userName;
                        settleExpenseData.userAction = "D";
                        settleExpenseData.userActionDate = DateTime.Now;

                        var res = await PettyCashService.updateExpenseDataSettlement(settleExpenseData);

                        if (res.isSuccess)
                        {
                            var res2 = await PettyCashService.createReimburseData(uploadData);

                            if (res2.isSuccess)
                            {
                                string temp = "PettyCash!_!AddPCR!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + reimburseId.Data;
                                var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                if (res3.isSuccess)
                                {
                                    successUpload = true;
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success");
                                }
                                else
                                {
                                    successUpload = false;
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed");
                                }

                                isLoading = false;
                                successAlert = true;
                                alertMessage = "Create Reimbursement Success !";
                                alertBody = $"Your Reimburse ID is {reimburseId.Data}";

                                StateHasChanged();
                            }

                        }
                        else
                        {
                            isLoading = false;
                            alertTrigger = true;
                            alertMessage = "Create Reimbursement Failed !";
                            alertBody = "Please Recheck Your Input Field";

                            StateHasChanged();
                        }
                    }
                    else
                    {
                        isLoading = false;
                        alertTrigger = true;
                        alertMessage = "You Have no Authority to Create Document !";
                        alertBody = "Please try again or Contact the Administrator";

                        StateHasChanged();
                    }
                }
                else
                {
                    isLoading = false;
                    alertTrigger = true;
                    alertMessage = "Input COA Account !";
                    alertBody = "Please Recheck Your Input Field";

                    StateHasChanged();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void removeExpense(ReimbursementExpense data)
        {
            expenses.Remove(data);

            var itemRemove = expenses.SingleOrDefault(a => a.expense.ExpenseID == data.expense.ExpenseID);

            if (itemRemove != null)
            {
                expenses.Remove(itemRemove);
            }

            while (PettyCashService.fileStreams.FirstOrDefault(a => a.type.Equals(data.expense.ExpenseID)) != null)
            {
                var delItem = PettyCashService.fileStreams.FirstOrDefault(a => a.type.Equals(data.expense.ExpenseID));

                PettyCashService.fileStreams.Remove(delItem);
            }

            while (reimbursement.lines.FirstOrDefault(x => x.ExpenseID.Equals(data.expense.ExpenseID)) != null)
            {
                var delLines = reimbursement.lines.FirstOrDefault(x => x.ExpenseID.Equals(data.expense.ExpenseID));

                reimbursement.lines.Remove(delLines);
            }
        }

        private void appendExpenseSelected(Expense data)
        {
            if (selectedExpense.FirstOrDefault(a => a.ExpenseID == data.ExpenseID) == null)
            {
                selectedExpense.Add(data);
            }
            else
            {
                var itemRemove1 = selectedExpense.SingleOrDefault(a => a.ExpenseID == data.ExpenseID);

                if (itemRemove1 != null)
                {
                    selectedExpense.Remove(itemRemove1);
                }

            }
        }

        private async Task applySelectedExpense()
        {
            PettyCashService.fileStreams.Clear();
            settlingExpense.Clear();
            isLoading = true;

            try
            {
                reimbursement.lines.Clear();

                foreach (var exp in selectedExpense)
                {
                    string temps = exp.ExpenseID + "!_!MASTER";

                    var files = await PettyCashService.getAttachmentFileStream(Base64Encode(temps));

                    if (files.isSuccess)
                    {
                        List<BPIWebApplication.Shared.MainModel.Stream.FileStream> streams = new();

                        foreach (var f in files.Data)
                        {
                            BPIWebApplication.Shared.MainModel.Stream.FileStream temp = new();

                            temp = f;

                            streams.Add(temp);
                        }

                        expenses.Add(new ReimbursementExpense
                        {
                            expense = exp,
                            filestreams = streams
                        });
                    }
                    else
                    {
                        expenses.Add(new ReimbursementExpense
                        {
                            expense = exp,
                            filestreams = new()
                        });
                    }

                    settlingExpense.Add(exp.ExpenseID);

                    foreach (var lines in exp.lines)
                    {
                        reimbursement.lines.Add(new ReimburseLine
                        {
                            ReimburseID = "",
                            ExpenseID = lines.ExpenseID,
                            LineNo = lines.LineNo,
                            AccountNo = "",
                            Details = lines.Details,
                            Amount = lines.ActualAmount,
                            ApprovedAmount = decimal.Zero,
                            //Attach = "",
                            Status = "OP"
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                triggerModal = false;
                isLoading = false;
                selectedExpense.Clear();

                throw new Exception(ex.Message);
            }
            
            triggerModal = false;
            isLoading = false;
            selectedExpense.Clear();
        }

        private Stream GetFileStream(byte[] data)
        {
            var fileBinData = data;
            var fileStream = new MemoryStream(fileBinData);

            return fileStream;
        }

        private async Task HandleViewDocument(Byte[] content)
        {
            var filestream = GetFileStream(content);

            using var streamRef = new DotNetStreamReference(stream: filestream);

            await _jsModule.InvokeVoidAsync("downloadFileFromStream", "Attachment", streamRef);
        }

        private void hideModal()
        {
            selectedExpense.Clear();
            triggerModal = false;
        }

        private async Task showModal()
        {
            triggerModal = true;

            PettyCashService.expenses = new();

            string expStatus = "Submited";
            string expFilType = "";
            string expFilValue = "";

            string exppz = "Expense!_!ExpenseID!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + activeUser.location;
            expenseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(exppz));

            string explocPage = "MASTER!_!" + activeUser.location + "!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + expensePageActive.ToString();
            await PettyCashService.getExpenseDatabyLocation(Base64Encode(explocPage));
        }

        private async void resetForm()
        {
            selectedExpense.Clear();
            reimbursement.lines.Clear();
            expenses.Clear();

            reimbursement.Applicant = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            reimbursement.LocationID = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1].Equals("") ? "HO" : Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];

            PettyCashService.fileStreams.Clear();
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

        private bool checkExpenseDataPresent()
        {
            try
            {
                if (PettyCashService.expenses.Any())
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

        private async Task expensePageSelect(int currPage)
        {
            expensePageActive = currPage;

            string expStatus = "Confirmed";
            string expFilType = "";
            string expFilValue = "";

            string explocPage = activeUser.location + "!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + expensePageActive.ToString();
            await PettyCashService.getExpenseDatabyLocation(Base64Encode(explocPage));

        }

    }
}
