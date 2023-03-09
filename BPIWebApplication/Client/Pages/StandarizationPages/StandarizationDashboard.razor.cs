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
            standarizationNumberofPage = 1;

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

        //
    }
}
