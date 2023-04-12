using BPIWebApplication.Client.Services.ManagementServices;
using BPIWebApplication.Client.Services.PettyCashServices;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.Standarizations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
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
        private bool isConfirmationDeletion = false;

        private int maxFileSize = 0;
        private string[] acceptedFileExtensions = new string[] {};

        private Standarizations standarizationData = new();
        private List<StandarizationTag> tags = new();
        private List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileLines = new();

        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

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

            maxFileSize = await StandarizationService.getStandarizationMaxFileSize();
            acceptedFileExtensions = await StandarizationService.getStandarizationAcceptedFileExtension();

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/StandarizationPages/AddEditStandarization.razor.js");
        }

        private void hideConfirmationModalonESC(KeyboardEventArgs e) { try { if (e.Key.Equals("Escape")) { isConfirmationActive = false; } } catch (Exception exc) { } }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                if (param != null)
                {
                    var temp = Base64Decode(param).Split("!_!");
                    var data = StandarizationService.standarizations.SingleOrDefault(x => x.StandarizationID.Equals(temp[0]));

                    if (data != null)
                    {
                        isLoading = true;
                        standarizationData = data;

                        if (!temp[0].Equals(temp[1]))
                        {
                            StandarizationService.fileStreams.Clear();

                            string temp1 = data.StandarizationID + "!_!" + data.TypeID;

                            await Task.Run(async () => { await StandarizationService.getFileStream(Base64Encode(temp1)); });
                        }

                        tags.Clear();
                        fileLines.Clear();

                        foreach (var tag in standarizationData.Tags)
                        {
                            tags.Add(new StandarizationTag
                            {
                                StandarizationID = tag.StandarizationID,
                                TagDescriptions = tag.TagDescriptions
                            });
                        }

                        foreach (var file in standarizationData.Attachments)
                        {
                            fileLines.Add(new BPIWebApplication.Shared.MainModel.Stream.FileStream
                            {
                                type = file.StandarizationID,
                                fileName = file.FilePath,
                                fileDesc = file.Descriptions,
                                fileType = file.FileExtention,
                                fileSize = 0,
                                content = new Byte[0]
                            });
                        }
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Data Not Found !");
                    }
                }

                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                isLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", $"Fetch Data Failed : Error {ex.Message} ");
            }
        }

        private Stream GetFileStream(byte[] data)
        {
            var fileBinData = data;
            var fileStream = new MemoryStream(fileBinData);

            return fileStream;
        }

        private async Task HandleDownloadDocument(Byte[] content, string filename)
        {
            var filestream = GetFileStream(content);

            using var streamRef = new DotNetStreamReference(stream: filestream);

            await _jsModule.InvokeVoidAsync("exportStream", filename, streamRef);
        }

        private async Task downloadStandarizationFile(StandarizationAttachment data)
        {
            try
            {
                if (StandarizationService.fileStreams.SingleOrDefault(x => x.type.Equals(data.StandarizationID) && x.fileName.Equals(data.FilePath)) != null)
                {
                    isLoading = true;

                    var content = StandarizationService.fileStreams.SingleOrDefault(x => x.type.Equals(data.StandarizationID) && x.fileName.Equals(data.FilePath)).content;
                    string filename = data.FilePath.Split("!_!")[1];

                    await HandleDownloadDocument(content, filename);

                    isLoading = false;
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Fetch Data Failed - Refresh Your Page and Try Again !");
                }
            }
            catch (Exception ex)
            {
                isLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message}");
            }
        }

        IReadOnlyList<IBrowserFile>? listFileUpload;

        //private bool fileValidation(string ext)
        //{
        //    if (!ext.Contains("pdf", StringComparison.OrdinalIgnoreCase))
        //        return false;

        //    if (!ext.Contains("xlsx", StringComparison.OrdinalIgnoreCase))
        //        return false;

        //    if (!ext.Contains("xls", StringComparison.OrdinalIgnoreCase))
        //        return false;

        //    if (!ext.Contains("mp4", StringComparison.OrdinalIgnoreCase))
        //        return false;

        //    return true;
        //}

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

            if (maxFileSize == 0)
            {
                successAlert = false;
                alertTrigger = true;
                alertMessage = "Fail Fetch Max File Size !";
                alertBody = "Please Check Your Connection";

                await _jsModule.InvokeVoidAsync("showAlert", "File Size Parameter is Invalid !");

                isLoading = false;
            }
            else
            {
                if (listFileUpload != null)
                {
                    foreach (var file in listFileUpload)
                    {
                        FileInfo fi = new FileInfo(file.Name);
                        string ext = fi.Extension;

                        if (!acceptedFileExtensions.Any(x => x.Equals(ext)) || file.Size > (1024 * 1024 * maxFileSize))
                        {
                            successUpload = false;
                            successAlert = false;
                            alertTrigger = true;
                            alertMessage = "File Selection Failed !";
                            alertBody = "File Extention / File Size is not Supported";

                            await _jsModule.InvokeVoidAsync("showAlert", "Invalid File Extensions / File Size Exceeded Limit !");

                            StateHasChanged();
                        }
                        else
                        {
                            Stream stream = file.OpenReadStream(file.Size);
                            MemoryStream ms = new MemoryStream();
                            await stream.CopyToAsync(ms);

                            stream.Close();

                            //line.type = "";
                            line.fileName = Path.GetRandomFileName() + "!_!" + fi.Name;
                            //line.fileDesc = "";
                            line.fileType = ext;
                            line.fileSize = Convert.ToInt32(file.Size);
                            line.content = ms.ToArray();
                        }
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
                                rowGuid = Guid.Empty,
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
                            alertMessage = "Create Document Success !";
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
                            alertMessage = "Create Document Failed !";
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

        private async void editStandarization()
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
                    if (LoginService.activeUser.userPrivileges.Contains("ED"))
                    {
                        isLoading = true;

                        StandarizationStream uploadData = new();

                        uploadData.standarizationDetails = new();
                        uploadData.files = new();

                        QueryModel<Standarizations> temp = new();
                        temp.Data = new();
                        temp.Data = standarizationData;

                        temp.Data.Tags = new();
                        temp.Data.Attachments = new();

                        foreach (var tag in tags)
                        {
                            temp.Data.Tags.Add(new StandarizationTag
                            {
                                rowGuid = Guid.Empty,
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
                        temp.userAction = "U";
                        temp.userActionDate = DateTime.Now;

                        uploadData.standarizationDetails = temp;
                        uploadData.files = fileLines;

                        var res = await StandarizationService.updateStandarizations(uploadData);

                        if (res.isSuccess)
                        {
                            await _jsModule.InvokeVoidAsync("showAlert", "Edit Document Success !");

                            successUpload = true;
                            isLoading = false;
                            alertTrigger = false;
                            successAlert = true;
                            alertMessage = "Edit Document Success !";
                            alertBody = $"Your Document ID is {res.Data.Data.StandarizationID}";

                            StateHasChanged();
                        }
                        else
                        {
                            await _jsModule.InvokeVoidAsync("showAlert", "Edit Document Failed, Please Check Your Connection !");

                            successUpload = false;
                            isLoading = false;
                            alertTrigger = true;
                            successAlert = false;
                            alertMessage = "Edit Document Failed !";
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

        private async void deleteStandarization()
        {
            try
            {
                if (param == null)
                {
                    successAlert = false;
                    alertTrigger = true;
                    alertMessage = "Parameter Data Empty !";
                    alertBody = "Please Reselect your Document";

                    StateHasChanged();
                }
                else
                {
                    isLoading = true;

                    string temp = standarizationData.StandarizationID + "!_!" + standarizationData.TypeID;

                    QueryModel<string> deleteData = new();

                    deleteData.Data = Base64Encode(temp);
                    deleteData.userEmail = activeUser.userName;
                    deleteData.userAction = "D";
                    deleteData.userActionDate = DateTime.Now;

                    var res = await StandarizationService.deleteStandarizations(deleteData);

                    if (res.isSuccess)
                    {
                        
                        await _jsModule.InvokeVoidAsync("showAlert", "Delete Document Success !");

                        successUpload = true;
                        isLoading = false;
                        alertTrigger = false;
                        successAlert = true;
                        alertMessage = "Delete Document Success !";
                        alertBody = "";

                        StateHasChanged();
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Delete Document Failed, Please Check Your Connection !");
                        
                        successUpload = false;
                        isLoading = false;
                        alertTrigger = true;
                        successAlert = false;
                        alertMessage = "Delete Document Failed !";
                        alertBody = "";

                        StateHasChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                isLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", $"Delete Document Failed, Error : {ex.Message}");
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
