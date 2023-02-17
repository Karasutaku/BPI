﻿using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.FileUploadModel;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.PettyCash;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.PettyCashPages
{
    public partial class AddExpense : ComponentBase
    {
        //private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();
        private ActiveUser activeUser = new();

        private Expense expense = new Expense();
        private List<ExpenseLine> expenseLines = new List<ExpenseLine>();
        private List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileLines = new();

        private Advance selectedAdvance = new();

        private int advancePageActive = 0;
        private int advanceNumberofPage = 0;

        private bool isTypeTransfer = false;
        private bool isSettlement = false;
        private bool clearInputFile = false;
        private bool successUpload = false;

        private bool alertTrigger = false;
        private bool successAlert = false;
        private string alertBody = string.Empty;
        private string alertMessage = string.Empty;

        private bool pdfInputFile = false;
        private bool cameraInputFile = true;

        private bool showModal = false;

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

            expense = new Expense();
            resetForm();

            expense.Applicant = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            expense.LocationID = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1].Equals("") ? "HO" : Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];

            advancePageActive = 1;

            await ManagementService.GetAllDepartment();

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/PettyCashPages/AddExpense.razor.js");
        }

        IReadOnlyList<IBrowserFile> listFileUpload;

        private async void UploadHandleSelection(InputFileChangeEventArgs files, BPIWebApplication.Shared.MainModel.Stream.FileStream line)
        {
            listFileUpload = files.GetMultipleFiles();
            string trustedFilename = string.Empty;

            if (listFileUpload != null)
            {
                foreach (var file in listFileUpload)
                {
                    FileInfo fi = new FileInfo(file.Name);
                    string ext = fi.Extension;

                    Stream stream = file.OpenReadStream(file.Size);
                    MemoryStream ms = new MemoryStream();
                    await stream.CopyToAsync(ms);

                    stream.Close();

                    line.type = "Expense";
                    line.fileName = Path.GetRandomFileName() + "!_!" + fi.Name;
                    line.fileType = ext;
                    line.fileSize = Convert.ToInt32(file.Size);
                    line.content = ms.ToArray();
                    
                }
            }

            this.StateHasChanged();
        }

        private async void submitExpense()
        {
            //
            if (!validateInput())
            {
                successAlert = false;
                alertTrigger = true;
                alertMessage = "Blank Input Field !";
                alertBody = "Please Recheck and Fill the blank Field";

                StateHasChanged();
            }
            else
            {
                try
                {
                    if (LoginService.activeUser.userPrivileges.Contains("CR"))
                    {
                        ExpenseStream uploadData = new ExpenseStream();

                        uploadData.expenseDetails = new();
                        uploadData.expenseDetails.Data = new();
                        uploadData.files = new();

                        var expenseId = await PettyCashService.createDocumentID("Expense");

                        expense.ExpenseID = expenseId.Data;
                        uploadData.expenseDetails.Data = expense;

                        uploadData.expenseDetails.Data.ExpenseDate = DateTime.Now;
                        uploadData.expenseDetails.Data.ExpenseStatus = "Open";

                        //uploadData.expenseDetails.Data.lines = expenseLines;
                        int nLine = 0;

                        foreach (var line in expenseLines)
                        {
                            nLine++;

                            ExpenseLine temp = new ExpenseLine
                            {
                                ExpenseID = expenseId.Data,
                                LineNo = nLine,
                                Details = line.Details,
                                Amount = line.Amount,
                                ActualAmount = line.ActualAmount,
                                Attach = line.Attach,
                                Status = "OP"
                            };

                            uploadData.expenseDetails.Data.lines.Add(temp);
                        }

                        uploadData.expenseDetails.userEmail = activeUser.userName;
                        uploadData.expenseDetails.userAction = "I";
                        uploadData.expenseDetails.userActionDate = DateTime.Now;

                        foreach (var f in fileLines)
                        {
                            f.type = expenseId.Data;
                        }

                        uploadData.files = fileLines;


                        //var res = await PettyCashService.createExpenseData(uploadData);
                        //var res2 = await PettyCashService.updateAdvanceDataSettlement(expense.AdvanceID);

                        // settlement

                        if (isSettlement)
                        {
                            QueryModel<string> updateData = new();
                            updateData.Data = expense.AdvanceID;
                            updateData.userEmail = activeUser.userName;
                            updateData.userAction = "U";
                            updateData.userActionDate = DateTime.Now;

                            var res = await PettyCashService.updateAdvanceDataSettlement(updateData);

                            if (res.isSuccess)
                            {

                                var res2 = await PettyCashService.createExpenseData(uploadData);

                                if (res2.isSuccess)
                                {
                                    string temp1 = "PettyCash!_!AddDocument!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + expenseId.Data;
                                    var res3 = await PettyCashService.autoEmail(Base64Encode(temp1));

                                    if (res3.isSuccess)
                                    {
                                        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success");
                                    }
                                    else
                                    {
                                        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed");
                                    }

                                    successUpload = true;
                                    alertTrigger = false;
                                    successAlert = true;
                                    alertMessage = "Create Expense Success !";
                                    alertBody = $"Your Expense ID is {expenseId.Data}";

                                    StateHasChanged();
                                }
                                else
                                {
                                    successUpload = false;
                                    successAlert = false;
                                    alertTrigger = true;
                                    alertMessage = "Create Expense Failed !";
                                    alertBody = "Please Recheck Your Input Field";

                                    StateHasChanged();
                                }
                            }
                            else
                            {
                                throw new Exception("Settle Error");
                            }

                        }
                        else
                        {
                            var res2 = await PettyCashService.createExpenseData(uploadData);

                            if (res2.isSuccess)
                            {
                                string temp1 = "PettyCash!_!AddDocument!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + expenseId.Data;
                                var res3 = await PettyCashService.autoEmail(Base64Encode(temp1));

                                if (res3.isSuccess)
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success");
                                }
                                else
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed");
                                }

                                successUpload = true;
                                alertTrigger = false;
                                successAlert = true;
                                alertMessage = "Create Expense Success !";
                                alertBody = $"Your Expense ID is {expenseId.Data}";

                                StateHasChanged();
                            }
                            else
                            {
                                successUpload = false;
                                successAlert = false;
                                alertTrigger = true;
                                alertMessage = "Create Expense Failed !";
                                alertBody = "Please Recheck Your Input Field";

                                StateHasChanged();
                            }
                        }
                    }
                    else
                    {
                        successUpload = false;
                        successAlert = false;
                        alertTrigger = true;
                        alertMessage = "You Have no Authority to Create Document !";
                        alertBody = "Please try again or Contact the Administrator";

                        StateHasChanged();
                    }
                    
                }
                catch (Exception ex)
                {
                    successAlert = false;
                    alertTrigger = true;
                    alertMessage = "Error !";
                    alertBody = ex.Message;
                }
            }

        }

        private void addLine()
        {
            expenseLines.Add(new ExpenseLine());
        }

        private void addFileLine()
        {
            fileLines.Add(new BPIWebApplication.Shared.MainModel.Stream.FileStream());
        }

        private void deleteLine(ExpenseLine data)
        {
            expenseLines.Remove(data);
        }

        private void deleteFileLine(BPIWebApplication.Shared.MainModel.Stream.FileStream data)
        {
            fileLines.Remove(data);
        }

        private void setExpenseType(string value)
        {
            expense.TypeAccount = string.Empty;
            expense.ExpenseType = value;

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
            isSettlement = false;
            clearInputFile = !clearInputFile;

            listFileUpload = null;

            expense.ExpenseID = "";
            expense.AdvanceID = "";
            expense.ExpenseDate = DateTime.Now;
            expense.ExpenseStatus = "";
            expense.ExpenseNIK = "";
            expense.ExpenseNote = "";
            expense.ExpenseType = "CH";
            expense.TypeAccount = "";
            expense.DepartmentID = "";
            expense.lines = new();

            expenseLines.Clear();
            fileLines.Clear();
        }

        private bool validateInput()
        {

            if (expense.Applicant.IsNullOrEmpty())
                return false;

            if (expense.ExpenseNote.IsNullOrEmpty())
                return false;

            if (expense.LocationID.IsNullOrEmpty())
                return false;

            if (expense.DepartmentID.IsNullOrEmpty())
                return false;

            if (expense.ExpenseType.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                if (expense.ExpenseType.Equals("TF"))
                {
                    if (expense.TypeAccount.IsNullOrEmpty())
                        return false;
                }

            }

            if (!expenseLines.Any())
                return false;

            if (!fileLines.Any())
                return false;

            if (fileLines.First().fileSize < 1)
                return false;

            return true;
        }

        private void modalHide()
        {
            showModal = false;
            selectedAdvance = null;
        } 

        private async Task selectAdvance()
        {
            showModal = true;
            advancePageActive = 1;

            PettyCashService.advances = new();

            string status = "Confirmed";
            string filType = "USER";
            string filValue = activeUser.userName;

            string advpz = "Advance!_!AdvanceID!_!" + status + "!_!" + filType + "!_!" + filValue + "!_!" + activeUser.location;
            advanceNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(advpz));

            string temp = "MASTER!_!" + activeUser.location + "!_!" + status + "!_!" + filType + "!_!" + filValue + "!_!" + advancePageActive;
            await PettyCashService.getAdvanceDatabyLocation(Base64Encode(temp));
        }

        private void selectAdvanceItem(Advance data)
        {
            selectedAdvance = new();
            selectedAdvance = data;
        }

        private void modalAdvanceSelect()
        {
            expenseLines.Clear();

            expense.ExpenseID = "";
            expense.AdvanceID = selectedAdvance.AdvanceID;
            expense.ExpenseDate = selectedAdvance.AdvanceDate;
            expense.ExpenseStatus = "";
            expense.ExpenseNIK = selectedAdvance.AdvanceNIK;
            expense.ExpenseNote = selectedAdvance.AdvanceNote;
            expense.ExpenseType = selectedAdvance.AdvanceType;
            //expense.Applicant = selectedAdvance.Applicant;

            if (selectedAdvance.AdvanceType.Equals("CH"))
            {
                isTypeTransfer = false;
            }
            else if (selectedAdvance.AdvanceType.Equals("TF"))
            {
                isTypeTransfer = true;
            }

            expense.TypeAccount = selectedAdvance.TypeAccount;
            expense.DepartmentID = selectedAdvance.DepartmentID;
            expense.lines = new();

            foreach (var line in selectedAdvance.lines)
            {
                expenseLines.Add(new ExpenseLine
                {
                    ExpenseID = "",
                    LineNo = line.LineNo,
                    Details = line.Details,
                    Amount = line.Amount,
                    ActualAmount = decimal.Zero,
                    Attach = "",
                    Status = "OP"
                });
            }

            isSettlement = true;
            showModal = false;
            StateHasChanged();

            selectedAdvance = null;
        }

        private bool checkAdvanceDataPresent()
        {
            try
            {
                if (PettyCashService.advances.Any())
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

        private async Task advancePageSelect(int currPage)
        {
            advancePageActive = currPage;

            PettyCashService.advances.Clear();

            string status = "Confirmed";
            string filType = "USER";
            string filValue = activeUser.userName;

            string temp = activeUser.location + "!_!" + status + "!_!" + filType + "!_!" + filValue + "!_!" + advancePageActive;
            await PettyCashService.getAdvanceDatabyLocation(Base64Encode(temp));

        }


    }
}
