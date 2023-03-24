using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Standarizations;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Net.Http.Json;

namespace BPIWebApplication.Client.Services.StandarizationServices
{
    public class StandarizationService : IStandarizationService
    {
        private readonly HttpClient _http;

        public StandarizationService(HttpClient http)
        {
            _http = http;
        }

        public List<StandarizationType> standarizationTypes { get; set; } = new();
        public List<Standarizations> standarizations { get; set; } = new();
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileStreams { get; set; } = new();

        public async Task<ResultModel<List<StandarizationType>>> getStandarizationTypes()
        {
            ResultModel<List<StandarizationType>> resData = new ResultModel<List<StandarizationType>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<StandarizationType>>>("api/endUser/Standarization/getStandarizationTypes");

                if (result.isSuccess)
                {
                    standarizationTypes.Clear();
                    standarizationTypes = result.Data;

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<List<Standarizations>>> getStandarizations(string param)
        {
            ResultModel<List<Standarizations>> resData = new ResultModel<List<Standarizations>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Standarizations>>>($"api/endUser/Standarization/getStandarizationData/{param}");

                if (result.isSuccess)
                {
                    //standarizations.Clear();
                    standarizations = result.Data;

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>> getFileStream(string param)
        {
            ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>> resData = new ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>>($"api/endUser/Standarization/getStandarizationAttachment/{param}");

                if (result.isSuccess)
                {
                    fileStreams.AddRange(result.Data);

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }
            return resData;
        }

        public async Task<ResultModel<QueryModel<Standarizations>>> createStandarizations(StandarizationStream data)
        {
            ResultModel<QueryModel<Standarizations>> resData = new ResultModel<QueryModel<Standarizations>>();

            try
            {
                var result = await _http.PostAsJsonAsync<StandarizationStream>("api/endUser/Standarization/createStandarizationData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Standarizations>>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<QueryModel<Standarizations>>> updateStandarizations(StandarizationStream data)
        {
            ResultModel<QueryModel<Standarizations>> resData = new ResultModel<QueryModel<Standarizations>>();

            try
            {
                var result = await _http.PostAsJsonAsync<StandarizationStream>("api/endUser/Standarization/editStandarizationData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Standarizations>>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<QueryModel<string>>> deleteStandarizations(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> resData = new ResultModel<QueryModel<string>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/endUser/Standarization/deleteStandarizationData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<string>>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<int> getModulePageSize(string Table)
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/endUser/Standarization/getModulePageSize/{Table}");

                if (result.isSuccess)
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;

                }
                else
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = 0;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData.Data;
        }

        public async Task<int> getStandarizationMaxFileSize()
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/endUser/Standarization/getStandarizationMaxSizeUpload");

                if (result.isSuccess)
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;

                }
                else
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = 0;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData.Data;
        }

        public async Task<string[]> getStandarizationAcceptedFileExtension()
        {
            ResultModel<string[]> resData = new ResultModel<string[]>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string[]>>($"api/endUser/Standarization/getStandarizationAcceptedFileExtension");

                if (result.isSuccess)
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;

                }
                else
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = new string[] {};
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData.Data;
        }

        //
    }
}
