using BPIWebApplication.Client.Services.CashierLogbookServices;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.Standarizations;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.StandarizationPages
{
    public partial class StandarizationDashboard : ComponentBase
    {
        private ActiveUser activeUser = new();

        Standarizations previewData = new();

        private int standarizationPageActive = 0;
        private int standarizationNumberofPage = 0;

        private bool showPreviewModal = false;
        private bool isLoading = false;
        private bool isFilterActive = false;

        private string standarizationFilterType { get; set; } = string.Empty;
        private string standarizationFilterValue { get; set; } = string.Empty;
        private string standarizationFilterSelectValue { get; set; } = string.Empty;

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

            standarizationPageActive = 1;
            string conditions = "!_!" + standarizationPageActive.ToString();
            string mainpz = "StandarizationDetails!_!" + activeUser.location + "!_!";

            standarizationNumberofPage = await StandarizationService.getModulePageSize(Base64Encode(mainpz));
            await StandarizationService.getStandarizations(Base64Encode(conditions));

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/StandarizationPages/StandarizationDashboard.razor.js");
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
                    string filename = data.FilePath.Split("!_!")[1] + data.FileExtention;

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

        private void editDocument(Standarizations data)
        {
            string param = Base64Encode(data.StandarizationID);

            navigate.NavigateTo($"standarization/editstandarization/{param}");
        }

        private async Task previewStandarization(Standarizations data)
        {
            try
            {
                showPreviewModal = true;
                isLoading = true;
                StandarizationService.fileStreams.Clear();

                previewData = data;

                string temp = data.StandarizationID + "!_!" + data.TypeID;

                await Task.Run(async () => { await StandarizationService.getFileStream(Base64Encode(temp)); });

                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                isLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message}");
            }
        }

        private async Task standarizationPageSelect(int currPage)
        {
            standarizationPageActive = currPage;
            isLoading = true;

            if (isFilterActive)
            {
                string conditions = "";
                string temp = conditions+ "!_!" + standarizationPageActive.ToString();

                await StandarizationService.getStandarizations(Base64Encode(temp));
            }
            else
            {
                string temp = "!_!" + standarizationPageActive.ToString();

                await StandarizationService.getStandarizations(Base64Encode(temp));
            }

            isLoading = false;
        }

        private async Task standarizationFilter()
        {
            if (standarizationFilterType.Length > 0)
            {
                standarizationPageActive = 1;
                isFilterActive = true;
                isLoading = true;

                string conditions = "";
                string temp = "";
                string mainpz = "";

                if (standarizationFilterType.Equals("TypeID"))
                {
                    conditions = $"WHERE {standarizationFilterType} LIKE \'%{standarizationFilterSelectValue}%\'";
                    temp = conditions + "!_!" + standarizationPageActive.ToString();

                    mainpz = "StandarizationDetails!_!" + activeUser.location + $"!_!WHERE {standarizationFilterType} LIKE \'%{standarizationFilterSelectValue}%\'";
                }
                else if (standarizationFilterType.Equals("TagDescriptions"))
                {
                    conditions = $"WHERE StandarizationID IN (SELECT StandarizationID FROM StandarizationTags WHERE CONTAINS({standarizationFilterType}, \'{standarizationFilterValue}\'))";
                    temp = conditions + "!_!" + standarizationPageActive.ToString();

                    mainpz = "StandarizationDetails!_!" + activeUser.location + $"!_!WHERE StandarizationID IN (SELECT StandarizationID FROM StandarizationTags WHERE CONTAINS({standarizationFilterType}, \'{standarizationFilterValue}\'))";
                }
                else
                {
                    conditions = $"WHERE {standarizationFilterType} LIKE \'%{standarizationFilterValue}%\'";
                    temp = conditions + "!_!" + standarizationPageActive.ToString();

                    mainpz = "StandarizationDetails!_!" + activeUser.location + $"!_!WHERE {standarizationFilterType} LIKE \'%{standarizationFilterValue}%\'";
                }

                StandarizationService.standarizations.Clear();
                standarizationNumberofPage = await StandarizationService.getModulePageSize(Base64Encode(mainpz));
                await StandarizationService.getStandarizations(Base64Encode(temp));

                isLoading = false;
                StateHasChanged();
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }
        }

        private async Task standarizationFilterReset()
        {
            isLoading = true;
            standarizationPageActive = 1;
            isFilterActive = false;
            standarizationFilterType = "";
            standarizationFilterValue = "";
            standarizationFilterSelectValue = "";

            string conditions = "!_!" + standarizationPageActive.ToString();
            string mainpz = "StandarizationDetails!_!" + activeUser.location + "!_!";

            standarizationNumberofPage = await StandarizationService.getModulePageSize(Base64Encode(mainpz));
            await StandarizationService.getStandarizations(Base64Encode(conditions));

            isLoading = false;
            StateHasChanged();
        }

        private bool checkStandarizationPresent()
        {
            try
            {
                if (StandarizationService.standarizations.Any())
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
