using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.PettyCash;
using BPIWebApplication.Shared.MainModel.Company;
using System.Net.Http.Json;
using BPIWebApplication.Shared.PagesModel.PettyCash;

namespace BPIWebApplication.Client.Services.PettyCashServices
{
    public class PettyCashService : IPettyCashService
    {
        private readonly HttpClient _http;

        public PettyCashService(HttpClient http)
        {
            _http = http;
        }

        public List<Advance> advances { get; set; } = new();
        //public List<AdvanceLine> advanceLines { get; set; } = new();
        public List<Expense> expenses { get; set; } = new();
        //public List<ExpenseLine> expenseLines { get; set; } = new();
        public List<Reimburse> reimburses { get; set; } = new();
        public List<Advance> padvances { get; set; } = new();
        public List<Expense> pexpenses { get; set; } = new();
        public List<Reimburse> preimburses { get; set; } = new();
        public List<Account> coas { get; set; } = new();

        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileStreams { get; set; } = new();

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

        public async Task<ResultModel<string>> createDocumentID(string docType)
        {
            ResultModel<string> resData = new ResultModel<string>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/endUser/PettyCash/createID/{docType}");

                if (result.isSuccess)
                {
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

        public async Task<ResultModel<QueryModel<Advance>>> createAdvanceData(QueryModel<Advance> data)
        {
            ResultModel<QueryModel<Advance>> resData = new ResultModel<QueryModel<Advance>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Advance>>("api/endUser/PettyCash/createAdvance", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Advance>>>();

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

        public async Task<ResultModel<QueryModel<Expense>>> createExpenseData(ExpenseStream data)
        {
            ResultModel<QueryModel<Expense>> resData = new ResultModel<QueryModel<Expense>>();

            try
            {
                var result = await _http.PostAsJsonAsync<ExpenseStream>("api/endUser/PettyCash/createExpense", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Expense>>>();

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

        public async Task<ResultModel<QueryModel<Reimburse>>> createReimburseData(ReimburseStream data)
        {
            ResultModel<QueryModel<Reimburse>> resData = new ResultModel<QueryModel<Reimburse>>();

            try
            {
                var result = await _http.PostAsJsonAsync<ReimburseStream>("api/endUser/PettyCash/createReimburse", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Reimburse>>>();

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

        // get
        public async Task<ResultModel<QueryModel<string>>> updateAdvanceDataSettlement(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> resData = new ResultModel<QueryModel<string>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/endUser/PettyCash/AdvanceSettlement", data);

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

        public async Task<ResultModel<QueryModel<List<string>>>> updateExpenseDataSettlement(QueryModel<List<string>> data)
        {
            ResultModel<QueryModel<List<string>>> resData = new ResultModel<QueryModel<List<string>>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<List<string>>>("api/endUser/PettyCash/ExpenseSettlement", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<List<string>>>>();

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

        public async Task<ResultModel<QueryModel<string>>> updateReimburseDataSettlement(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> resData = new ResultModel<QueryModel<string>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/endUser/PettyCash/ReimburseSettlement", data);

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

        public async Task<ResultModel<QueryModel<string>>> updateDocumentStatus(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> resData = new ResultModel<QueryModel<string>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/endUser/PettyCash/editDocumentStatus", data);

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
                    else
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

        public async Task<List<ResultModel<ReimbursementMultiSelectStatusUpdate>>> editMultiSelectDocumentStatus(List<ReimbursementMultiSelectStatusUpdate> data)
        {
            List<ResultModel<ReimbursementMultiSelectStatusUpdate>> resData = new();

            try
            {
                var result = await _http.PostAsJsonAsync<List<ReimbursementMultiSelectStatusUpdate>>("api/endUser/PettyCash/editMultiSelectDocumentStatus", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<List<ResultModel<ReimbursementMultiSelectStatusUpdate>>>();

                    resData = respBody;
                }
            }
            catch (Exception ex)
            {
                resData.Add(new ResultModel<ReimbursementMultiSelectStatusUpdate>
                {
                    Data = null,
                    isSuccess = false,
                    ErrorCode = "99",
                    ErrorMessage = "ERROR : REASON " + ex.Message
                });
            }

            return resData;
        }

        public async Task<ResultModel<QueryModel<Reimburse>>> updateReimburseLineData(QueryModel<Reimburse> data)
        {
            ResultModel<QueryModel<Reimburse>> resData = new ResultModel<QueryModel<Reimburse>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Reimburse>>("api/endUser/PettyCash/editReimburseLine", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Reimburse>>>();

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

        public async Task<ResultModel<List<Account>>> getCoabyModule(string moduleName)
        {
            ResultModel<List<Account>> resData = new ResultModel<List<Account>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Account>>>($"api/endUser/PettyCash/getCoabyModule/{moduleName}");

                if (result.isSuccess)
                {
                    coas.Clear();
                    coas = result.Data;

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

        public async Task<ResultModel<LocationBalanceDetails>> getPettyCashOutstandingAmount(string loc)
        {
            ResultModel<LocationBalanceDetails> resData = new ResultModel<LocationBalanceDetails>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<LocationBalanceDetails>>($"api/endUser/PettyCash/getPettyCashOutstandingAmountandLocBalanceDetails/{loc}");

                if (result.isSuccess)
                {
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

        //public async Task<ResultModel<BalanceDetails>> getLocationBudgetDetails(string loc)
        //{
        //    ResultModel<BalanceDetails> resData = new ResultModel<BalanceDetails>();

        //    try
        //    {
        //        var result = await _http.GetFromJsonAsync<ResultModel<BalanceDetails>>($"api/endUser/PettyCash/getLocationBudgetDetails/{loc}");

        //        if (result.isSuccess)
        //        {
        //            resData.Data = result.Data;
        //            resData.isSuccess = result.isSuccess;
        //            resData.ErrorCode = result.ErrorCode;
        //            resData.ErrorMessage = result.ErrorMessage;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resData.Data = null;
        //        resData.isSuccess = false;
        //        resData.ErrorCode = "99";
        //        resData.ErrorMessage = ex.Message;
        //    }
        //    return resData;
        //}

        public async Task<ResultModel<QueryModel<BalanceDetails>>> updateLocationBudgetDetails(QueryModel<BalanceDetails> data)
        {
            ResultModel<QueryModel<BalanceDetails>> resData = new ResultModel<QueryModel<BalanceDetails>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<BalanceDetails>>("api/endUser/PettyCash/updateLocationBudget", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<BalanceDetails>>>();

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

        public async Task<ResultModel<QueryModel<CutoffDetails>>> updateLocationCutoffDate(QueryModel<CutoffDetails> data)
        {
            ResultModel<QueryModel<CutoffDetails>> resData = new ResultModel<QueryModel<CutoffDetails>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<CutoffDetails>>("api/endUser/PettyCash/updateLocationCutoffDate", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<CutoffDetails>>>();

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

        public async Task<ResultModel<List<Advance>>> getAdvanceDatabyLocation(string locPage)
        {
            ResultModel<List<Advance>> resData = new ResultModel<List<Advance>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Advance>>>($"api/endUser/PettyCash/getAdvanceDatabyLocation/{locPage}");

                if (result.isSuccess)
                {
                    if (Base64Decode(locPage).Split("!_!")[0].Contains("MASTER"))
                    {
                        advances = result.Data;
                    }
                    else if (Base64Decode(locPage).Split("!_!")[0].Contains("POSTED"))
                    {
                        padvances = result.Data;
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

        public async Task<ResultModel<List<Advance>>> getAdvanceDatabyUser(string locPage)
        {
            ResultModel<List<Advance>> resData = new ResultModel<List<Advance>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Advance>>>($"api/endUser/PettyCash/getAdvanceDatabyUser/{locPage}");

                if (result.isSuccess)
                {
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

        public async Task<ResultModel<List<Expense>>> getExpenseDatabyLocation(string locPage)
        {
            ResultModel<List<Expense>> resData = new ResultModel<List<Expense>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Expense>>>($"api/endUser/PettyCash/getExpenseDatabyLocation/{locPage}");

                if (result.isSuccess)
                {
                    if (Base64Decode(locPage).Split("!_!")[0].Contains("MASTER"))
                    {
                        expenses = result.Data;
                    }
                    else if (Base64Decode(locPage).Split("!_!")[0].Contains("POSTED"))
                    {
                        pexpenses = result.Data;
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

        public async Task<ResultModel<List<Reimburse>>> getReimburseDatabyLocation(string locPage)
        {
            ResultModel<List<Reimburse>> resData = new ResultModel<List<Reimburse>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Reimburse>>>($"api/endUser/PettyCash/getReimburseDatabyLocation/{locPage}");

                if (result.isSuccess)
                {
                    if (Base64Decode(locPage).Split("!_!")[0].Contains("MASTER"))
                    {
                        reimburses = result.Data;
                    }
                    else if (Base64Decode(locPage).Split("!_!")[0].Contains("POSTED"))
                    {
                        preimburses = result.Data;
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

        //public async Task<ResultModel<List<AdvanceLine>>> getAdvanceLinesbyID(string AdvanceId)
        //{
        //    ResultModel<List<AdvanceLine>> resData = new ResultModel<List<AdvanceLine>>();

        //    try
        //    {
        //        var result = await _http.GetFromJsonAsync<ResultModel<List<AdvanceLine>>>($"api/endUser/PettyCash/getAdvanceLinesbyID/{AdvanceId}");

        //        if (result.isSuccess)
        //        {
        //            advanceLines = result.Data;

        //            resData.Data = result.Data;
        //            resData.isSuccess = result.isSuccess;
        //            resData.ErrorCode = result.ErrorCode;
        //            resData.ErrorMessage = result.ErrorMessage;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resData.Data = null;
        //        resData.isSuccess = false;
        //        resData.ErrorCode = "99";
        //        resData.ErrorMessage = ex.Message;
        //    }
        //    return resData;
        //}

        public async Task<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>> getAttachmentFileStream(string Id)
        {
            ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>> resData = new ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>>($"api/endUser/PettyCash/getAttachFileStream/{Id}");

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

        public async Task<ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream>> getLedgerDataEntriesbyDate(List<ledgerParam> data)
        {
            ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream> resData = new ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream>();

            try
            {
                var result = await _http.PostAsJsonAsync<List<ledgerParam>>("api/endUser/PettyCash/getLedgerDataEntriesbyDate", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream>>();

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

        // is

        public async Task<bool> isAdvancePresent(string AdvanceId)
        {
            ResultModel<string> resData = new ResultModel<string>();

            var temp = Base64Encode(AdvanceId);

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/endUser/PettyCash/isAdvanceDataPresent/{temp}");

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
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData.isSuccess;
        }

        public async Task<int> getModulePageSize(string Table)
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/endUser/PettyCash/getModulePageSize/{Table}");

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

        public async Task<ResultModel<bool>> autoEmail(string param)
        {
            ResultModel<bool> resData = new ResultModel<bool>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<bool>>($"api/endUser/PettyCash/getMailingDetails/{param}");

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
                resData.Data = false;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<int> getPettyCashMaxSizeUpload()
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/endUser/PettyCash/getPettyCashMaxSizeUpload");

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
