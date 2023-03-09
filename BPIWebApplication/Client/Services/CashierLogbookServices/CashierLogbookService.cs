using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.CashierLogbook;
using BPIWebApplication.Shared.MainModel.Company;
using BPIWebApplication.Shared.PagesModel.CashierLogbook;
using System.Net.Http.Json;

namespace BPIWebApplication.Client.Services.CashierLogbookServices
{
    public class CashierLogbookService : ICashierLogbookService
    {
        private readonly HttpClient _http;

        public CashierLogbookService(HttpClient http)
        {
            _http = http;
        }

        public List<Shift> Shifts { get; set; } = new();
        public List<AmountTypes> types { get; set; } = new();
        public List<AmountCategories> categories { get; set; } = new();
        public List<AmountSubCategories> subCategories { get; set; } = new();
        public List<CashierLogData> mainLogs { get; set; } = new();
        public List<CashierLogData> transitLogs { get; set; } = new();
        public List<CashierLogAction> actionLogs { get; set; } = new();

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public async Task<ResultModel<QueryModel<CashierLogData>>> createLogData(QueryModel<CashierLogData> data)
        {
            ResultModel<QueryModel<CashierLogData>> resData = new ResultModel<QueryModel<CashierLogData>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<CashierLogData>>("api/endUser/CashierLogbook/createLogData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<CashierLogData>>>();

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

        public async Task<ResultModel<QueryModel<CashierLogData>>> editLogData(QueryModel<CashierLogData> data)
        {
            ResultModel<QueryModel<CashierLogData>> resData = new ResultModel<QueryModel<CashierLogData>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<CashierLogData>>("api/endUser/CashierLogbook/editLogData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<CashierLogData>>>();

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

        public async Task<ResultModel<List<Shift>>> getShiftData(string moduleName)
        {
            ResultModel<List<Shift>> resData = new ResultModel<List<Shift>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Shift>>>($"api/endUser/CashierLogbook/getShiftbyModule/{moduleName}");

                if (result.isSuccess)
                {
                    Shifts = result.Data;

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

        public async Task<ResultModel<CashierLogbookCategories>> getLogbookCategories()
        {
            ResultModel<CashierLogbookCategories> resData = new ResultModel<CashierLogbookCategories>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<CashierLogbookCategories>>($"api/endUser/CashierLogbook/getCashierLogbookCategories");

                if (result.isSuccess)
                {
                    categories = result.Data.categories;
                    subCategories = result.Data.subCategories;
                    types = result.Data.types;

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

        public async Task<ResultModel<List<CashierLogData>>> getLogData(string locPage)
        {
            ResultModel<List<CashierLogData>> resData = new ResultModel<List<CashierLogData>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<CashierLogData>>>($"api/endUser/CashierLogbook/getLogData/{locPage}");

                if (result.isSuccess)
                {
                    if (Base64Decode(locPage).Split("!_!")[0].Contains("MAIN"))
                    {
                        mainLogs = result.Data;
                    }
                    else if (Base64Decode(locPage).Split("!_!")[0].Contains("TRANSIT"))
                    {
                        transitLogs = result.Data;
                    }

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

        public async Task<ResultModel<List<CashierLogAction>>> getBrankasActionLogData(string locPage)
        {
            ResultModel<List<CashierLogAction>> resData = new ResultModel<List<CashierLogAction>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<CashierLogAction>>>($"api/endUser/CashierLogbook/getBrankasActionLogData/{locPage}");

                if (result.isSuccess)
                {
                    actionLogs = result.Data;

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

        public async Task<ResultModel<QueryModel<CashierLogApproval>>> editBrankasApproveLogOnConfirm(QueryModel<CashierLogApproval> data)
        {
            ResultModel<QueryModel<CashierLogApproval>> resData = new ResultModel<QueryModel<CashierLogApproval>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<CashierLogApproval>>("api/endUser/CashierLogbook/editBrankasApproveLogOnConfirm", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<CashierLogApproval>>>();

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

        public async Task<ResultModel<QueryModel<string>>> updateBrankasDocumentStatusData(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> resData = new ResultModel<QueryModel<string>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/endUser/CashierLogbook/editBrankasDocumentStatus", data);

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
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/endUser/CashierLogbook/getModulePageSize/{Table}");

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


        //
    }
}
