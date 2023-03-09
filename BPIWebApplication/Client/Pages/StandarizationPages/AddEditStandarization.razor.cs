using BPIWebApplication.Client.Services.ManagementServices;
using BPIWebApplication.Client.Services.PettyCashServices;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.Standarizations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.StandarizationPages
{
    public partial class AddEditStandarization : ComponentBase
    {
        [Parameter]
        public string? param { get; set; }

        private bool alertTrigger = false;
        private bool successAlert = false;
        private string alertMessage = string.Empty;
        private string alertBody = string.Empty;

        private bool successUpload = false;
        private bool isLoading = false;
        private bool isConfirmationActive = false;

        private Standarizations standarizationData = new();
        private List<StandarizationTag> tags = new();
        private List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileLines = new();

        private ActiveUser activeUser = new();

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

            await StandarizationService.getStandarizationTypes();

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/StandarizationPages/AddEditStandarization.razor.js");
        }

        IReadOnlyList<IBrowserFile>? listFileUpload;

        private bool fileValidation(string ext)
        {
            if (!ext.Contains("pdf", StringComparison.OrdinalIgnoreCase))
                return false;

            if (!ext.Contains("xlsx", StringComparison.OrdinalIgnoreCase))
                return false;

            if (!ext.Contains("xls", StringComparison.OrdinalIgnoreCase))
                return false;

            if (!ext.Contains("mp4", StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        private bool validateInput()
        {
            if (standarizationData.TypeID.IsNullOrEmpty())
                return false;

            if (standarizationData.StandarizationID.IsNullOrEmpty())
                return false;

            if (standarizationData.StandarizationDetails.IsNullOrEmpty())
                return false;

            if (!tags.Any())
                return false;

            if (tags.Any(x => x.TagDescriptions.IsNullOrEmpty()))
                return false;

            if (!fileLines.Any())
                return false;

            if (fileLines.Any(x => x.fileDesc.IsNullOrEmpty()))
                return false;

            return true;
        }

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

                    if (fileValidation(ext))
                    {
                        successUpload = false;
                        successAlert = false;
                        alertTrigger = true;
                        alertMessage = "File Selection Failed !";
                        alertBody = "File Extention is not Supported";

                        StateHasChanged();
                    }
                    else
                    {
                        Stream stream = file.OpenReadStream(file.Size);
                        MemoryStream ms = new MemoryStream();
                        await stream.CopyToAsync(ms);

                        stream.Close();

                        line.type = "";
                        line.fileName = Path.GetRandomFileName() + "!_!" + fi.Name;
                        line.fileDesc = "";
                        line.fileType = ext;
                        line.fileSize = Convert.ToInt32(file.Size);
                        line.content = ms.ToArray();
                    }
                }
            }

            this.StateHasChanged();
        }

        private async void createStandarization()
        {
            try
            {
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
                    if (LoginService.activeUser.userPrivileges.Contains("CR"))
                    {
                        isLoading = true;

                        StandarizationStream uploadData = new();

                        uploadData.standarizationDetails = new();
                        uploadData.files = new();

                        QueryModel<Standarizations> temp = new();
                        temp.Data = new();
                        temp.Data.Tags = new();
                        temp.Data.Attachments = new();

                        temp.Data = standarizationData;
                        
                        foreach (var tag in tags)
                        {
                            temp.Data.Tags.Add(new StandarizationTag
                            {
                                StandarizationID = standarizationData.StandarizationID,
                                TagDescriptions = tag.TagDescriptions
                            });
                        }
                        
                        foreach (var f in fileLines)
                        {
                            FileInfo fi = new FileInfo(f.fileName);

                            temp.Data.Attachments.Add(new StandarizationAttachment
                            {
                                StandarizationID = standarizationData.StandarizationID,
                                Descriptions = f.fileDesc,
                                FileExtention = fi.Extension,
                                FilePath = f.fileName,
                                UploadDate = DateTime.Now
                            });
                        }

                        temp.userEmail = activeUser.userName;
                        temp.userAction = "I";
                        temp.userActionDate = DateTime.Now;

                        uploadData.standarizationDetails = temp;
                        uploadData.files = fileLines;

                        var res = await StandarizationService.createStandarizations(uploadData);

                        if (res.isSuccess)
                        {
                            await _jsModule.InvokeVoidAsync("showAlert", "Create Document Success !");

                            successUpload = true;
                            isLoading = false;
                            alertTrigger = false;
                            successAlert = true;
                            alertMessage = "Create Expense Success !";
                            alertBody = $"Your Document ID is {res.Data.Data.StandarizationID}";

                            StateHasChanged();
                        }
                        else
                        {
                            await _jsModule.InvokeVoidAsync("showAlert", "Create Document Failed, Please Check Your Connection !");

                            successUpload = false;
                            isLoading = false;
                            alertTrigger = true;
                            successAlert = false;
                            alertMessage = "Create Expense Failed !";
                            alertBody = "";

                            StateHasChanged();
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Create Document Failed, Error : {ex.Message}");
            }
        }

        private bool checkStandarizationTypesDataPresent()
        {
            try
            {
                if (StandarizationService.standarizationTypes.Any())
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
