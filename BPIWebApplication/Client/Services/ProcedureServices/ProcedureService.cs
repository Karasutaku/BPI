using BPIWebApplication.Shared;
using BPIWebApplication.Shared.FileUploadModel;
using BPIWebApplication.Shared.PagesModel.ApplyProcedure;
using BPIWebApplication.Shared.DbModel;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using BPIWebApplication.Client.Pages.SopPages;
using BPIWebApplication.Shared.PagesModel.Dashboard;
using BPIWebApplication.Shared.PagesModel.AccessHistory;
using BPIWebApplication.Shared.ReportModel;

namespace BPIWebApplication.Client.Services.ProcedureServices
{
    public class ProcedureService : IProcedureService
    {
        private readonly HttpClient _http;
        public ProcedureService(HttpClient http)
        {
            _http = http;
        }

        public List<BisnisUnit> bisnisUnits { get; set; } = new List<BisnisUnit>();
        public List<Department> departments { get; set; } = new List<Department>();
        public List<Procedure> procedures { get; set; } = new List<Procedure>();
        public List<HistoryAccess> historyAccess { get; set; } = new List<HistoryAccess>();
        public List<HistoryAccess> historyAccessReport { get; set; } = new List<HistoryAccess>();
        public List<DepartmentProcedure> departmentProcedures { get; set; } = new List<DepartmentProcedure>();
        public bool isProcedurePresent { get; set; }
        

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

        public async Task<bool> checkProsedureExisting(string ProcNo)
        {
            ResultModel<string> resData = new ResultModel<string>();

            var temp = Base64Encode(ProcNo);

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/Procedure/isProcedureDataPresent/{temp}");

                if (result.isSuccess)
                {
                    isProcedurePresent = result.isSuccess;

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    isProcedurePresent = result.isSuccess;

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch(Exception ex)
            {
                isProcedurePresent = false;

                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }
            return resData.isSuccess;
        }

        public async Task<ResultModel<List<BisnisUnit>>> GetAllBisnisUnit()
        {
            ResultModel<List<BisnisUnit>> resData = new ResultModel<List<BisnisUnit>>();

            try {
                var result = await _http.GetFromJsonAsync<ResultModel<List<BisnisUnit>>>("api/Procedure/getAllBisnisUnitData");

                if (result.isSuccess)
                {
                    bisnisUnits = result.Data;

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            } catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }
            return resData;
        }

        public async Task<ResultModel<List<Department>>> GetAllDepartment()
        {
            ResultModel<List<Department>> resData = new ResultModel<List<Department>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Department>>>("api/Procedure/getAllDepartmentData");

                if (result.isSuccess)
                {
                    departments = result.Data;

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

        public async Task<ResultModel<List<Procedure>>> GetAllProcedure()
        {
            ResultModel<List<Procedure>> resData = new ResultModel<List<Procedure>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Procedure>>>("api/Procedure/getAllProcedureData");

                if (result.isSuccess)
                {
                    procedures = result.Data;

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

        public async Task<ResultModel<List<DepartmentProcedure>>> GetAllDepartmentProcedure()
        {
            ResultModel<List<DepartmentProcedure>> resData = new ResultModel<List<DepartmentProcedure>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<DepartmentProcedure>>>($"api/Procedure/getDepartmentProcedureData");

                if (result.isSuccess)
                {
                    departmentProcedures = result.Data;

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

        public async Task<ResultModel<List<DepartmentProcedure>>> GetDepartmentProcedurewithPaging(int pageNo)
        {
            ResultModel<List<DepartmentProcedure>> resData = new ResultModel<List<DepartmentProcedure>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<DepartmentProcedure>>>($"api/Procedure/getDepartmentProcedureDatawithPaging/{pageNo}");

                if (result.isSuccess)
                {
                    departmentProcedures = result.Data;

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
        
        public async Task<ResultModel<List<HistoryAccess>>> GetHistoryAccessbyPaging(int pageNo)
        {
            ResultModel<List<HistoryAccess>> resData = new ResultModel<List<HistoryAccess>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<HistoryAccess>>>($"api/Procedure/getAllHistoryAccessDatawithPaging/{pageNo}");

                if (result.isSuccess)
                {
                    historyAccess = result.Data;

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

        public async Task<ResultModel<FileReadyDownload>> GetFile(string path)
        {
            ResultModel<FileReadyDownload> resData = new ResultModel<FileReadyDownload>();
            var temp = Base64Encode(path);
            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<FileReadyDownload>>($"api/Procedure/getFile/{temp}");

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

        public async Task<ResultModel<ProcedureUpload>> createProcedure(ProcedureUpload data)
        {
            ResultModel<ProcedureUpload> resData = new ResultModel<ProcedureUpload>();
            
            try
            {
                var result = await _http.PostAsJsonAsync<ProcedureUpload>("api/Procedure/createProcedureData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<ProcedureUpload>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                }
            }
            catch(Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<QueryModel<ApplyProcedureMultiDept>>> createDepartmentProcedure(QueryModel<ApplyProcedureMultiDept> data)
        {
            ResultModel<QueryModel<ApplyProcedureMultiDept>> resData = new ResultModel<QueryModel<ApplyProcedureMultiDept>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<ApplyProcedureMultiDept>>("api/Procedure/createDepartmentProcedureData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<ApplyProcedureMultiDept>>>();

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

        public async Task<ResultModel<QueryModel<HistoryAccess>>> createHistoryAccess(QueryModel<HistoryAccess> data)
        {
            ResultModel<QueryModel<HistoryAccess>> resData = new ResultModel<QueryModel<HistoryAccess>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<HistoryAccess>>("api/Procedure/createHistoryAccessData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<HistoryAccess>>>();

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

        public async Task<ResultModel<ProcedureUpload>> editProcedure(ProcedureUpload data)
        {
            ResultModel<ProcedureUpload> resData = new ResultModel<ProcedureUpload>();

            try
            {
                var result = await _http.PostAsJsonAsync<ProcedureUpload>("api/Procedure/editProcedureData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<ProcedureUpload>>();

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

        public async Task<ResultModel<QueryModel<ApplyProcedureMultiDept>>> deleteDepartmentProcedure(QueryModel<ApplyProcedureMultiDept> data)
        {
            ResultModel<QueryModel<ApplyProcedureMultiDept>> resData = new ResultModel<QueryModel<ApplyProcedureMultiDept>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<ApplyProcedureMultiDept>>("api/Procedure/deleteDepartmentProcedureData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<ApplyProcedureMultiDept>>>();

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

        public async Task<int> getHistoryAccessNumberofPage()
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/Procedure/getHistoryAccessNumberofPage");

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

        public async Task<int> getDepartmentProcedureNumberofPage()
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/Procedure/getDepartmentProcedureNumberofPage");

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

        public async Task<int> getDepartmentProcedurewithFilterNumberofPage(DashboardFilter data)
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.PostAsJsonAsync<DashboardFilter>($"api/Procedure/getDepartmentProcedurewithFilterNumberofPage", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<int>>();

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
                resData.Data = 0;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData.Data;
        }

        public async Task<ResultModel<List<DepartmentProcedure>>> GetDepartmentProcedurewithFilterbyPaging(DashboardFilter data)
        {
            ResultModel<List<DepartmentProcedure>> resData = new ResultModel<List<DepartmentProcedure>>();

            try
            {
                var result = await _http.PostAsJsonAsync<DashboardFilter>("api/Procedure/getDepartmentProcedureDatawithFilterbyPaging", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<DepartmentProcedure>>>();

                    if (respBody.isSuccess)
                    {
                        departmentProcedures = respBody.Data;

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

        public async Task<ResultModel<List<HistoryAccess>>> GetHistoryAccessbyFilterwithPaging(AccessHistoryFilter data)
        {
            ResultModel<List<HistoryAccess>> resData = new ResultModel<List<HistoryAccess>>();

            try
            {
                var result = await _http.PostAsJsonAsync<AccessHistoryFilter>("api/Procedure/getAllHistoryAccessDatabyFilterwithPaging", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<HistoryAccess>>>();

                    if (respBody.isSuccess)
                    {
                        historyAccess = respBody.Data;

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

        public async Task<int> getAccessHistorywithFilterNumberofPage(AccessHistoryFilter data)
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.PostAsJsonAsync<AccessHistoryFilter>($"api/Procedure/getHistoryAccessbyFilterwithPagingNumberofPage", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<int>>();

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
                resData.Data = 0;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData.Data;
        }

        public async Task<ResultModel<List<HistoryAccess>>> GetHistoryAccessReportbyFilter(AccessHistoryReport data)
        {
            ResultModel<List<HistoryAccess>> resData = new ResultModel<List<HistoryAccess>>();

            try
            {
                var result = await _http.PostAsJsonAsync<AccessHistoryReport>("api/Procedure/getAllHistoryAccessDataReportbyFilter", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<HistoryAccess>>>();

                    if (respBody.isSuccess)
                    {
                        historyAccessReport = respBody.Data;

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

        // config

        public async Task<long> getProcedureMaxFileSize()
        {
            ResultModel<long> resData = new ResultModel<long>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<long>>($"api/Procedure/getProcedureMaxSizeUpload");

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
    }
}
